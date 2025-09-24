using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CameraController mainCamera;
    
    [Header("Particle System Settings")]
    public ParticleSystem openParticlePrefab;
    
    /// <summary>
    /// 손님 종류 및 스폰
    /// </summary>
    [SerializeField] public CustomerTable CustomerTable;
    [SerializeField] public CustomerSpawnerManager CustomerSpawnerManager;
    
    protected override void Awake()
    {
        base.Awake();
        mainCamera.GetComponent<CameraController>();
        UnlockActionFactory.OpenParticle = openParticlePrefab;
    }
}
