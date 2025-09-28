using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class VisitState : CustomerBaseState
{
    private Showcase targetShowcase;
    private NavPoint targetPoint;
    private bool isPicking = false;

    public VisitState(CustomerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // 랜덤 쇼케이스 선택 및 빈 자리 확보
        targetShowcase = ShowCaseManager.Instance.GetRandomShowcase();
        if (targetShowcase == null)
        {
            Debug.LogWarning("쇼케이스 없음!");
            return;
        }

        targetPoint = targetShowcase.GetFreePoint();
        if (targetPoint == null)
        {
            Debug.Log("쇼케이스 자리가 없음, 대기 또는 다른 행동");
            return;
        }

        // 이동 시작
        targetShowcase.Customers.Enqueue(stateMachine.Customer);
        stateMachine.Customer.MoveToNavAgentPoint(targetPoint.transform);
    }

    public override void Update()
    {
        if (targetShowcase == null || targetPoint == null || isPicking) return;
        if (!stateMachine.Customer.ArriveCheck()) return;

        // 도착 후 행동
        OnArriveAtShowcase();
    }

    private void OnArriveAtShowcase()
    {
        // UI 업데이트
        var customerUI = stateMachine.Customer.CustomerUI;
        var customerData = stateMachine.Customer.customerData;

        customerUI.SetBreadCount(customerData.quantity);
        customerUI.OnSprite(customerUI.Want);

        // Idle 애니메이션
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);

        // 쇼케이스가 사용 중이면 대기
        if (targetShowcase.IsBusy) return;
        if (targetShowcase.Customers.Peek() != stateMachine.Customer) return;
        
        // 빵 집기 시작
        StartPickUpBread();
    }

    private void StartPickUpBread()
    {
        if (isPicking) return;

        isPicking = true;
        stateMachine.Customer.StartCoroutine(PickUpBreadCoroutine());
    }

    private IEnumerator PickUpBreadCoroutine()
    {
        var customer = stateMachine.Customer;
        var customerUI = customer.CustomerUI;
        var data = customer.customerData;

        while (data.pickedUpCount < data.quantity)
        {
            if (targetShowcase.IsBusy)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            Product bread = targetShowcase.GetProduct();
            if (bread == null)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            // 한 번만 애니메이션
            if (!customer.isPickingAnimationPlayed)
            {
                customer.animator.SetTrigger(CustomerAnimationController.StackIdle);
                customer.isPickingAnimationPlayed = true;
            }

            // 빵 가져오기
            data.pickedUpCount++;
            customerUI.SetBreadCount(data.quantity - data.pickedUpCount);

            bread.MoveTo(customer, Product.GoalType.Customer);

            yield return new WaitForSeconds(0.1f);
        }

        // 픽업 완료
        customer.isPickingAnimationPlayed = false;
        customer.animator.SetTrigger(CustomerAnimationController.StackMove);
        isPicking = false;
        stateMachine.ChangeState(stateMachine.OrderWaitingState);
    }

    public override void Exit()
    {
        // 자리 반환
        if (targetPoint != null)
            targetPoint.IsOccupied = false;

        targetShowcase.Customers.Dequeue();
    }

    public void SetTargetShowcase(Showcase showcase)
    {
        targetShowcase = showcase;

        if (targetPoint != null) stateMachine.Customer.MoveToNavAgentPoint(targetPoint.transform);
    }
}
