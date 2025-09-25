using System.Collections.Generic;
using UnityEngine;

public interface IProductTarget
{
    Transform BreadTransform { get; }       // 빵을 놓을 Transform
    Stack<Product> PickedUpBreads { get; }  // 보유 빵 리스트
}