using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine
{
    public IState currentState;
    
    public void ChangeState(IState newState)
    {
        if (currentState == newState) return;
        
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