using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingState : PlayerBaseState
{
    public BlockingState(PlayerStateMachine stateMachine): base(stateMachine){
        stateMachine.currentState = "blocking";
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime("block loop", 0.1f);
        PlayerStatusInfo.Instance.playerState = "BlockingState";
        
    }

    public override void Tick(float deltaTime)
    {
        FaceTarget(deltaTime);
        if (!stateMachine.InputReader.IsBlocking)
        {
            if (stateMachine.Targeter.currentTarget)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
        
       
      

    }


    public override void Exit()
    {
        PlayerStatusInfo.Instance.playerState = "";
        stateMachine.currentState = "";
    }



}
