using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine
{
    protected IState currentState;
    
    public void ChangeState(IState newState)
    {
        if (currentState == newState) return;
        
        // 기존 상태가 있으면 Exit 후 전환
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update(); 
    }
}