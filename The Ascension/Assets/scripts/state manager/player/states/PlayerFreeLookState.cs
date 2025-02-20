using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookHash = Animator.StringToHash("freeLookMovement"); 
    private readonly int BlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private const float CrossFadeDuration = 0.2f;
    public PlayerFreeLookState(PlayerStateMachine stateMachine): base(stateMachine){}

    public float AnimatorDampTime = 0.1f;
    public override void Enter()
    {
        stateMachine.closestEnemy = null;
        stateMachine.Animator.CrossFadeInFixedTime(BlendTreeHash, CrossFadeDuration);
        stateMachine.InputReader.TargetEvent += OnTarget;
    }
 
    public override void Tick(float deltaTime)
    {
        HandleAttack();

        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new BlockingState(stateMachine));
            return;
        }
        // if (PlayerStatusInfo.Instance.inEnemyRange)
        // {
        //     stateMachine.SwitchState(new InRangeState(stateMachine));
        //     return;
        // }
        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        controlAnimations(movement, deltaTime);

    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget; 
    }

    private void controlAnimations(Vector3 movement, float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(FreeLookHash, 1, 0.2f, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }


     private void OnTarget()
    {
        if (stateMachine.Targeter.SelectTarget())
        {
           stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }  
    }
    
}
