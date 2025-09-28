using System.Collections.Generic;
using UnityEngine;

public class Showcase : ProductContainer
{
    public Transform BreadPos;
    [Header("손님 대기 포인트")]
    public List<NavPoint> CustomerPoints = new List<NavPoint>();
    public Queue<Customer> Customers = new Queue<Customer>();
    
    public bool IsBusy { get; private set; }
    
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
        SetBusy(true);
        AddProduct(product);
    }
    
    public void SetBusy(bool busy)
    {
        IsBusy = busy;
    }
}