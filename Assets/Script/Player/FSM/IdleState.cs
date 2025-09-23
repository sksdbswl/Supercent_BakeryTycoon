using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
      Debug.Log("Idle Enter");   
      stateMachine.Player.animator.SetTrigger(PlayerAnimationController.Idle);
    }
    
    public override void Update()
    {
        //Debug.Log("Idle Update");   
    }

    public override void Exit()
    {
        Debug.Log("Idle Exit");   
    }
}