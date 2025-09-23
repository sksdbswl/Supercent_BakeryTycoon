using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public GameObject OriginPrefab { get; private set; }

    // 풀에서 최초 생성될 때 호출
    public void Initialize(GameObject prefab)
    {
        OriginPrefab = prefab;
    }

    // 풀로 반환될 때 상태 초기화
    public virtual void ResetObject()
    {
        OriginPrefab = null;
    }
}