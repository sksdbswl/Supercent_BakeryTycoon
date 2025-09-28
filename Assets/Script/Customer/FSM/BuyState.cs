using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyState:CustomerBaseState
{
    public BuyState(CustomerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("계산해주세요~");
    }

    public IEnumerator FinishAfterDelay(Player player, GameObject paperBox)
    {
        yield return PackingBread(player, paperBox);

        stateMachine.Customer.Payment(GameManager.PaymentType.Cashier);
        
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

        foreach (var bread in stateMachine.Customer.PickedUpBreads)
        {
            yield return GameManager.Instance.MoveTo(bread.gameObject, box.transform, 0.8f,true);

            bread.transform.SetParent(box.transform);
            bread.transform.localPosition = Vector3.zero;

            yield return new WaitForSeconds(0.2f); 
        }

        boxAnimator.SetTrigger("Close");

        yield return GameManager.Instance.MoveTo(box.gameObject, stateMachine.Customer.BreadTransform, 0.8f, false);
        
        box.transform.SetParent(stateMachine.Customer.BreadTransform);
        box.transform.localPosition = Vector3.zero;
    }
}