using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingState : CustomerBaseState
{
    public LeavingState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        stateMachine.Customer.CustomerUI.OnLike();
        var customer = stateMachine.Customer;
        
        stateMachine.Customer.MoveToNavAgentPoint(GameManager.Instance.customerSpawner.spawnPosition);
        customer.animator.SetTrigger(CustomerAnimationController.StackMove);

        stateMachine.Customer.StartCoroutine(WaitToReturnPool(stateMachine.Customer));
    }

    private IEnumerator WaitToReturnPool(Customer customer)
    {
        while (!stateMachine.Customer.ArriveCheck())
        {
            yield return null;
        }
        
        if (customer.currentPaperBox != null)
        {
            var origin = customer.currentPaperBox.GetComponent<PooledObject>();
            GenericPoolManager.Instance.Release(origin.OriginPrefab, origin.gameObject);
        }

        if (stateMachine.Customer.PickedUpBreads.Count > 0)
        {
            foreach (var bread in stateMachine.Customer.PickedUpBreads)
            {
                var origin = bread.GetComponent<PooledObject>();
                bread.SetTrigger();
                GenericPoolManager.Instance.Release(origin.OriginPrefab, origin.gameObject);
            }
        }
        
        stateMachine.Customer.CustomerUI.OffState();
        GenericPoolManager.Instance.Release(customer.PooledObject.OriginPrefab, customer.PooledObject.gameObject);

    }
}