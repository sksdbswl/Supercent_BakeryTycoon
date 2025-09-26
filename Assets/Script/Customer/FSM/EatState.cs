using System.Collections;
using UnityEngine;

public class EatState : CustomerBaseState
{
    public EatState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        Debug.Log("먹고 갈 자리가 있는지 찾아줘");

        var availableTables = GameManager.Instance.GetOpenTables();

        if (availableTables.Count == 0)
        {
            Debug.Log("앉을 자리가 없음, 다른 행동");
            //stateMachine.ChangeState(new LeaveState(stateMachine));
            return;
        }

        // 랜덤으로 자리 선택
        UnLock table = availableTables[UnityEngine.Random.Range(0, availableTables.Count)];

        // 해당 자리 점유
        table.SetOccupied(true);

        // 손님 이동 (NavMesh 또는 Transform)
        stateMachine.Customer.MoveToEat(table.SeatPosition);

        // 자리 도착 후 Eat 상태 지속
        stateMachine.Customer.StartCoroutine(EattingWaitting());
    }

    private IEnumerator EattingWaitting()
    {
        Debug.Log("밥먹는 중");
        yield return new WaitForSeconds(5f);
        
        stateMachine.ChangeState(stateMachine.LeavingState);
    }

    public override void Exit()
    {
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);
    }
}