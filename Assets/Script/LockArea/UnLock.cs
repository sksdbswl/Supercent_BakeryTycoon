using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum UnlockType
{
    Open,       // 새로운 지역 오픈
    Upgrade,    // 빵 기계 추가
    Employment  // 직원 고용
}

[System.Serializable]
public class UnlockTarget
{
    public GameObject Activate;   // 해금 시 활성화할 오브젝트
    public GameObject Deactivate; // 해금 시 비활성화할 오브젝트
    public UnLock CurrentUnlock;  // 현재 해금 지역 비활
    public UnLock NextUnlock;     // 다음 해금 가능 지역
}

[RequireComponent(typeof(Collider))]
public class UnLock : MonoBehaviour
{
    [Header("Unlock Settings")]
    public UnlockType unlockType;
    public int cost;
    public TMP_Text costText;
    public GameObject LockIcon;
    public bool HasIcon;

    [Header("Unlock Targets")]
    public UnlockTarget[] Targets;

    [Header("Seat Settings")] 
    public Seat Seat;
    public Transform SeatPosition;
    private bool isOccupied = false;

    [HideInInspector]
    public bool isUnlocked = false;
    private bool isPlayerInside = false;
    
    private IUnlockAction unlockAction;

    private void Awake()
    {
        if (!HasIcon && LockIcon != null)
            LockIcon.SetActive(false);

        Collider col = GetComponent<Collider>();
        col.isTrigger = true;

        if (costText != null)
            costText.text = cost.ToString();

        GameManager.Instance.Register(this);
        isOccupied = false;
        remainCost = cost;
    }

    private void Start()
    {
        // UnlockType별 전략 생성
        unlockAction = UnlockActionFactory.Create(unlockType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isUnlocked) return;
        GameManager.Instance.OnStepComplete(GameManager.TutorialStep.UnlockZoneArrow);

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            isPlayerInside = true;
            TryUnlock(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            isPlayerInside = false;
        }
    }

    public void TryUnlock(Player player)
    {
        StartCoroutine(PlayPayEffect(player)); 
    }
    
    private void Unlock()
    {
        isUnlocked = true;
        if (LockIcon != null)
            LockIcon.SetActive(false);
        
        SoundManager.Instance.PlaySound(SoundType.Open);
    }

    public bool CanSit() => !isOccupied;

    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;
    }

    public void SetUnlockedState(bool state)
    {
        isUnlocked = state;
        if (LockIcon != null)
            LockIcon.SetActive(!state);
    }

    private int remainCost = 0;
    
    private IEnumerator PlayPayEffect(Player player)
    {
        int payAmount = Mathf.Min(remainCost, player.Money);
        SoundManager.Instance.PlaySound(SoundType.Money);
        
        for (int i = 0; i < payAmount; i++)
        {
            if (!isPlayerInside)
            {
                yield break;
            }

            GameObject money = GenericPoolManager.Instance.Get(
                GameManager.Instance.MoneyZone.moneyPrefab,
                player.BreadTransform.position,
                Quaternion.identity,
                GameManager.Instance.MoneyZone.zonePoint
            );

            yield return StartCoroutine(GameManager.Instance.MoveTo(money, gameObject.transform, 0.05f, true));

            remainCost--;
            if (costText != null)
                costText.text = remainCost.ToString();

            player.SpendMoney(1);
        }

        if (remainCost <= 0)
        {
            Unlock();

            if (Targets != null && Targets.Length > 0)
            {
                unlockAction.Execute(player, Targets);
            }
        }
    }
}
