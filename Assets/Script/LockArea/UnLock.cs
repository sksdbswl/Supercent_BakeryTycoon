using TMPro;
using UnityEngine;

public enum UnlockType
{
      Open, // 새로운 지역 오픈
      Upgrade, // 빵 기계 추가 (?)
      Employment // 직원 고용
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
    
    public GameObject OpneArea;
    public GameObject CloseArea;
    
    private bool isUnlocked = false;
    private IUnlockAction unlockAction;
    

    private void Awake()
    {
        if(!HasIcon) LockIcon.SetActive(false);
        
        // 트리거 전용
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
        
        // 비용 적용
        if (costText != null)
            costText.text = cost.ToString();

        // UnlockType에 맞는 전략을 선택
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
        
        // OpenArea 활성화 & 애니메이션
        // if(OpneArea != null)
        // {
        //     OpneArea.SetActive(true);
        //     Animator anim = OpneArea.GetComponent<Animator>();
        //     if(anim != null) anim.SetTrigger("Open");
        // }
        
        Unlock(player);
        
        UnlockContext context = new UnlockContext { Target = OpneArea, Cost = cost, NoneTarget = CloseArea };
        unlockAction.Execute(player, context);
    }

    private bool CanUnlock(Player player) => player.Money >= cost;

    private void Unlock(Player player)
    {
        player.Money -= cost;
        isUnlocked = true;
    }
}