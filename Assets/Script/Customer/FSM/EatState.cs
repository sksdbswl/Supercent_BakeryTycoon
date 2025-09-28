using System.Collections;
using UnityEngine;

public class EatState : CustomerBaseState
{
    public EatState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        stateMachine.Customer.CustomerUI.OnSprite(stateMachine.Customer.CustomerUI.Eat);
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.StackMove);
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
                table = availableTables[Random.Range(0, availableTables.Count)];
                table.SetOccupied(true); 
                break;
            }
            yield return new WaitForSeconds(0.5f); 
        }

        // 자리로 이동
        stateMachine.Customer.MoveToEat(table.SeatPosition);

        // 이동 완료될 때까지 대기
        while (! stateMachine.Customer.ArriveCheck())
        {
            yield return null;
        }
        
        // 자리 도착까지 대기
        //yield return WaitUntilArrived(table.SeatPosition);

        // 도착 후 애니메이션 전환
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Seat);

        // 식사 시작
        yield return EattingWaiting(table);
    }

    private IEnumerator WaitUntilArrived(Transform target, float threshold = 0.1f)
    {
        while (Vector3.Distance(stateMachine.Customer.transform.position, target.position) > threshold)
        {
            yield return null;
        }
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
        //stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Idle);
    }
}