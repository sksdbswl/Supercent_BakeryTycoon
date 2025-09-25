using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : ProductContainer
{
    [SerializeField] private float bakingTime = 3f;
    
    public void Bake(Product product)
    {
        AddProduct(product);
    }
}