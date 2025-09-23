using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Bakery/Customer Data")]
public class CustomerData : ScriptableObject
{
    public string customerName;
    public BreadData desiredBread;   // 미리 정해진 빵 SO
    public int desiredBreadId;   
    public int quantity;             // 수량
    public bool wantsToEatIn;        // 포장 여부
}
