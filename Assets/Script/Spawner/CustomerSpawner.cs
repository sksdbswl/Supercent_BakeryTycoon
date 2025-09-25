using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab; 
    [SerializeField] private Transform spawnParent;     
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float checkInterval = 10f;  // 체크 간격

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
                // 쇼케이스별 현재 대기 인원
                int activeCount = CountActiveCustomers(showcase);
                int maxShowcaseCustomers = 5;

                if (activeCount < maxShowcaseCustomers)
                {
                    int availableSlots = maxShowcaseCustomers - activeCount;
                    int spawnCount = Random.Range(1, 4);
                    spawnCount = Mathf.Min(spawnCount, availableSlots);

                    for (int i = 0; i < spawnCount; i++)
                    {
                        SpawnCustomer(showcase);
                    }
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void SpawnCustomer(Showcase targetShowcase)
    {
        GameObject obj = GenericPoolManager.Instance.Get(customerPrefab, spawnPosition.position, Quaternion.identity, spawnParent);
        obj.SetActive(true);

        Customer customer = obj.GetComponent<Customer>();
        customer.navAgent.Warp(spawnPosition.position);

        // VisitState에서 이동할 쇼케이스 지정
        var visitState = customer.CustomerStateMachine.VisitState;
        visitState.SetTargetShowcase(targetShowcase);

        customer.CustomerStateMachine.ChangeState(visitState);
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
