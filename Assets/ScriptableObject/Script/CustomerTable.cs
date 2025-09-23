using UnityEngine;

[CreateAssetMenu(fileName = "CustomerTable", menuName = "Bakery/Customer Table")]
public class CustomerTable : AssetTable<CustomerData>
{
    public CustomerData GetRandomCustomer()
    {
        if (entries.Length == 0) return null;
        int index = Random.Range(0, entries.Length);
        return entries[index].asset;
    }
}