using UnityEngine;

[System.Serializable]
public class NavPoint
{
    public Transform Point;         // 손님이 설 위치
    public bool IsOccupied = false; // 점유 여부
}