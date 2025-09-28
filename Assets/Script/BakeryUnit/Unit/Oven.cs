using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : ProductContainer
{
    [SerializeField] private float bakingTime = 3f;
    public Queue<Product> breadQueue = new Queue<Product>();
    public GameObject TriggerCheck;
    
    public void Bake(Product product)
    {
        AddProduct(product);
    }
    
    public bool BakedCheck()
    {
        if (breadQueue.Count == 0) return false;

        return true;
    }
}