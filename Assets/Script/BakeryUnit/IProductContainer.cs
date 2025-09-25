using System.Collections.Generic;
using UnityEngine;

public interface IProductContainer
{
    public Stack<Product> Products { get; }
    public Transform transform { get; }
    Product GetProduct();
}