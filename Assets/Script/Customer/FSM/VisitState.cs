using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class VisitState:CustomerBaseState
{
    private NavPoint targetPoint;

    public VisitState(CustomerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        targetPoint = AreaManager.Instance.GetFreePoint(AreaManager.Instance.BreadPoints);
        
        if (targetPoint != null)
        {
            stateMachine.Customer.navAgent.SetDestination(targetPoint.Point.position);
            stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Move);
        }
    }

    public override void Update()
    {
        if (!stateMachine.Customer.navAgent.pathPending && 
            stateMachine.Customer.navAgent.remainingDistance <= 0.1f)
        {
            Debug.Log("쇼케이스앞에 도착함");
            
            //TODO:: 빵픽업
            // if (showcase.HasBread)
            // {
            //     Product bread = showcase.GetProduct();
            //     // 먹기 or 애니메이션 실행
            // }
            
            // stateMachine.ChangeState(stateMachine.BakeWaitingState);
            // stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);
        }
    }

    public override void Exit()
    {
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);
        targetPoint.IsOccupied = false;
    }
}