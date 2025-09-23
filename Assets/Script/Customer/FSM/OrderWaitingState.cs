public class OrderWaitingState:CustomerBaseState
{
    private NavPoint targetPoint;
    
    public OrderWaitingState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        targetPoint = AreaManager.Instance.GetFreePoint(AreaManager.Instance.CashierPoints);
        
        if (targetPoint != null)
        {
            stateMachine.Customer.navAgent.SetDestination(targetPoint.Point.position);
            stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Move);
        }
    }
}