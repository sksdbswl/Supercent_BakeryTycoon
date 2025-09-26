using UnityEngine;

public class EatState : CustomerBaseState
{
    public EatState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        Debug.Log("먹고 갈 자리가 있는지 찾아줘");
    }
    
    public override void Exit()
    {
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);
    }
}