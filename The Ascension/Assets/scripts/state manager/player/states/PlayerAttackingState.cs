using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private bool appliedForce;
    private bool isHeavy;
    private Attack attack;
    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex, bool isHeavy): base(stateMachine)   
    { 
        stateMachine.currentState = "attacking";
        this.isHeavy = isHeavy; // Use 'this' to assign the parameter to the field

        attack = isHeavy ? stateMachine.HeavyAttacks[attackIndex] : stateMachine.Attacks[attackIndex];
    }
    public override void Enter()
    {
        // Debug.Log(this.isHeavy);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        stateMachine.Weapons[attack.WeaponIndex].SetAttack(attack.Damage, attack.PoiseDamage);
    }

    public override void Tick(float deltaTime)
    {
        // stateMachine.Weapons[attack.WeaponIndex].SetAttack(attack.Damage);
        FindClosestEnemyWithTag();
        Move(deltaTime);

        FaceClosestEnemyWithTag();
        FaceTarget(deltaTime);

        float normalizedTime = GetNormalizedTime();

        // Debug.Log(isHeavy);

        if (normalizedTime < 1f)
        {
            if (normalizedTime >= attack.AttackForceTime)
            {
                TryApplyForce();
            }

            HandleAttackInput(normalizedTime);
        }

        else
        {
           if (stateMachine.Targeter.currentTarget)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
                return;
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        previousFrameTime = normalizedTime;

    }

    public override void Exit()
    {
        stateMachine.currentState = "";
    }

    private void HandleAttackInput(float normalizedTime)
    {
        // Determine the desired attack type based on input
        bool inputIsHeavy = stateMachine.InputReader.IsHeavyAttacking;
        bool inputIsLight = stateMachine.InputReader.IsAttacking;

        // Switch state if the input type doesn't match the current attack type
        if ((isHeavy && inputIsLight) || (!isHeavy && inputIsHeavy))
        {
            if (normalizedTime < attack.ComboAttackTime) { return; }
            
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0, inputIsHeavy));
            return;
        }

        // Proceed with combo logic if the input matches the current attack type
        if (inputIsLight && !isHeavy)
        {
            TryComboAttack(normalizedTime, isHeavy: false);
        }
        else if (inputIsHeavy && isHeavy)
        {
            TryComboAttack(normalizedTime, isHeavy: true);
        }
    }

    private void TryComboAttack(float normalizedTime, bool isHeavy)
    {
        if (attack.ComboStateIndex == -1) { 
            stateMachine.SwitchState
        (
            new PlayerAttackingState
            (
                stateMachine,
                0, 
                isHeavy
            )
        );
         }

        if (normalizedTime < attack.ComboAttackTime) { return; }
        
        stateMachine.SwitchState
        (
            new PlayerAttackingState
            (
                stateMachine,
                attack.ComboStateIndex, 
                isHeavy
            )
        );
    }


    private float GetNormalizedTime()
    {
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    private void TryApplyForce()
    {
        if(!appliedForce)
        {
            stateMachine.ForceReceiver.AddForce(stateMachine.MainCameraTransform.forward * attack.AttackForce);
        }
        appliedForce = true;
    }

}