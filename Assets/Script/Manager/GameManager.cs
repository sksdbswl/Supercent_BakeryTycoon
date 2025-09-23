using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CameraController mainCamera;
    
    /// <summary>
    /// 손님 종류 및 빵 종류 셋팅
    /// </summary>
    [SerializeField] public CustomerTable CustomerTable;
    
    protected override void Awake()
    {
        base.Awake();
        mainCamera.GetComponent<CameraController>();
    }
    
    private void Start()
    {
        // customerDataLoader.ImportCustomerJson();
        //
        // Debug.Log($"Loaded {customers.Length} customers");
    }
}
