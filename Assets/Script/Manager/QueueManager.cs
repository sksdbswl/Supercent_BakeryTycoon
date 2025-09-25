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

    public NavPoint RequestCashierPoint(Customer customer) => RequestPoint(CashierPoints, cashierQueue, customer);
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

    public void ReleaseCashierPoint(NavPoint point) => ReleasePoint(point, CashierPoints, cashierQueue);
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
}
