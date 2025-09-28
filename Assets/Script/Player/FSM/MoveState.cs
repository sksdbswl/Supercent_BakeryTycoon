using UnityEngine;

public class MoveState : PlayerBaseState
{
    public MoveState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        if (stateMachine.Player.PickedUpBreads.Count > 0)
        {
            stateMachine.Player.animator.SetTrigger(PlayerAnimationController.StackMove);
        }
        else
        {
            stateMachine.Player.animator.SetTrigger(PlayerAnimationController.Move);
        }
    }
    
    public override void Update()
    {
        Vector3 move = stateMachine.Player.Mover.GetMoveDirection();

        if (move.sqrMagnitude < 0.01f)
        {
            stateMachine.ChangeState(new IdleState(stateMachine));
            return;
        }

        stateMachine.Player.Mover.Move(move);
    }
}