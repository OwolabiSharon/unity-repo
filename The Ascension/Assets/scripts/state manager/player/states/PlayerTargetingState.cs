using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    public PlayerTargetingState(PlayerStateMachine stateMachine): base(stateMachine){}
    private readonly int BlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");
    public float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.2f;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BlendTreeHash, CrossFadeDuration);
        stateMachine.InputReader.TargetEvent += OnCancel;
    }

    public override void Tick(float deltaTime)
    {

        HandleAttack();
        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new BlockingState(stateMachine));
            return;
        }
        if (stateMachine.Targeter.currentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
        UpdateAnimator(deltaTime);
        CamFaceTarget(deltaTime);
        FaceTarget(deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnCancel;
    }

    private void OnCancel()
    {
        stateMachine.Targeter.CancelTarget();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
        
        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        // works better for keyboardandmouseScheme
        // float forward = stateMachine.InputReader.MovementValue.x;
        // float right = stateMachine.InputReader.MovementValue.y;

        // stateMachine.Animator.SetFloat(TargetingForwardHash, forward, AnimatorDampTime, deltaTime);
        // stateMachine.Animator.SetFloat(TargetingRightHash, right, AnimatorDampTime, deltaTime);
        if(stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(TargetingForwardHash, 0, AnimatorDampTime, deltaTime);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.y > 0? 1f: -1f;
            stateMachine.Animator.SetFloat(TargetingForwardHash, value, AnimatorDampTime, deltaTime);
        }

        if(stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(TargetingRightHash, 0, AnimatorDampTime, deltaTime);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.x > 0? 1f: -1f;
            stateMachine.Animator.SetFloat(TargetingRightHash, value, AnimatorDampTime, deltaTime);
        }
    }
}
 