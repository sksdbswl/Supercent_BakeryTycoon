using System.Collections.Generic;
using UnityEngine;

public class GenericPoolManager : MonoBehaviour
{
    public static GenericPoolManager Instance { get; private set; }

    // 프리팹을 키로, 해당 풀을 값으로 저장
    private Dictionary<GameObject, GenericObjectPool<GameObject>> pools 
        = new Dictionary<GameObject, GenericObjectPool<GameObject>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 풀 생성
    public void CreatePool(GameObject prefab, int initialSize, Transform parent = null)
    {
        if (pools.ContainsKey(prefab)) return;

        var pool = new GenericObjectPool<GameObject>(
            createFunc: () => {
                GameObject obj = Instantiate(prefab, parent);
                obj.SetActive(false);
                return obj;
            },
            onGet: (obj) => obj.SetActive(true),
            onRelease: (obj) => {
                obj.SetActive(false);
                if(parent != null) obj.transform.SetParent(parent);
            },
            initialSize: initialSize
        );

        pools[prefab] = pool;
    }

    // 풀에서 가져오기
    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!pools.ContainsKey(prefab))
        {
            CreatePool(prefab, 1, parent);
        }

        GameObject obj = pools[prefab].Get();
        obj.transform.SetPositionAndRotation(position, rotation);
        if(parent != null) obj.transform.SetParent(parent);
        return obj;
    }

    // 풀로 반환
    public void Release(GameObject prefab, GameObject obj)
    {
        if (!pools.ContainsKey(prefab))
        {
            Debug.LogWarning("풀에 없는 오브젝트 반환: " + prefab.name);
            Destroy(obj);
            return;
        }

        pools[prefab].Release(obj);
    }
}