using UnityEngine;

public class BakeWaitingState:CustomerBaseState
{
    public BakeWaitingState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        //Debug.Log("Bake waiting state");
    }
    
    public override void Update()
    {
        //Debug.Log("빵이 있는지 체크해");
        if (true)
        {
            //Debug.Log("빵이 있는지 체크가 완료되면");
            if (stateMachine.Customer.customerData.wantsToEatIn)
            {
                // 먹고갈거면 
                stateMachine.ChangeState(stateMachine.CheckOutState);
            }
            else
            {
                // 안 먹고갈거면
                stateMachine.ChangeState(stateMachine.OrderWaitingState);
            }
        }
    }
    
    public override void Exit()
    {
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);
    }
}