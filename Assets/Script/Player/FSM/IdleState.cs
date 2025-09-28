using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        if (stateMachine.Player.PickedUpBreads.Count > 0)
        {
            stateMachine.Player.animator.SetTrigger(PlayerAnimationController.StackIdle);
        }
        else
        {
            stateMachine.Player.animator.SetTrigger(PlayerAnimationController.Idle);
        }
    }
    
    public override void Update()
    {
        Vector3 move = stateMachine.Player.Mover.GetMoveDirection();

        if (move.sqrMagnitude > 0.01f)
        {
            stateMachine.ChangeState(new MoveState(stateMachine));
        }
    }
}