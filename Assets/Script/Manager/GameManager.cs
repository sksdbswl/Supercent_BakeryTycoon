using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    public CameraController mainCamera;
    
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

    public bool IsUnlocked(GameObject targetArea)
    {
        var unlock = unlocks.Find(u => u.OpneArea == targetArea);
        return unlock != null && unlock.isUnlocked;
    }

    public void UnlockArea(Player player, GameObject targetArea)
    {
        var unlock = unlocks.Find(u => u.OpneArea == targetArea);
        if (unlock != null)
            unlock.TryUnlock(player);
    }
    
    /// <summary>
    /// 사용할 수 있는 의자 확인
    /// </summary>
    public List<UnLock> GetOpenTables()
    {
        return unlocks.FindAll(u => u.unlockType == UnlockType.Open && u.isUnlocked && u.CanSit());
    }
}
