using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField] GameObject croassantPrefab;
    [SerializeField] Transform croassantParent;
    
    [SerializeField] GameObject customerPrefab;
    [SerializeField] Transform customerParent;
    
    private void Start()
    {
        // 빵 풀 생성
        //GenericPoolManager.Instance.CreatePool(croassantPrefab, 20, croassantParent);
        // 손님 풀 생성
        //GenericPoolManager.Instance.CreatePool(customerPrefab, 20, customerParent);
    }
}