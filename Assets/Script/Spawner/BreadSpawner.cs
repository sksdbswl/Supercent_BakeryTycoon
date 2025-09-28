using System.Collections;
using UnityEngine;

public class BreadSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private Oven oven; 
    [SerializeField] private GameObject breadPrefab;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private Transform spawnPoint; 
    [SerializeField] private Transform movePoint; 
    [SerializeField] private Showcase showcase;
    private float createInterval = 1f;
    private float forceDelay = 0.5f; 
    private float forcePower = 4.5f;

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
                GameObject bread = GenericPoolManager.Instance.Get(
                    breadPrefab,
                    spawnPoint.position,
                    Quaternion.identity,
                    spawnParent
                );

                if (bread != null)
                {
                    bread.transform.position = spawnPoint.position;
                    bread.transform.rotation = Quaternion.identity;
                    bread.SetActive(true);

                    Rigidbody rb = bread.GetComponent<Rigidbody>();
                    Product product = bread.GetComponent<Product>();

                    if (rb != null)
                    {
                        rb.velocity = Vector3.zero;
                        StartCoroutine(AddForceAfterDelay(rb, product));
                    }

                    // 빵 이동은 코루틴으로 병렬 처리
                    //StartCoroutine(MoveAndBakeBread(bread, product));
                    
                    // Bread 수 증가 및 Oven Bake는 이동과 동시에 처리
                    if (product != null)
                    {
                        product.Init(this, showcase);
                        oven.Bake(product);
                        Bread++;
                    }
                }
            }

            yield return new WaitForSeconds(createInterval);
        }
    }
    
    // Force 지연 적용
    private IEnumerator AddForceAfterDelay(Rigidbody rb, Product product)
    {
        yield return new WaitForSeconds(forceDelay);
        rb.isKinematic = false; 
        rb.AddForce(-Vector3.forward * forcePower, ForceMode.Impulse);
        oven.breadQueue.Enqueue(product);
    }

    public void PickupBread()
    {
        if (Bread > 0)
            Bread--;
    }
}
