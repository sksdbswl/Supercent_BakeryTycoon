using System.Collections;
using System.Collections.Generic;
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
}
