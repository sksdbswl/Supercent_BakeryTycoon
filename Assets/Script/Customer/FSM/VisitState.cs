using UnityEngine;
using UnityEngine.AI;

public class VisitState : CustomerBaseState
{
    private Showcase targetShowcase;
    private NavPoint targetPoint;

    public VisitState(CustomerStateMachine stateMachine) : base(stateMachine) { }

    private bool IsArrived = false;
    
    public override void Enter()
    {
        // 랜덤 쇼케이스 선택
        targetShowcase = AreaManager.Instance.GetRandomShowcase();
        if (targetShowcase == null)
        {
            Debug.LogWarning("쇼케이스 없음!");
            return;
        }

        targetPoint = targetShowcase.GetFreePoint();

        // 쇼케이스에서 빈 자리 가져오기
        if (targetPoint == null)
        {
            Debug.Log("쇼케이스 자리가 없음, 대기 또는 다른 행동");
            return;
        }

        // 이동
        stateMachine.Customer.navAgent.SetDestination(targetPoint.Point.position);
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Move);
    }

    public override void Update()
    {
        if (targetPoint == null || targetShowcase == null) return;

        if (!stateMachine.Customer.navAgent.pathPending &&
            stateMachine.Customer.navAgent.remainingDistance <= 0.1f)
        {
            IsArrived = true;
            // 쇼케이스에서 빵 가져오기
            if (IsArrived)
            {
                IsArrived = false;
                //PickUpBread();
            }
        }
    }

    public override void Exit()
    {
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);

        // 자리 반환
        if (targetPoint != null)
            targetPoint.IsOccupied = false;
    }
    
    public void SetTargetShowcase(Showcase showcase)
    {
        targetShowcase = showcase;

        if (targetPoint != null)
            stateMachine.Customer.navAgent.SetDestination(targetPoint.Point.position);
    }

    public void PickUpBread()
    {
        for (int i = 0; i < stateMachine.Customer.customerData.quantity; i++)
        {
            Product bread = targetShowcase.GetProduct();
            if (bread != null)
            {
                // Debug.Log("빵 픽업 완료: " + bread.name);
                // stateMachine.Customer.PickedUpBreads.Push(bread);
                // 쇼케이스에 있는 빵 수량 제거
            }
        }
    }
}