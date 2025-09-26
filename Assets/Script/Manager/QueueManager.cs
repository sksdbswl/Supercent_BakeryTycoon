using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueManager : Singleton<QueueManager>
{
    [Header("대기열 포인트")]
    public List<NavPoint> CashierPoints = new List<NavPoint>();
    public List<NavPoint> DiningPoints = new List<NavPoint>();
    public List<NavPoint> EntryPoints = new List<NavPoint>();

    public Queue<Customer> cashierQueue = new Queue<Customer>();
    public Queue<Customer> diningQueue = new Queue<Customer>();
    public Queue<Customer> entryQueue = new Queue<Customer>();

    #region 요청

    //public NavPoint RequestCashierPoint(Customer customer) => RequestPoint(CashierPoints, cashierQueue, customer);
    public NavPoint RequestDiningPoint(Customer customer) => RequestPoint(DiningPoints, diningQueue, customer);
    public NavPoint RequestEntryPoint(Customer customer) => RequestPoint(EntryPoints, entryQueue, customer);

    private NavPoint RequestPoint(List<NavPoint> points, Queue<Customer> queue, Customer customer)
    {
        foreach (var p in points)
        {
            if (!p.IsOccupied)
            {
                p.IsOccupied = true;
                return p;
            }
        }

        // 자리 없으면 대기열에 추가
        if (!queue.Contains(customer))
            queue.Enqueue(customer);

        return null;
    }
    
    

    #endregion

    #region 해제

    //public void ReleaseCashierPoint(NavPoint point) => ReleasePoint(point, CashierPoints, cashierQueue);
    public void ReleaseDiningPoint(NavPoint point) => ReleasePoint(point, DiningPoints, diningQueue);
    public void ReleaseEntryPoint(NavPoint point) => ReleasePoint(point, EntryPoints, entryQueue);

    private void ReleasePoint(NavPoint point, List<NavPoint> points, Queue<Customer> queue)
    {
        if (point == null) return;

        point.IsOccupied = false;

        if (queue.Count > 0)
        {
            Customer nextCustomer = queue.Dequeue();
            NavPoint freePoint = RequestPoint(points, queue, nextCustomer);
            if (freePoint != null)
            {
                nextCustomer.OnPointAssigned(freePoint);
            }
        }
    }
    private NavPoint RequestPoint(List<NavPoint> points)
    {
        // 항상 앞쪽부터 순회해서 비어 있는 포인트 반환
        return points.FirstOrDefault(p => !p.IsOccupied);
    }

    #endregion

    
    public NavPoint RequestCashierPoint(Customer customer)
    {
        foreach (var p in CashierPoints)
        {
            if (!p.IsOccupied)
            {
                p.IsOccupied = true;
                // 맨 앞 포인트면 바로 계산 시작
                if (p == CashierPoints[0])
                    customer.StartBuy();
                
                return p;
            }
        }

        // 비어있는 포인트 없으면 큐에 등록
        if (!cashierQueue.Contains(customer))
            cashierQueue.Enqueue(customer);

        return null;
    }

    public void ReleaseCashierPoint(NavPoint point)
    {
        if (point == null) return;

        point.IsOccupied = false;

        // 대기 중인 손님이 있으면 빈 포인트부터 채움
        Queue<Customer> tempQueue = new Queue<Customer>();

        while (cashierQueue.Count > 0)
        {
            Customer nextCustomer = cashierQueue.Dequeue();
            NavPoint freePoint = QueueManager.Instance.RequestPoint(CashierPoints); // 빈 포인트 확인

            if (freePoint != null)
            {
                freePoint.IsOccupied = true;
                nextCustomer.OnPointAssigned(freePoint); // 이동 시작
            }
            else
            {
                // 빈 포인트 없으면 임시 큐에 넣어 다시 대기
                tempQueue.Enqueue(nextCustomer);
            }
        }

        // 다시 대기열 복원
        while (tempQueue.Count > 0)
            cashierQueue.Enqueue(tempQueue.Dequeue());
    }
    
    // public void ReleaseCashierPoint(NavPoint point)
    // {
    //     if (point == null) return;
    //
    //     point.IsOccupied = false;
    //
    //     if (cashierQueue.Count > 0)
    //     {
    //         Customer nextCustomer = cashierQueue.Dequeue();
    //         nextCustomer.OnPointAssigned(point); // 이동 후 도착 시 StartBuy 호출
    //     }
    // }
    
    public bool IsFrontOfCashierQueue(Customer customer)
    {
        return cashierQueue.Count > 0 && cashierQueue.Peek() == customer;
    }
    
    // 차례가 된 손님 상태 변경
    // public void StartBuy(Customer customer)
    // {
    //     customer.CustomerStateMachine.ChangeState(customer.CustomerStateMachine.BuyState);
    // }
}
