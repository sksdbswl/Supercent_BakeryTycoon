using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class VisitState : CustomerBaseState
{
    private Showcase targetShowcase;
    private NavPoint targetPoint;

    public VisitState(CustomerStateMachine stateMachine) : base(stateMachine) { }

    private bool IsArrived = false;
    private bool isPickUp = false;
    
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
        if (targetPoint == null || targetShowcase == null || isPickUp) return;

        if (!stateMachine.Customer.ArriveCheck()) return;
        stateMachine.Customer.CustomerUI.SetBreadCount(stateMachine.Customer.customerData.quantity);
        stateMachine.Customer.CustomerUI.OnSprite(stateMachine.Customer.CustomerUI.Want);
        
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
        isPickUp = true;
        stateMachine.Customer.StartCoroutine(PickUpBreadCoroutine());
    }

    private IEnumerator PickUpBreadCoroutine()
    {
        int need = stateMachine.Customer.customerData.quantity;
        int pickup = stateMachine.Customer.customerData.pickedUpCount;
        
        while (pickup < need)
        {
            if (targetShowcase.IsBusy)
            {
                Debug.Log("쇼케이스 사용 중 → 잠깐 대기");
                yield return new WaitForSeconds(0.5f);
                continue; 
            }

            Product bread = targetShowcase.GetProduct();
            if (bread == null)
            {
                Debug.Log("쇼케이스 빵 부족 → 기다리는 중");
                yield return new WaitForSeconds(0.5f);
                continue; 
            }

            // 빵 가져가기
            pickup++;
            stateMachine.Customer.CustomerUI.SetBreadCount(
                need - pickup
            );

            bread.MoveTo(stateMachine.Customer, Product.GoalType.Customer);

            // 하나 가져간 후 텀 두기
            yield return new WaitForSeconds(0.5f);
        }

        // 다 모았으면 다음 상태로
        stateMachine.ChangeState(stateMachine.OrderWaitingState);
        isPickUp = false;
    }
}