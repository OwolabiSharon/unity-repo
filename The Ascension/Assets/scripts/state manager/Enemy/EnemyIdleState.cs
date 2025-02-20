using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("movement");
    private readonly int ForwardHash = Animator.StringToHash("movementForward");
    private readonly int RightHash = Animator.StringToHash("movementRight");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime("movement", 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        celebrateIf();
        Move(deltaTime);

        RegeneratePoise(deltaTime);
        HandlePoiseBreak();

        if(IsInChaseRange())
        {
            //FacePlayer();
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        
        
        stateMachine.Animator.SetFloat(ForwardHash, 0f, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat(RightHash, 0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit() { }
}

