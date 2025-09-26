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
        wantsToEatIn = stateMachine.Customer.customerData.wantsToEatIn;
        myQueueType = wantsToEatIn ? QueueManager.QueueType.Dining : QueueManager.QueueType.Cashier;

        // 대기 루틴 시작
        stateMachine.Customer.StartCoroutine(WaitForQueuePoint());
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Move);
    }

    public override void Update()
    {
        if (!stateMachine.Customer.ArriveCheck()) return;
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);
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
        stateMachine.Customer.navAgent.SetDestination(targetPoint.transform.position);

        // 맨 앞 자리 될 때까지 대기
        while (!QueueManager.Instance.CheckMyTurn(stateMachine.Customer, myQueueType))
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // 상태 전환
        if (wantsToEatIn)
        {
            Debug.Log("내가 밥먹을 차례야");
            stateMachine.ChangeState(stateMachine.EatState);
        }
        else
        {
            stateMachine.ChangeState(stateMachine.BuyState);
        }
    }
}