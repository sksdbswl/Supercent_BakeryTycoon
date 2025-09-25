public class OrderWaitingState:CustomerBaseState
{
    private NavPoint targetPoint;
    
    public OrderWaitingState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        if (stateMachine.Customer.customerData.wantsToEatIn)
        {
            //나 밥먹고 갈테야
            targetPoint = QueueManager.Instance.RequestDiningPoint(stateMachine.Customer);
        
            if (targetPoint != null)
            {
                stateMachine.Customer.navAgent.SetDestination(targetPoint.transform.position);
                stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Move);
            }
        }
        else
        {
            //나 포장하고 계산해줘
            targetPoint = QueueManager.Instance.RequestCashierPoint(stateMachine.Customer);
        
            if (targetPoint != null)
            {
                stateMachine.Customer.navAgent.SetDestination(targetPoint.transform.position);
                stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Move);
            }
        }
        
    }
}