using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NavPoint
{
    public Transform Point;
    public bool IsOccupied = false;
}

public class AreaManager :  Singleton<AreaManager>
{
    public List<NavPoint> BreadPoints = new List<NavPoint>();
    public List<NavPoint> CashierPoints = new List<NavPoint>();
    public List<NavPoint> CheckoutPoints = new List<NavPoint>();

    public NavPoint GetFreePoint(List<NavPoint> points)
    {
        foreach (var p in points)
        {
            if (!p.IsOccupied)
            {
                p.IsOccupied = true;
                return p;
            }
        }
        return null; // 없으면 대기
    }
}