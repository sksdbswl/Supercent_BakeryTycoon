using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BreadSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject breadPrefab;
    [SerializeField] private Transform spawnParent;  
    
    public Transform spawnPoint;         // 빵이 나올 위치
    public float spawnInterval = 1f;     // 스폰 간격
    public float forceMin = 2f;          // 최소 힘
    public float forceMax = 5f;          // 최대 힘

    [SerializeField] private float createInterval = 5f;  // 체크 간격
    
    private void Start()
    {
        StartCoroutine(SpawnBread());
    }

    private IEnumerator SpawnBread()
    {
        while (true)
        {
            // 풀에서 오브젝트 가져오기
            GameObject bread = GenericPoolManager.Instance.Get(breadPrefab, spawnPoint.position, Quaternion.identity, spawnParent);
            if (bread != null)
            {
                bread.transform.position = spawnPoint.position;
                bread.transform.rotation = Quaternion.identity;
                bread.SetActive(true);

                Rigidbody rb = bread.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.AddForce(-Vector3.forward * 6f, ForceMode.Impulse);
                }
            }
            
            yield return new WaitForSeconds(createInterval);
        }
    }
}