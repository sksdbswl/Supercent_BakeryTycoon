using System.Collections;
using UnityEngine;

public class EatState : CustomerBaseState
{
    public EatState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        stateMachine.Customer.CustomerUI.OnSprite(stateMachine.Customer.CustomerUI.Eat);
        stateMachine.Customer.StartCoroutine(TrySitCoroutine());
    }

    private IEnumerator TrySitCoroutine()
    {
        UnLock table = null;
        
        // 사용 가능한 자리가 생길 때까지 대기
        while (true)
        {
            var availableTables = GameManager.Instance.GetOpenTables();
           
            if (availableTables.Count > 0)
            {
                // 랜덤으로 자리 선택
                table = availableTables[Random.Range(0, availableTables.Count)];
                table.SetOccupied(true); 
                break;
            }

            yield return new WaitForSeconds(0.5f); 
        }

        // 자리로 이동
        stateMachine.Customer.MoveToEat(table.SeatPosition);

        // 자리 도착 후 식사
        yield return stateMachine.Customer.StartCoroutine(EattingWaiting(table));
    }

    private IEnumerator EattingWaiting(UnLock table)
    {
        while (stateMachine.Customer.transform.position == table.SeatPosition.position)
        {
            yield return null;
        }
        
        stateMachine.Customer.CustomerUI.Balloon.SetActive(false);
        QueueManager.Instance.ReleaseDiningPoint(stateMachine.Customer.currentPoint);
        stateMachine.Customer.currentPoint = null;
        
        //상태 전환
        yield return new WaitForSeconds(12f);
        
        stateMachine.Customer.Payment(GameManager.PaymentType.Dining);
        table.Seat.Dirty();

        stateMachine.ChangeState(stateMachine.LeavingState);
    }

    public override void Exit()
    {
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);
    }
}