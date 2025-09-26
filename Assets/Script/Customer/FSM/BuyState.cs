using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyState:CustomerBaseState
{
    public BuyState(CustomerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("계산해주세요~");
        stateMachine.Customer.StartCoroutine(FinishAfterDelay());
    }

    public IEnumerator FinishAfterDelay()
    {
        //player.isWorking = true;
        
        PackingBread();
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
    
    
    private void PackingBread()
    {
        //TODO ::상자 애니메이션 추가
        Debug.Log("포장중입니다.");
        
        // // 1. 상자 오브젝트 생성
        // GameObject box = GenericPoolManager.Instance.Get("PaperBox", stateMachine.Customer.PaperBoxPosition.position, Quaternion.identity);
        //
        //
        // // 애니메이션 트리거
        // Animator boxAnimator = box.GetComponent<Animator>();
        // if (boxAnimator != null)
        // {
        //     boxAnimator.SetTrigger("Open"); // 박스 열리는 애니메이션
        // }
        //
        // // 2. 손님이 들고 있는 빵을 박스로 이동
        // foreach (var bread in stateMachine.Customer.PickedUpBreads)
        // {
        //     // 빵 위치를 상자로 이동 (애니메이션 or DoTween)
        //     bread.transform.SetParent(box.transform);
        //     bread.transform.localPosition = Vector3.zero; 
        //     // => 단순히 위치 맞추기, 나중에 DoTween Bezier 애니메이션 가능
        // }
        //
        // // 3. 박스 닫기 애니메이션 실행
        // if (boxAnimator != null)
        // {
        //     boxAnimator.SetTrigger("Close");
        // }
        //
        // // 4. 손님 위치로 전달 (들고 있게 하기)
        // box.transform.SetParent(stateMachine.Customer.BreadTransform);
        // box.transform.localPosition = stateMachine.Customer.BreadTransform.position;
    }

}