using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cashier : ProductContainer
{
    [Header("Paper Box Position")]
    public Transform PaperBoxPosition;
    [SerializeField] private GameObject paperBoxPrefab;
    [SerializeField] private Transform spawnParent; 
    
    // 손님 대기 관리용
    public Queue<Customer> Queue => QueueManager.Instance.cashierQueue;

    // 오브젝트 풀링된 PaperBox 생성
    public void SpawnPaperBox()
    {
        GameObject box = GenericPoolManager.Instance.Get(paperBoxPrefab, spawnParent.position,Quaternion.identity,spawnParent);
        box.transform.position = PaperBoxPosition.position;
        box.transform.rotation = PaperBoxPosition.rotation;

        // Tween으로 살짝 점프 후 자리 고정
        box.transform.DOLocalMoveY(0.2f, 0.3f).SetLoops(2, LoopType.Yoyo);
    }
}