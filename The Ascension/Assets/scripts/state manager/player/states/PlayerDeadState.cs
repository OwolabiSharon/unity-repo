using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{

    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        PlayerStatusInfo.Instance.isAlive = false;
        int randomIndex = Random.Range(0, stateMachine.Deaths.Length);
        
        stateMachine.Animator.CrossFadeInFixedTime(stateMachine.Deaths[randomIndex], 0.2f);
        stateMachine.Controller.enabled = false;
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit() { }
}
