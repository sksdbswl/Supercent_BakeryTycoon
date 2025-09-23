public class VisitState:CustomerBaseState
{
    public VisitState(CustomerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);
    }
}