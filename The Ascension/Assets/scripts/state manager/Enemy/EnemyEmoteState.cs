using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEmoteState : EnemyBaseState
{
     
    private Emote emote;
    string AnimationName;
    private float rantime = 0f;
    private bool appliedForce = false;

    public EnemyEmoteState(EnemyStateMachine stateMachine, string emoteType) : base(stateMachine) 
    {
        // int randomIndex = Random.Range(0, stateMachine.Emotes.Length);
        //Debug.Log(stateMachine);
        // Use the randomly selected index to get the Attack object
        emote = stateMachine.Emotes[0];
        if (emoteType == "blockingImpact")
        {
            AnimationName = "blockingImpact";
        }else{
            AnimationName = emote.AnimationName;
        }
    }

    public override void Enter()
    {
        
        //stateMachine.Weapon.SetAttack(stateMachine.AttackDamage);
        stateMachine.Animator.CrossFadeInFixedTime(AnimationName, 0.1f);
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
        return stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationName);
    }

    // private void TryApplyForce()
    // {
    //     if(!appliedForce)
    //     {
    //         stateMachine.ForceReceiver.AddForce(stateMachine.MainCameraTransform.forward * emote.AttackForce);
    //     }
    //     appliedForce = true;
    // }


    public override void Exit() { }
}





//test sequence 

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EnemyEmoteState : EnemyBaseState
// {
     
//     private Emote emote;
//     private float rantime = 0f;
//     private bool appliedForce = false;

//     public EnemyEmoteState(EnemyStateMachine stateMachine) : base(stateMachine) 
//     {
//         Debug.Log("im emoting");
//         int randomIndex = Random.Range(0, stateMachine.Emotes.Length);
//         //Debug.Log(stateMachine);
//         // Use the randomly selected index to get the Attack object
//         emote = stateMachine.Emotes[randomIndex];
//         Debug.Log(emote.AnimationName);
//     }

//     public override void Enter()
//     {
        
//         //stateMachine.Weapon.SetAttack(stateMachine.AttackDamage);
//         stateMachine.Animator.CrossFadeInFixedTime(emote.AnimationName, 0.1f);
//     }

//     public override void Tick(float deltaTime)
//     {
//         Debug.Log("im emoting");
//         FacePlayer();
//         rantime += 0.03f;
//         if (rantime < .1f) {
//             return;
//         }
//         if (IsAttackAnimationPlaying() || stateMachine.Animator.IsInTransition(0))
//         {
//             Debug.Log("emote is playing or is transitioning");
//         }else
//         {
//             Debug.Log("animation isnt playing");

//                 stateMachine.SwitchState(new EnemyPacingState(stateMachine));

//         }

//         // if (!IsInRange(stateMachine.AttackRange)) 
//         // {
//         //     PlayerStatusInfo.Instance.IsInAttackRange = false;
//         //     stateMachine.SwitchState(new EnemyChasingState(stateMachine));
//         // }
        
//     }

//     private bool IsAttackAnimationPlaying()
//     {
//         // Debug.Log(stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName(emote.AnimationName));
//         return stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName(emote.AnimationName);
//     }

//     private void TryApplyForce()
//     {
//         if(!appliedForce)
//         {
//             stateMachine.ForceReceiver.AddForce(stateMachine.MainCameraTransform.forward * emote.AttackForce);
//         }
//         appliedForce = true;
//     }


//     public override void Exit() { }
// }
