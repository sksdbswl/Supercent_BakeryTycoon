using System.Collections;
using UnityEngine;

public class CustomerSpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab; 
    [SerializeField] private Transform spawnParent;     
    [SerializeField] private Transform spawnPosition;

    // [SerializeField] private int maxActiveCustomers = 10; // 동시에 매장에 있을 최대 손님
    // [SerializeField] private int maxShowCaseActiveCustomers = 5; // 쇼케이스 기준 손님
    [SerializeField] private float checkInterval = 10f;  // 체크 간격

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // private IEnumerator SpawnRoutine()
    // {
    //     while (true)
    //     {
    //         // 현재 활성 손님 수 확인
    //         int activeCount = CountActiveCustomers();
    //
    //         Debug.Log(activeCount);
    //         
    //         if (activeCount < maxActiveCustomers)
    //         {
    //             // 랜덤으로 1~3명 스폰
    //             int spawnCount = Random.Range(1, 4);
    //             spawnCount = Mathf.Min(spawnCount, maxActiveCustomers - activeCount);
    //
    //             for (int i = 0; i < spawnCount; i++)
    //             {
    //                 SpawnCustomer();
    //             }
    //         }
    //
    //         yield return new WaitForSeconds(checkInterval);
    //     }
    // }
    
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            int activeInShowcase = AreaManager.Instance.CountActiveCustomers(AreaManager.Instance.BreadPoints);
            int activeInCashier  = AreaManager.Instance.CountActiveCustomers(AreaManager.Instance.CashierPoints);

            // 영역별 수용 한도
            int maxShowcase = 5;
            int maxCashier  = 9;

            // 총 스폰 가능 여부 체크
            if (activeInShowcase < maxShowcase && activeInCashier < maxCashier)
            {
                // 스폰 가능한 수 계산 (쇼케이스 공간 고려)
                int availableSlots = maxShowcase - activeInShowcase;
                int spawnCount = Random.Range(1, 4);
                spawnCount = Mathf.Min(spawnCount, availableSlots);

                for (int i = 0; i < spawnCount; i++)
                {
                    SpawnCustomer();
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void SpawnCustomer()
    {
        GameObject obj = GenericPoolManager.Instance.Get(customerPrefab, spawnPosition.position, Quaternion.identity, spawnParent);
        obj.SetActive(true);

        Customer customer = obj.GetComponent<Customer>();
        customer.navAgent.Warp(spawnPosition.position); 
        customer.CustomerStateMachine.ChangeState(customer.CustomerStateMachine.VisitState);
    }

    // private int CountActiveCustomers()
    // {
    //     int count = 0;
    //     foreach (Transform child in spawnParent)
    //     {
    //         if (child.gameObject.activeInHierarchy) count++;
    //     }
    //     return count;
    // }
}