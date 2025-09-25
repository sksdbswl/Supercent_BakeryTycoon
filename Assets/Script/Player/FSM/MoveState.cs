using UnityEngine;

public class MoveState : PlayerBaseState
{
    public MoveState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        //Debug.Log("Move Enter");   
        stateMachine.Player.animator.SetTrigger(PlayerAnimationController.Move);
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

    public override void Exit()
    {
        //Debug.Log("Move Exit");   
        stateMachine.Player.animator.ResetTrigger(PlayerAnimationController.Move);
    }
}