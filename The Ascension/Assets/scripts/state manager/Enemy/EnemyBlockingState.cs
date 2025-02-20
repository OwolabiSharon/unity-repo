using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockingState : EnemyBaseState
{
     
    private Emote emote;
    private float rantime = 0f;
    private bool appliedForce = false;

    public EnemyBlockingState(EnemyStateMachine stateMachine) : base(stateMachine) 
    {
        stateMachine.currentState = "blocking";
    }

    public override void Enter()
    {
        //stateMachine.Weapon.SetAttack(stateMachine.AttackDamage);
        stateMachine.Animator.CrossFadeInFixedTime("block loop", 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        RegeneratePoise(deltaTime);
        HandlePoiseBreak();
        
        FacePlayer();
        rantime += 0.03f;
        if (rantime < .1f) {
            return;
        }
        if (IsAttackAnimationPlaying() || stateMachine.Animator.IsInTransition(0))
        {
            
        }else
        {
            

            //add conditional logic here
            stateMachine.SwitchState(new EnemyPacingState(stateMachine));
        }

        // if (!IsInRange(stateMachine.AttackRange)) 
        // {
        //     PlayerStatusInfo.Instance.IsInAttackRange = false;
        //     stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        // }
        
    }

    private bool IsAttackAnimationPlaying()
    {
        // Debug.Log(stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName(emote.AnimationName));
        return stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName("block loop");
    }

    private void TryApplyForce()
    {
        if(!appliedForce)
        {
            stateMachine.ForceReceiver.AddForce(stateMachine.MainCameraTransform.forward * emote.AttackForce);
        }
        appliedForce = true;
    }


    public override void Exit() { 
        stateMachine.currentState = "";
    }
}



