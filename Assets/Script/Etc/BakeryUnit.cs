using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { Oven, ShowCase }

public class BakeryUnit : MonoBehaviour
{
    // public UnitType unitType;
    // public Stack<GameObject> breadList = new Stack<GameObject>();
    // private BreadSpawner BreadSpawner;
    //
    // private void Awake()
    // {
    //     if (unitType == UnitType.Oven) BreadSpawner = GetComponent<BreadSpawner>();
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     Player player = other.GetComponent<Player>();
    //     if (player == null) return;
    //
    //     Debug.Log($"{breadList.Count} :: 빵수량");
    //     
    //     switch(unitType)
    //     {
    //         case UnitType.Oven:
    //             player.Bread.Push(breadList.Pop());
    //             BreadSpawner.Bread--;
    //             player.PlayerStateMachine.ChangeState(new BakingState(player.PlayerStateMachine));
    //             
    //             break;
    //
    //         case UnitType.ShowCase:
    //             player.PlayerStateMachine.ChangeState(new RestockState(player.PlayerStateMachine));
    //             break;
    //     }
    // }
}