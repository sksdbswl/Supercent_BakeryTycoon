using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cashier : ProductContainer
{
    [Header("Paper Box Position")]
    public Transform PaperBoxPosition;
    [SerializeField] private GameObject paperBoxPrefab;
    [SerializeField] private Transform spawnParent; 
    public GameObject TriggerCheck;
    
    // 손님 대기 관리용
    public Queue<Customer> cashierQueue => QueueManager.Instance.cashierQueue;

    // 오브젝트 풀링된 PaperBox 생성
    public GameObject SpawnPaperBox()
    {
        GameObject box = GenericPoolManager.Instance.Get(paperBoxPrefab, spawnParent.position,Quaternion.identity,spawnParent);
        box.transform.position = PaperBoxPosition.position;
        box.transform.rotation = PaperBoxPosition.rotation;
    
        box.transform.DOLocalMoveY(0.2f, 0.3f).SetLoops(2, LoopType.Yoyo);

        return box;
    }

    public Customer GetCurrentCustomer()
    {
        if (cashierQueue.Count == 0) return null;

        var customer = cashierQueue.Peek();
        return customer.IsReadyToPay ? customer : null;
    }
}