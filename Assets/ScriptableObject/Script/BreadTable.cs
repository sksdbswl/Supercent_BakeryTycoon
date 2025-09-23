using UnityEngine;

[CreateAssetMenu(fileName = "BreadTable", menuName = "Bakery/Bread Table")]
public class BreadTable : AssetTable<BreadData>
{
    public BreadData GetRandomBread()
    {
        if (entries.Length == 0) return null;
        int index = Random.Range(0, entries.Length);
        return entries[index].asset;
    }
}