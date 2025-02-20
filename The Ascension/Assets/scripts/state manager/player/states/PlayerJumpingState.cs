using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public Vector3 movement;
    private float rantime = 0f;
    private string AnimationName = "dark souls roll";
    private bool isMovingBack = false;
    
    // Start is called before the first frame update"
    public PlayerJumpingState(PlayerStateMachine stateMachine): base(stateMachine){
       stateMachine.currentState = "jumping";
    }
    public override void Enter()
    {
        movement = CalculateMovement();

        Move(movement * stateMachine.JumpSpeed, Time.deltaTime);

        controlAnimations(movement, Time.deltaTime);
    }

    // Update is called once per frame
    public override void Tick(float deltaTime)
    {
        if (isMovingBack)
        {
            // Apply backward movement continuously during Tick
            Vector3 movementBack = CalculateNonInputMovement();

                Move(movementBack * 7f, deltaTime);
        }
        else
        {
            // Regular jump movement
            movement = CalculateMovement();
            Move(movement * stateMachine.JumpSpeed, deltaTime);
        }

        movement = CalculateMovement();
        Move(movement * stateMachine.JumpSpeed, deltaTime);
        CamFaceTarget(deltaTime);
        // FaceMovementDirection(movement, deltaTime);


        rantime += 0.03f;
        if (rantime < .1f) {
            return;
        }
        if (HasJumpAnimationCompleted())
        {
            if (stateMachine.Targeter.currentTarget)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }else
        {
            return;
        }
        
    }

    public override void Exit()
    {
        isMovingBack = false;
        stateMachine.currentState = "";
    }

    private bool IsJumpAnimationPlaying()
    {
        return stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationName);
    }

    private bool HasJumpAnimationCompleted()
{
    AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
    bool isJumpAnimation = stateInfo.IsName(AnimationName);
    
    // Check if the animation has finished and is not in transition
    bool isCompleted = isJumpAnimation && !stateMachine.Animator.IsInTransition(0) && stateInfo.normalizedTime >= 1.0f;
    
    return isCompleted;
}

    private void controlAnimations(Vector3 movement, float deltaTime)
    {
        if (IsJumpAnimationPlaying() || stateMachine.Animator.IsInTransition(0))
        {
            return;
        }

        if (movement == Vector3.zero)
        {
            if (stateMachine.Targeter.currentTarget != null) // Check if there is a target
            {
                AnimationName = "Jump Backward";
                stateMachine.Animator.CrossFadeInFixedTime(AnimationName, 0.2f);
                // Move the character backward
                isMovingBack = true;
                return;
            }
            AnimationName = "Jumping";
            // Play a different animation when movement is zero
            stateMachine.Animator.CrossFadeInFixedTime(AnimationName, 0.2f); // Replace with the name of your idle animation
        }
        else
        {
            // Check if moving backward relative to the camera
            Vector3 forward = stateMachine.MainCameraTransform.forward;
            forward.y = 0f; // Ignore vertical movement
            forward.Normalize();

            float dotProduct = Vector3.Dot(movement.normalized, forward); // Determine direction
            Debug.Log($"dotProduct: {dotProduct}");

            // Check target presence and movement direction
            if (stateMachine.Targeter.currentTarget != null)
            {
                if ( stateMachine.InputReader.MovementValue.y < 0) // Moving backward
                {
                    AnimationName = "Jump Backward";
                    isMovingBack = true;
                }
                else // Moving forward or sideways
                {
                    AnimationName = "dark souls roll";
                    isMovingBack = false;
                }

                stateMachine.Animator.CrossFadeInFixedTime(AnimationName, 0.2f);
            }
            else
            {
                isMovingBack = false;
                AnimationName = "dark souls roll";
                stateMachine.Animator.CrossFadeInFixedTime(AnimationName, 0.2f);
            }

        }
          FaceMovementDirection(movement, deltaTime);
    }
}


