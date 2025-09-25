using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
      //Debug.Log("Idle Enter");   
      stateMachine.Player.animator.SetTrigger(PlayerAnimationController.Idle);
    }
    
    public override void Update()
    {
        Vector3 move = stateMachine.Player.Mover.GetMoveDirection();

        if (move.sqrMagnitude > 0.01f)
        {
            stateMachine.ChangeState(new MoveState(stateMachine));
        }
    }

    public override void Exit()
    {
        //Debug.Log("Idle Exit");   
    }
}