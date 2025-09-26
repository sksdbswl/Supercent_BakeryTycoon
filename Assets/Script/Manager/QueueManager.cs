using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueManager : Singleton<QueueManager>
{
    [Header("대기열 포인트")]
    public List<NavPoint> CashierPoints = new List<NavPoint>();
    public List<NavPoint> DiningPoints = new List<NavPoint>();
    public List<NavPoint> EntryPoints = new List<NavPoint>();

    private Queue<Customer> cashierQueue = new Queue<Customer>();
    private Queue<Customer> diningQueue = new Queue<Customer>();
    private Queue<Customer> entryQueue = new Queue<Customer>();

    #region Request Points

    public NavPoint RequestCashierPoint(Customer customer) => RequestPoint(CashierPoints, cashierQueue, customer, true);
    public NavPoint RequestDiningPoint(Customer customer) => RequestPoint(DiningPoints, diningQueue, customer);
    public NavPoint RequestEntryPoint(Customer customer) => RequestPoint(EntryPoints, entryQueue, customer);

    private NavPoint RequestPoint(List<NavPoint> points, Queue<Customer> queue, Customer customer,
        bool isCashier = false)
    {
        // 1.대기열 등록
        queue.Enqueue(customer);
        
        // 2.지점 점유
        for (int i = 0; i < points.Count; i++)
        {
            if (!points[i].IsOccupied)
            {
                points[i].IsOccupied = true;
                customer.currentPoint = points[i];
                
                return points[i];
            }
        }
        
        return null;
    }

    #endregion

    #region Release Points

    public void ReleaseCashierPoint(NavPoint point) => ReleaseQueuePoint(CashierPoints, cashierQueue, true, point);
    public void ReleaseDiningPoint(NavPoint point) => ReleaseQueuePoint(DiningPoints, diningQueue, false, point);
    public void ReleaseEntryPoint(NavPoint point) => ReleaseQueuePoint(EntryPoints, entryQueue, false, point);

    private void ReleaseQueuePoint(List<NavPoint> points, Queue<Customer> queue, bool isCashier,
        NavPoint releasedPoint)
    {
        queue.Dequeue();
        
        //1. 전체 리스트 포인트 초기화
        foreach (var p in points)
            p.IsOccupied = false;
        
        //2. 큐에 담긴 수량만큼 리스트 점유 재적용
        for (var i = 0; i < queue.Count; i++)
        {
            points[i].IsOccupied = true;
        }

        int index = 0;
        //3. 점유시 큐에 담긴 손님의 포인트를 리스트 순서대로 적용
        foreach (var customer in queue)
        {
            if(index >= points.Count -1) return;
            
            customer.currentPoint = points[index];
            customer.OnPointAssigned(points[index]);
            index++;
        }
    }
    
    public bool CheckMyTurn(Customer customer)
    {
        if (cashierQueue.Count == 0) return false;
        return cashierQueue.Peek() == customer;
    }

    #endregion
}
