using System;
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
    public Transform SeatPosition;
    private bool isOccupied = false;

    [HideInInspector]
    public bool isUnlocked = false;

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
    }

    private void Start()
    {
        // UnlockType별 전략 생성
        unlockAction = UnlockActionFactory.Create(unlockType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isUnlocked) return;

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            TryUnlock(player);
        }
    }

    public void TryUnlock(Player player)
    {
        if (!CanUnlock(player))
        {
            Debug.Log("자산이 부족합니다!");
            return;
        }

        Unlock(player); // 현재 Unlock 처리

        if (Targets != null && Targets.Length > 0)
        {
            // UnlockType 전략 실행
            unlockAction.Execute(player, Targets);
        }
    }

    private bool CanUnlock(Player player) => player.Money >= cost;

    private void Unlock(Player player)
    {
        player.SpendMoney(cost);
        isUnlocked = true;
        if (LockIcon != null)
            LockIcon.SetActive(false);
    }

    public bool CanSit() => !isOccupied;

    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;
    }

    // 다음 해금 가능 상태 설정 (NextUnlock에서 호출 가능)
    public void SetUnlockedState(bool state)
    {
        isUnlocked = state;
        if (LockIcon != null)
            LockIcon.SetActive(!state);
    }
}
