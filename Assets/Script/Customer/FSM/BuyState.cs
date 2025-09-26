using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyState:CustomerBaseState
{
    public BuyState(CustomerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("계산해주세요~");
    }

    public IEnumerator FinishAfterDelay(Player player, GameObject paperBox)
    {
        // 1. 포장 애니메이션 실행
        yield return PackingBread(player, paperBox);

        // 2. 결제 처리 대기 (애니메이션 포함)
        yield return new WaitForSeconds(3f);

        // 3. 결제 완료 후 상태 전환
        FinishBuying(player);
    }
    

    public void FinishBuying(Player player)
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
        player.Customer = null;
    }
    
    
    private IEnumerator PackingBread(Player player, GameObject box)
    {
        Animator boxAnimator = box.GetComponent<Animator>();

        if (boxAnimator != null)
        {
            boxAnimator.SetTrigger("Open");
            yield return new WaitForSeconds(0.5f); // 박스 열리는 시간
        }

        // 손님의 빵을 상자로 이동 (DoTween 또는 Transform)
        foreach (var bread in stateMachine.Customer.PickedUpBreads)
        {
            bread.transform.SetParent(box.transform);
            bread.transform.localPosition = Vector3.zero;
            // 필요하면 DoTween Bezier 이동
            yield return new WaitForSeconds(0.2f); // 빵 하나 이동 대기
        }

        if (boxAnimator != null)
        {
            boxAnimator.SetTrigger("Close");
            yield return new WaitForSeconds(0.5f); // 박스 닫는 시간
        }

        // 상자를 손님 손 위치로 이동
        box.transform.SetParent(stateMachine.Customer.BreadTransform);
        box.transform.localPosition = Vector3.zero;
    }


}