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
        
        customer.navAgent.SetDestination(GameManager.Instance.customerSpawner.spawnPosition.position);
        //customer.animator.SetTrigger(CustomerAnimationController.StackMove);

        stateMachine.Customer.StartCoroutine(WaitToReturnPool(stateMachine.Customer));
    }

    private IEnumerator WaitToReturnPool(Customer customer)
    {
        while (!stateMachine.Customer.ArriveCheck())
        {
            yield return null;
        }
        
        GenericPoolManager.Instance.Release(customer.PooledObject.OriginPrefab, customer.PooledObject.gameObject);
        if (customer.currentPaperBox != null)
        {
            GenericPoolManager.Instance.Release(customer.currentPaperBox, customer.currentPaperBox.gameObject);
        }
    }
}