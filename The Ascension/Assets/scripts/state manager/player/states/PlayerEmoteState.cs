using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEmoteState : PlayerBaseState
{
    private float previousFrameTime;
    private bool appliedForce;
    string AnimationName;
    private Emote emote;
    public PlayerEmoteState(PlayerStateMachine stateMachine, string emoteType): base(stateMachine)
    {
        // int randomIndex = Random.Range(0, stateMachine.Emotes.Length);
        
        // Use the randomly selected index to get the Attack object
        emote = stateMachine.Emotes[0];
        if (emoteType == "blockingImpact")
        {
            AnimationName = "blockingImpact";
        }else{
            AnimationName = emote.AnimationName;
        }

        //if emote is -1 its kick and play kick
    }
    public override void Enter()
    {
        //stateMachine.Weapon.SetAttack(attack.Damage);
        stateMachine.Animator.CrossFadeInFixedTime(AnimationName, emote.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        // FindClosestEnemyWithTag();
        Move(deltaTime);

        FaceTarget(deltaTime);

        float normalizedTime = GetNormalizedTime();

        if (normalizedTime < 1f)
        {
            if (normalizedTime >= emote.AttackForceTime)
            {
                TryApplyForce();
            }

            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
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
        
    }

    private void TryComboAttack(float normalizedTime)
    {
        // if (emote.ComboStateIndex == -1) { return; }

        // if (normalizedTime < emote.ComboAttackTime) { return; }

        // stateMachine.SwitchState
        // (
        //     new PlayerAttackingState
        //     (
        //         stateMachine,
        //         1
        //     )
        // );
    }


    private float GetNormalizedTime()
    {
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Emote"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Emote"))
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
            // stateMachine.ForceReceiver.AddForce(-stateMachine.MainCameraTransform.forward * emote.AttackForce);
            stateMachine.ForceReceiver.AddForce(-stateMachine.transform.forward * emote.AttackForce);
        }
        appliedForce = true;
    }
}
