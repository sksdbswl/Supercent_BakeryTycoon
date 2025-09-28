using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    public CameraController mainCamera;
    public MoneyZone MoneyZone;
    
    [Header("Particle System Settings")]
    public ParticleSystem openParticlePrefab;
    
    /// <summary>
    /// 손님 종류 및 스폰
    /// </summary>
    [SerializeField] public CustomerTable CustomerTable;
    [FormerlySerializedAs("CustomerSpawnerManager")] [SerializeField] public CustomerSpawner customerSpawner;
    
    protected override void Awake()
    {
        base.Awake();
        mainCamera.GetComponent<CameraController>();
        UnlockActionFactory.OpenParticle = openParticlePrefab;
    }
    
    
    /// <summary>
    /// 오픈 지역 확인
    /// </summary>
    private List<UnLock> unlocks = new List<UnLock>();

    public void Register(UnLock unlock)
    {
        if (!unlocks.Contains(unlock))
            unlocks.Add(unlock);
    }
    
    /// <summary>
    /// 사용할 수 있는 의자 확인
    /// </summary>
    public List<UnLock> GetOpenTables()
    {
        return unlocks.FindAll(u => u.unlockType == UnlockType.Open && u.isUnlocked && u.CanSit());
    }
    
    /// <summary>
    /// 돈 생성 지역 관리
    /// </summary>
    public enum PaymentType { Cashier, Dining }
    public MoneyZone cashierZone;
    public MoneyZone diningZone;

    public void SpawnMoney(PaymentType type, int amount)
    {
        MoneyZone zone = type == PaymentType.Cashier ? cashierZone : diningZone;
        zone.SpawnMoney(amount);
    }
    
    /// <summary>
    /// 기본 배지어 무브 처리
    /// </summary>
    public IEnumerator MoveTo(GameObject obj, Transform endObj, float bezierDuration, bool returnPool)
    {
        Vector3 start = obj.transform.position;

        Vector3 control1 = start + Vector3.up * Random.Range(0.5f, 1.0f);
        Vector3 control2 = (start + endObj.position) * 0.5f + Vector3.up * Random.Range(0.3f, 0.7f);
        Vector3 end = endObj.position;

        float elapsed = 0f;

        while (elapsed < bezierDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / bezierDuration);

            Vector3 pos = Bezier.Cubic(start, control1, control2, end, t);
            obj.transform.position = pos;

            yield return null;
        }

        var origin = obj.GetComponent<PooledObject>();

        if (returnPool)
        {
            GenericPoolManager.Instance.Release(origin.OriginPrefab, origin.gameObject);
        }
    }
    
    /// <summary>
    /// 튜토리얼 적용
    /// </summary>
    [System.Serializable]
    public class TutorialData
    {
        public TutorialStep step;
        public Transform targetPos;
    }
    
     public enum TutorialStep
    {
        PickupBread,       // 오븐에서 빵 픽업
        ShowcaseArrow,     // 쇼케이스에 놓기
        CashierArrow,      // 계산대 이동
        MoneyPickupArrow,  // 돈 픽업
        UnlockZoneArrow,   // 해금 지역
        CleanZoneArrow     // 청소 지역
    }

    [Header("Tutorial Settings")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private List<TutorialData> tutorialOrder;

    private Queue<TutorialData> tutorialQueue = new Queue<TutorialData>();
    private TutorialData currentData;
    
    private void Start()
    {
        foreach (var step in tutorialOrder)
            tutorialQueue.Enqueue(step);
        
        NextStep();
    }
    
    public void OnStepComplete(TutorialStep step)
    {
        if (currentData != null && currentData.step == step)
        {
            HideArrow();

            if (step == TutorialStep.UnlockZoneArrow)
            {
                Debug.Log("해금 완료. 다음 단계(청소)는 손님 이용 후 진행됩니다.");
                return;
            }

            NextStep();
        }
    }
    
    public void NextStep()
    {
        if (tutorialQueue.Count > 0)
        {
            currentData = tutorialQueue.Dequeue();
            ShowArrow(currentData.targetPos);
        }
        else
        {
            currentData = null; 
        }
    }

    private Tween arrowTween;

    private void ShowArrow(Transform target)
    {
        arrowPrefab.SetActive(true);
        //arrowPrefab.transform.forward = mainCamera.transform.forward;
        arrowPrefab.transform.position = target.position;
        
        // 화살표 시작 위치
        Vector3 startPos = arrowPrefab.transform.position;
        
        arrowTween = arrowPrefab.transform.DOMoveY(startPos.y + 0.2f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine); 
    }
    
    private void HideArrow()
    {
        arrowPrefab.SetActive(false);
    }
}
