using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    // Update is called once per frame
    void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public void SwitchState(State nextState)
    {
        currentState?.Exit();
        currentState = nextState;
        currentState?.Enter();
    }
}
