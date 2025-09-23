public abstract class CustomerBaseState : IState
{
    protected CustomerStateMachine stateMachine;

    public CustomerBaseState(CustomerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void HandleInput() { }
    public virtual void Update() { }
}