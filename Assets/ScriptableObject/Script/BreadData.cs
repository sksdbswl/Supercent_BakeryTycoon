using UnityEngine;

[CreateAssetMenu(fileName = "BreadData", menuName = "Bakery/Bread Data")]
public class BreadData : ScriptableObject
{
    public string breadName;      // 빵 이름
    public Sprite icon;           // UI 아이콘
    public int price;             // 가격
    public string description;    // 설명
}