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
        
        stateMachine.Customer.navAgent.SetDestination(targetPoint.transform.position);
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Move);
    }

    public override void Update()
    {
        if (!stateMachine.Customer.ArriveCheck()) return;
        
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);
    }
}