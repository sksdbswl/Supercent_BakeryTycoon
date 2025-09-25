using System.Collections.Generic;
using UnityEngine;

public class Showcase : ProductContainer
{
    public Transform BreadPos;
    [Header("손님 대기 포인트들")]
    public List<Transform> CustomerPositions;
    public List<NavPoint> CustomerPoints = new List<NavPoint>();
    
    private void Awake()
    {
        CustomerPoints.Clear();
        foreach (var pos in CustomerPositions)
        {
            CustomerPoints.Add(new NavPoint { Point = pos, IsOccupied = false });
        }
    }
    
    public NavPoint GetFreePoint()
    {
        foreach (var p in CustomerPoints)
        {
            if (!p.IsOccupied)
            {
                p.IsOccupied = true;
                return p;
            }
        }
        return null;
    }
    
    public void Exhibition(Product product)
    {
        Debug.Log("쇼케이스에 빵 넣어 :: Exhibition");
        AddProduct(product);
    }
}