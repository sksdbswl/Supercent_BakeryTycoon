using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BreadSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private Oven oven; 
    [SerializeField] private GameObject breadPrefab;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private Transform spawnPoint;  
    [SerializeField] private Transform movePoint;
    [SerializeField] private float createInterval = 5f; // 스폰 후 다음 스폰까지 간격
    [SerializeField] private float forceDelay = 1f;     // 튀어나오기까지 대기 시간
    [SerializeField] private Showcase showcase;
    [SerializeField] private float forcePower = 5.5f;

    private int MaxBread = 10;
    public int Bread = 0;
    
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
                GameObject bread = GenericPoolManager.Instance.Get(breadPrefab, spawnPoint.position, Quaternion.identity, spawnParent);
                
                if (bread != null)
                {
                    bread.transform.position = spawnPoint.position;
                    bread.transform.rotation = Quaternion.identity;
                    bread.SetActive(true);

                    Rigidbody rb = bread.GetComponent<Rigidbody>();
                    var product = bread.GetComponent<Product>();
                    
                    if (product != null)
                    {
                        product.Init(this, showcase);
                        oven.Bake(product);
                        Bread++;
                    }
                    
                    product.transform.position = Vector3.Lerp(
                        product.transform.position,
                        movePoint.position,
                        Time.deltaTime * 0.5f
                    );
                    
                    if (rb != null)
                    {
                        rb.velocity = Vector3.zero;
                        yield return new WaitForSeconds(forceDelay);
                        rb.AddForce(-Vector3.forward * forcePower, ForceMode.Impulse);
                    }
                }
            }

            // 스폰 간격
            yield return new WaitForSeconds(createInterval);
        }
    }
    
    public void PickupBread()
    {
        if (Bread > 0)
            Bread--;
    }
}
