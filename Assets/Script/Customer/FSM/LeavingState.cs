using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingState : CustomerBaseState
{
    public LeavingState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        Debug.Log("감사합니다:)");
        stateMachine.Customer.CustomerUI.OnLike();
        var customer = stateMachine.Customer;
        
        // 출발점으로 이동
        customer.navAgent.SetDestination(GameManager.Instance.customerSpawner.spawnPosition.position);
        customer.animator.SetTrigger(CustomerAnimationController.Move);

        stateMachine.Customer.StartCoroutine(WaitToReturnPool(stateMachine.Customer));
    }

    private IEnumerator WaitToReturnPool(Customer customer)
    {
        while (!stateMachine.Customer.ArriveCheck())
        {
            yield return null;
        }
        
        // 도착 후 풀로 반환
        GenericPoolManager.Instance.Release(customer.PooledObject.OriginPrefab, customer.PooledObject.gameObject);
        if (customer.currentPaperBox != null)
        {
            GenericPoolManager.Instance.Release(customer.currentPaperBox, customer.currentPaperBox.gameObject);
        }
    }
}