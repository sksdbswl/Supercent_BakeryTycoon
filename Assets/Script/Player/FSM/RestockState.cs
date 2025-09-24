using UnityEngine;

public class RestockState: PlayerBaseState
{
    public RestockState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        Debug.Log("RestockState Enter");   
    }
}