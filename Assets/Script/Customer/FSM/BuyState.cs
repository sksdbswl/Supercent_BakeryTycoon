using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyState:CustomerBaseState
{
    public BuyState(CustomerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("내가 계산할 차례야");
        stateMachine.Customer.StartCoroutine(FinishAfterDelay());
    }

    private IEnumerator FinishAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        //TODO:: 포장 및 결제 애니메이션 처리
        FinishBuying();
    }

    public void FinishBuying()
    {
        // 손님이 들고 있는 빵 풀로 반환
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