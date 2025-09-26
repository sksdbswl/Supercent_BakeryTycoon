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
        targetShowcase = ShowCaseManager.Instance.GetRandomShowcase();
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
        stateMachine.Customer.navAgent.SetDestination(targetPoint.transform.position);
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Move);
    }

    public override void Update()
    {
        if (targetPoint == null || targetShowcase == null) return;

        if (!stateMachine.Customer.ArriveCheck()) return;
        IsArrived = true;
        if (!IsArrived) return;
        IsArrived = false;
        if (targetShowcase.IsBusy) return;

        // 쇼케이스 사용 시작
        PickUpBread();
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
            stateMachine.Customer.navAgent.SetDestination(targetPoint.transform.position);
    }

    public void PickUpBread()
    {
        int need = stateMachine.Customer.customerData.quantity;
        int already = stateMachine.Customer.customerData.pickedUpCount;

        for (int i = already; i < need; i++)
        {
            if (targetShowcase.IsBusy)
            {
                Debug.Log("쇼케이스 사용 중 → 픽업 중단");
                break;
            }

            Product bread = targetShowcase.GetProduct();
            if (bread == null)
            {
                //Debug.Log("쇼케이스 빵이 부족함 → 픽업 중단");
                break;
            }

            // 빵 가져가기
            stateMachine.Customer.customerData.pickedUpCount++;
            bread.MoveTo(stateMachine.Customer, Product.GoalType.Customer);
        }
    
        if (stateMachine.Customer.customerData.pickedUpCount >= need)
        {
            //Debug.Log("손님이 필요한 빵을 모두 픽업 완료!");
            stateMachine.ChangeState(stateMachine.OrderWaitingState);
        }
    }
}