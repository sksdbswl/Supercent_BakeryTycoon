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
                table = availableTables[Random.Range(0, availableTables.Count)];
                table.SetOccupied(true); 
                break;
            }
            yield return new WaitForSeconds(0.5f); 
        }

        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.StackMove);
        stateMachine.Customer.CustomerUI.Balloon.SetActive(false);
        
        stateMachine.Customer.MoveToNavAgentPoint(table.SeatPosition);

        while (!stateMachine.Customer.ArriveCheck())
        {
            yield return null;
        }
        
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Seat);
        stateMachine.Customer.navAgent.enabled = false; 
        stateMachine.Customer.transform.position = table.SeatPosition.position + Vector3.up * 0.4f;
        
        // 식사 시작
        yield return EattingWaiting(table);
    }

    private IEnumerator EattingWaiting(UnLock table)
    {
        while (stateMachine.Customer.transform.position == table.SeatPosition.position)
        {
            yield return null;
        }
        
        QueueManager.Instance.ReleaseDiningPoint(stateMachine.Customer.currentPoint);
        stateMachine.Customer.currentPoint = null;
        
        yield return new WaitForSeconds(10f);
        
        stateMachine.Customer.navAgent.enabled = true;
        stateMachine.Customer.Payment(GameManager.PaymentType.Dining);
        table.Seat.Dirty();

        stateMachine.ChangeState(stateMachine.LeavingState);
    }
}