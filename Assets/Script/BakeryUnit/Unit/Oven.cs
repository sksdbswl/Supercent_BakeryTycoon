using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : ProductContainer
{
    [SerializeField] private float bakingTime = 3f;

    // public Product GetProduct()
    // {
    //     Debug.Log("빵 겟또");
    //     
    //     if (Products.Count == 0) return null;
    //     return Products.Pop();
    // } 
    
    public void Bake(Product product)
    {
        AddProduct(product);
        //Debug.Log("빵 구워짐!");
    }
}