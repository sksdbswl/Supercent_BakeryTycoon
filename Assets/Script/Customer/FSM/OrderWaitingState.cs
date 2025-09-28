using System.Collections;
using UnityEngine;

public class OrderWaitingState : CustomerBaseState
{
    private NavPoint targetPoint;
    private bool wantsToEatIn;
    private QueueManager.QueueType myQueueType;

    public OrderWaitingState(CustomerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Customer.isPickingAnimationPlayed = false;
        wantsToEatIn = stateMachine.Customer.customerData.wantsToEatIn;
        myQueueType = wantsToEatIn ? QueueManager.QueueType.Dining : QueueManager.QueueType.Cashier;
        
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.StackMove);
        stateMachine.Customer.CustomerUI.OnSprite(stateMachine.Customer.CustomerUI.Cashier);
        
        // 대기 루틴 시작
        stateMachine.Customer.StartCoroutine(WaitForQueuePoint());
    }

    private IEnumerator WaitForQueuePoint()
    {
        // 포인트 확보될 때까지 반복
        while (targetPoint == null)
        {
            targetPoint = wantsToEatIn
                ? QueueManager.Instance.RequestDiningPoint(stateMachine.Customer)
                : QueueManager.Instance.RequestCashierPoint(stateMachine.Customer);

            if (targetPoint == null)
                yield return new WaitForSeconds(0.5f);
        }

        // 포인트 확보 후 이동
        var customer = stateMachine.Customer;
        stateMachine.Customer.MoveToNavAgentPoint(targetPoint.transform);
        customer.animator.SetTrigger(CustomerAnimationController.StackMove);

        // 이동 완료될 때까지 대기
        while (!customer.ArriveCheck())
        {
            yield return null;
        }

        // 이동 완료 후 Idle 애니메이션
        customer.animator.SetTrigger(CustomerAnimationController.StackIdle);

        // 맨 앞 자리 될 때까지 대기
        while (!QueueManager.Instance.CheckMyTurn(customer, myQueueType))
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // 상태 전환
        if (wantsToEatIn)
            stateMachine.ChangeState(stateMachine.EatState);
        else
            stateMachine.ChangeState(stateMachine.BuyState);
    }
}