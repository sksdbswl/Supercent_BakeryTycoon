using System.Collections.Generic;
using UnityEngine;

public class Showcase : ProductContainer
{
    public Stack<Product> Products { get; }

    public Product GetProduct()
    {
        if (Products.Count == 0) return null;
        return Products.Pop();
    } 
}