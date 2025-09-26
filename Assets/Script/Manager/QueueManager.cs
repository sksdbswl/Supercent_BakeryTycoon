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
        // 대기열 등록
        queue.Enqueue(customer);
        // 지점 점유
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
            
            customer.OnPointAssigned(points[index]);
            index++;
        }
    }


    public bool CheckMyTurn(Customer customer)
    {
        if (cashierQueue.Count == 0) return false;
        return cashierQueue.Peek() == customer;
    }
    
    
    // private NavPoint RequestPoint(List<NavPoint> points, Queue<Customer> queue, Customer customer, bool isCashier = false)
    // {
    //     if (!queue.Contains(customer))
    //         queue.Enqueue(customer);
    //
    //     for (int i = 0; i < points.Count; i++)
    //     {
    //         points[i].IsOccupied = true;
    //         customer.currentPoint = points[i];
    //         customer.OnPointAssigned(points[i]);
    //         
    //         return points[i];
    //     }
    //     
    //     return null;
    // }
    
    // private NavPoint RequestPoint(List<NavPoint> points, Queue<Customer> queue, Customer customer, bool isCashier = false)
    // {
    //     // 큐에 대기 손님이 있으면 새 손님은 바로 자리 배정하지 않고 큐에 넣기
    //     if (queue.Count > 0)
    //     {
    //         if (!queue.Contains(customer))
    //             queue.Enqueue(customer);
    //         return null;
    //     }
    //
    //     // 앞에서부터 비어 있는 포인트 할당
    //     for (int i = 0; i < points.Count; i++)
    //     {
    //         points[i].IsOccupied = true;
    //         customer.currentPoint = points[i];
    //         customer.OnPointAssigned(points[i]);
    //
    //         // 계산대라면 맨 앞자리 손님 즉시 계산 시작 / 맨앞 대기면 식사
    //         if (isCashier && i == 0)
    //             customer.Sequence();
    //
    //         return points[i];
    //     }
    //
    //     // 포인트 없으면 큐에 등록
    //     if (!queue.Contains(customer))
    //         queue.Enqueue(customer);
    //
    //     return null;
    // }

    
    // private void ReleaseQueuePoint(List<NavPoint> points, Queue<Customer> queue, bool isCashier, NavPoint releasedPoint)
    // {
    //     if (releasedPoint != null)
    //         releasedPoint.IsOccupied = false;
    //
    //     // 1. 전체 포인트 초기화
    //     foreach (var p in points)
    //         p.IsOccupied = false;
    //
    //     if (queue.Count == 0) return;
    //
    //     // 2. 맨 앞자리 손님 먼저 꺼내서 첫 포인트 할당
    //     Customer frontCustomer = queue.Dequeue();
    //     frontCustomer.currentPoint = points[0];
    //     points[0].IsOccupied = true;
    //     frontCustomer.OnPointAssigned(points[0]);
    //
    //     if (isCashier)
    //         frontCustomer.Sequence();
    //
    //     // 3. 나머지 손님들을 순서대로 포인트 배치
    //     int i = 1;
    //     int count = queue.Count;
    //     Queue<Customer> tempQueue = new Queue<Customer>();
    //
    //     while (queue.Count > 0)
    //     {
    //         Customer c = queue.Dequeue();
    //         if (i >= points.Count)
    //         {
    //             tempQueue.Enqueue(c); // 포인트 부족 시 다시 큐에
    //         }
    //         else
    //         {
    //             c.currentPoint = points[i];
    //             points[i].IsOccupied = true;
    //             c.OnPointAssigned(points[i]);
    //         }
    //         i++;
    //     }
    //
    //     // 남은 손님들을 다시 큐에 넣기
    //     while (tempQueue.Count > 0)
    //         queue.Enqueue(tempQueue.Dequeue());
    // }

    #endregion
}
