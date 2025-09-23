public class LeavingState:CustomerBaseState
{
    public LeavingState(CustomerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        base.Update();
        //GenericPoolManager.Instance.Release(stateMachine.Customer.gameObject, customer);
    }
}