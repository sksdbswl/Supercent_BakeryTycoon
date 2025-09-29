using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab; 
    [SerializeField] private Transform spawnParent;     
    [SerializeField] public Transform spawnPosition;
    [SerializeField] private float checkInterval = 10f;
    [SerializeField] private Transform startPos;
    
    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            foreach (var showcase in ShowCaseManager.Instance.Showcases)
            {
                int activeCount = CountActiveCustomers(showcase);
                int maxShowcaseCustomers = 5;

                if (activeCount < maxShowcaseCustomers)
                {
                    int availableSlots = maxShowcaseCustomers - activeCount;
                    int spawnCount = Random.Range(1, 4);
                    spawnCount = Mathf.Min(spawnCount, availableSlots);

                    for (int i = 0; i < spawnCount; i++)
                    {
                        yield return StartCoroutine(SpawnCustomer(showcase));
                    }
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private IEnumerator SpawnCustomer(Showcase targetShowcase)
    {
        yield return new WaitForSeconds(1f);

        GameObject obj = GenericPoolManager.Instance.Get(customerPrefab, spawnPosition.position, Quaternion.identity, spawnParent);
        obj.SetActive(true);

        Customer customer = obj.GetComponent<Customer>();
        customer.navAgent.Warp(spawnPosition.position);
        customer.startPos = startPos;

        var visitState = customer.CustomerStateMachine.VisitState;
        visitState.SetTargetShowcase(targetShowcase);
    }


    private int CountActiveCustomers(Showcase showcase)
    {
        int count = 0;
        foreach (var point in showcase.CustomerPoints)
        {
            if (point.IsOccupied) count++;
        }
        return count;
    }
}
