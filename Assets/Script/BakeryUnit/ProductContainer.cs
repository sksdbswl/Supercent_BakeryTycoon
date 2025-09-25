using System.Collections.Generic;
using UnityEngine;

public abstract class ProductContainer : MonoBehaviour, IProductContainer
{
    public Stack<Product> Products { get; private set; } = new Stack<Product>();

    public virtual Product GetProduct()
    {
        return Products.Count > 0 ? Products.Pop() : null;
    }

    public virtual void AddProduct(Product product)
    {
        Products.Push(product);
    }
}