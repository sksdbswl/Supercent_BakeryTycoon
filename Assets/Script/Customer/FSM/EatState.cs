using System.Collections;
using UnityEngine;

public class EatState : CustomerBaseState
{
    public EatState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        stateMachine.Customer.StartCoroutine(TrySitCoroutine());
    }

    private IEnumerator TrySitCoroutine()
    {
        UnLock table = null;

        // 사용 가능한 자리가 생길 때까지 대기
        while (table == null)
        {
            var availableTables = GameManager.Instance.GetOpenTables();

            if (availableTables.Count > 0)
            {
                // 랜덤으로 자리 선택
                table = availableTables[UnityEngine.Random.Range(0, availableTables.Count)];
                table.SetOccupied(true); // 점유
                break;
            }

            Debug.Log("앉을 자리가 없음, 대기 중...");
            yield return new WaitForSeconds(0.5f); // 0.5초마다 확인
        }

        // 자리로 이동
        stateMachine.Customer.MoveToEat(table.SeatPosition);

        // 자리 도착 후 식사
        yield return stateMachine.Customer.StartCoroutine(EattingWaitting(table));
    }

    private IEnumerator EattingWaitting(UnLock table)
    {
        Debug.Log("밥먹으러 가는 중");

        while (stateMachine.Customer.transform.position == table.SeatPosition.position)
        {
            yield return null;
        }
        
        QueueManager.Instance.ReleaseDiningPoint(stateMachine.Customer.currentPoint);
        stateMachine.Customer.currentPoint = null;
        
        Debug.Log("밥먹으러 도착");
        
        //상태 전환
        yield return new WaitForSeconds(5f);
        
        
        table.SetOccupied(false);
        stateMachine.ChangeState(stateMachine.LeavingState);
    }

    public override void Exit()
    {
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);
    }
}