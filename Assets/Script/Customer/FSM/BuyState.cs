using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyState:CustomerBaseState
{
    public BuyState(CustomerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("내가 계산할 차례야");
    }

    private IEnumerator FinishAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        //TODO:: 포장 및 결제 애니메이션 처리
        FinishBuying();
    }

    public override void Update()
    {
        if (stateMachine.Customer.ArriveCheck())
        {
            stateMachine.Customer.StartCoroutine(FinishAfterDelay());
        }
    }
    
    public bool OnBuyPointAssigned(NavPoint point)
    {
        Debug.Log(QueueManager.Instance.CashierPoints[0]);
        Debug.Log(point);
        
        // 만약 내가 "맨 앞 계산대 자리"라면 
        if (!QueueManager.Instance.CashierPoints[0] == point) return false;
        return true;
    }

    public void FinishBuying()
    {
        // 1️⃣ 손님이 들고 있는 빵 풀로 반환
        while (stateMachine.Customer.PickedUpBreads.Count > 0)
        {
            Product bread = stateMachine.Customer.PickedUpBreads.Pop();
            GenericPoolManager.Instance.Release(bread.PooledObject.OriginPrefab, bread.gameObject);
        }
        
        // 계산 끝나면 포인트 해제
        QueueManager.Instance.ReleaseCashierPoint(stateMachine.Customer.currentPoint);
        stateMachine.Customer.currentPoint = null;
        
        // LeavingState 전환
        stateMachine.ChangeState(stateMachine.LeavingState);
    }

    // private void MoveTo(Vector3 target, System.Action onArrive)
    // {
    //     // NavMeshAgent 또는 DOTween으로 이동 처리
    //     // 도착하면 onArrive() 호출
    // }
}