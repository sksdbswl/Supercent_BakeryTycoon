using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderWaitingState:CustomerBaseState
{
    private NavPoint targetPoint;
    private bool OrderType;
    
    public OrderWaitingState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        OrderType = stateMachine.Customer.customerData.wantsToEatIn;
        
        if(OrderType)
        {
            //나 밥먹고 갈테야
            targetPoint = QueueManager.Instance.RequestDiningPoint(stateMachine.Customer);
        }
        else
        {
            //나 포장하고 계산해줘
            targetPoint = QueueManager.Instance.RequestCashierPoint(stateMachine.Customer);
        }
        
        stateMachine.Customer.StartCoroutine(CheckoutRoutine());
        stateMachine.Customer.navAgent.SetDestination(targetPoint.transform.position);
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Move);
    }
    
    public override void Update()
    {
        if (!stateMachine.Customer.ArriveCheck()) return;
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);
    }
    
    private IEnumerator CheckoutRoutine()
    {
        while (true)
        {
            // 맨 앞 자리인지 확인
            bool check = QueueManager.Instance.CheckMyTurn(stateMachine.Customer);
            
            if (check && !OrderType)
            {
                yield return new WaitForSeconds(0.5f);
                stateMachine.ChangeState(stateMachine.BuyState); 
                yield break;
            }
            
            if (check && OrderType)
            {
                yield return new WaitForSeconds(0.5f);
                Debug.Log("내가 밥먹을 차례야");
                stateMachine.ChangeState(stateMachine.EatState); 
                yield break;
            }
            
            yield return null; 
        }
    }
}