using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField] GameObject croassantPrefab;
    [SerializeField] Transform croassantParent;

    private void Start()
    {
        // 빵 풀 생성
        GenericPoolManager.Instance.CreatePool(croassantPrefab, 20, croassantParent);
    }
}