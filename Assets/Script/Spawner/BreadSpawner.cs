using System.Collections;
using UnityEngine;

public class BreadSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private Oven oven; 
    [SerializeField] private GameObject breadPrefab;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private Transform spawnPoint;        
    [SerializeField] private float createInterval = 5f;
    
    //private BakeryUnit BakeryUnit;
    
    private int MaxBread = 10;
    public int Bread = 0;

    // private void Awake()
    // {
    //     BakeryUnit = GetComponent<BakeryUnit>();
    // }
    
    private void Start()
    {
        StartCoroutine(SpawnBread());
    }

    private IEnumerator SpawnBread()
    {
        while (true)
        {
            if (Bread < MaxBread)
            {
                // 풀에서 오브젝트 가져오기
                GameObject bread = GenericPoolManager.Instance.Get(breadPrefab, spawnPoint.position, Quaternion.identity, spawnParent);
                
                if (bread != null)
                {
                    bread.transform.position = spawnPoint.position;
                    bread.transform.rotation = Quaternion.identity;
                    bread.SetActive(true);

                    Rigidbody rb = bread.GetComponent<Rigidbody>();
                    var product = bread.GetComponent<Product>();
                    
                    if (rb != null)
                    {
                        rb.velocity = Vector3.zero;
                        rb.AddForce(-Vector3.forward * 6f, ForceMode.Impulse);
                    }
                    
                    if (product != null)
                    {
                        oven.Bake(product);
                        Bread++;
                    }
                }
            }

            yield return new WaitForSeconds(createInterval);
        }
    }
    
    public void PickupBread()
    {
        if (Bread > 0)
            Bread--;
    }
}