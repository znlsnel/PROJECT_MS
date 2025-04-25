using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState currentState;
    private Stack<IState> prevStates = new Stack<IState>();

    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            prevStates.Push(currentState);
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

    public void GoPrevState()
    {
        if (prevStates.Count > 0)
        {
            currentState?.Exit();
            currentState = prevStates.Pop();
            currentState.Enter();
        }
    }

    public void ResetPrevStates()
    {
        prevStates.Clear();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }
}
