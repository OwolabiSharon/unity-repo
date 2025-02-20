using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeState : PlayerBaseState
{
    private readonly int AttackHash = Animator.StringToHash("combatMovement");
    private readonly int BlendTreeHash = Animator.StringToHash("movement");
     private float previousFrameTime;
    public InRangeState(PlayerStateMachine stateMachine): base(stateMachine){}
//     private bool appliedForce;
//     private Attack attack;
    public override void Enter()
    {
        
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, 0.082f);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            // stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            // return;
        }
        
        if (!PlayerStatusInfo.Instance.inEnemyRange)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.AttackMovementSpeed, deltaTime);

        controlAnimations(movement, deltaTime);
        
      

    }

    private void controlAnimations(Vector3 movement, float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(BlendTreeHash, 0, 0.1f, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(BlendTreeHash, 1, 0.1f, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        
    }



}
