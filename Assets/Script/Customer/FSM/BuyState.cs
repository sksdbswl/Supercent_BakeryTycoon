using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyState:CustomerBaseState
{
    private static readonly int Open = Animator.StringToHash("Open");
    private static readonly int Close = Animator.StringToHash("Close");
    
    public BuyState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
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
        
        stateMachine.ChangeState(stateMachine.LeavingState);
        player.Customer = null;
    }
    
    
    private IEnumerator PackingBread(Player player, GameObject box)
    {
        Animator boxAnimator = box.GetComponent<Animator>();

        if (boxAnimator != null)
        {
            boxAnimator.SetTrigger(Open);
            yield return new WaitForSeconds(0.2f);
        }

        foreach (var bread in stateMachine.Customer.PickedUpBreads)
        {
            bread.SetTrigger();
            yield return GameManager.Instance.MoveTo(bread.gameObject, box.transform, 0.8f,true);
        }

        boxAnimator.SetTrigger(Close);

        yield return GameManager.Instance.MoveTo(box.gameObject, stateMachine.Customer.BreadTransform, 0.3f, false);
        
        box.transform.SetParent(stateMachine.Customer.BreadTransform);
        box.transform.localPosition = Vector3.zero;
    }
}