using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOutState:CustomerBaseState
{
    private NavPoint targetPoint;
    
    public CheckOutState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        //targetPoint = AreaManager.Instance.GetFreePoint(AreaManager.Instance.CheckoutPoints);
        
        if (targetPoint != null)
        {
            stateMachine.Customer.navAgent.SetDestination(targetPoint.transform.position);
            stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Move);
        }
        
        //일정 시간이 지나면 떠나
        stateMachine.Customer.StartCoroutine(CheckoutRoutine());
    }
    
    public override void Update()
    {
    }

    private IEnumerator CheckoutRoutine()
    {
        // 10초 대기
        yield return new WaitForSeconds(20f);
        stateMachine.ChangeState(stateMachine.LeavingState);
    }
    
    
    public override void Exit()
    {
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Move);
    }
}