using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPoiseState : EnemyBaseState
{
    private float poiseBreakDuration;
    private float elapsedTime = 0f;
    private string poiseBreakAnimation;

    public BrokenPoiseState(EnemyStateMachine stateMachine, float duration, string animationName) : base(stateMachine)
    {
        poiseBreakDuration = duration;
        poiseBreakAnimation = animationName;
        
        stateMachine.currentState = "staggered";
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(poiseBreakAnimation, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        elapsedTime += deltaTime;

        if (elapsedTime >= poiseBreakDuration)
        {
            stateMachine.SwitchState(new EnemyBlockingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.currentState = "";
    }
}
