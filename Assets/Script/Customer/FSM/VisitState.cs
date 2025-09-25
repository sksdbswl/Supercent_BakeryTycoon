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
            //TODO::조건
            //쇼케이스에 빵이 있음.
            // 플레이어가 쇼케이스에 빵을 진열하고 있지 않음
            
            if (IsArrived)
            {
                IsArrived = false;
                //Debug.Log($"진열된 빵의 수량 :: {targetShowcase.Products.Count}");
                
                // 쇼케이스가 사용 중이면 대기
                if (targetShowcase.IsBusy) return;

                // 쇼케이스 사용 시작
                PickUpBread();
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
        int need = stateMachine.Customer.customerData.quantity;
        int already = stateMachine.Customer.customerData.pickedUpCount;

        Debug.Log($"손님 빵 픽업 시도 :: 총 {need}개 필요, 이미 {already}개 픽업함");

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
            //stateMachine.Customer.PickedUpBreads.Push(bread);
            stateMachine.Customer.customerData.pickedUpCount++;

            bread.MoveTo(stateMachine.Customer, Product.GoalType.Customer);

            //Debug.Log($"빵 픽업 성공! 현재까지 {stateMachine.Customer.customerData.pickedUpCount}/{need}");
        }
    
        if (stateMachine.Customer.customerData.pickedUpCount >= need)
        {
            //Debug.Log("손님이 필요한 빵을 모두 픽업 완료!");
            // ExitState로 전환 등 다음 행동 지정
            // stateMachine.ChangeState(new ExitState(stateMachine));
        }
    }
}