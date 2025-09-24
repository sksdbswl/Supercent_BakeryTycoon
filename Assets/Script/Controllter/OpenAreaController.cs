using System.Collections.Generic;
using UnityEngine;

public class OpenAreaController : MonoBehaviour
{
    [SerializeField] private List<UnLock> openAreas = new List<UnLock>();

    public void OpenNextArea()
    {
        // foreach (var area in openAreas)
        // {
        //     if (!area.isUnlocked)
        //     {
        //         area.Open();
        //         break;
        //     }
        // }
    }

    public void OpenAllAreas()
    {
        // foreach (var area in openAreas)
        // {
        //     if (!area.IsOpen)
        //         area.Open();
        // }
    }
}