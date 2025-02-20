using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private Attack attack;
    private string attackTypeString;
    private float rantime = 0f;
    private int randomIndex;



    public EnemyAttackingState(EnemyStateMachine stateMachine, string attackType) : base(stateMachine) 
    {
        randomIndex = Random.Range(0, stateMachine.Attacks.Length); 
        // Use the randomly selected index to get the Attack object
        if (attackType == "combo")
        {
            randomIndex = Random.Range(0, stateMachine.Combos.Length); 
            attack = stateMachine.Combos[randomIndex];
        }else
        {
            attack = stateMachine.Attacks[randomIndex];
        }

        stateMachine.currentState = "attacking";

    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        stateMachine.Weapons[attack.WeaponIndex].SetAttack(attack.Damage, attack.PoiseDamage);
        //set attack speed here too if possible
    }

    public override void Tick(float deltaTime)
    {
        RegeneratePoise(deltaTime);
        HandlePoiseBreak();
        // stateMachine.Weapons[attack.WeaponIndex].SetAttack(attack.Damage);
        // celebrateIf();
        //PlayerStatusInfo.Instance.IsInAttackRange = true;

        rantime += 0.03f;
        if (rantime < .2f) {
            return;
        }

        FacePlayer();

        if (IsAttackAnimationPlaying() || stateMachine.Animator.IsInTransition(0))
        {
            Debug.Log("emote is playing or is transitioning");
        }else
        {
            Debug.Log("animation isnt playing");

            //add conditional logic here
            stateMachine.SwitchState(new EnemyPacingState(stateMachine));
        }
        

        // if (!IsInRange(stateMachine.AttackRange)) 
        // {
        //     PlayerStatusInfo.Instance.IsInAttackRange = false;
        //     stateMachine.SwitchState(new EnemyPacingState(stateMachine));
        // }
    
    }

    private bool IsAttackAnimationPlaying()
    {
    return stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName(attack.AnimationName);
    }

    private void PlayRandomAttackAnimation()
    {
        int randomIndex = Random.Range(0, stateMachine.Attacks.Length);
        attack = stateMachine.Attacks[randomIndex];
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Exit() {
        stateMachine.currentState = "";
     }
}





// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EnemyAttackingState : EnemyBaseState
// {
//     private Attack attack;

    

//     public EnemyAttackingState(EnemyStateMachine stateMachine, string attackType) : base(stateMachine) 
//     {
//         int randomIndex = Random.Range(0, stateMachine.Attacks.Length); 
//         // Use the randomly selected index to get the Attack object
//         attack = stateMachine.Attacks[randomIndex];
//     }

//     public override void Enter()
//     {
//         stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
//         //set attack speed here too if possible
//     }

//     public override void Tick(float deltaTime)
//     {
//         stateMachine.Weapons[attack.WeaponIndex].SetAttack(attack.Damage);
//         // celebrateIf();
//         //PlayerStatusInfo.Instance.IsInAttackRange = true;
//         FacePlayer();
//         if (!IsInRange(stateMachine.AttackRange)) 
//         {
//             PlayerStatusInfo.Instance.IsInAttackRange = false;
//             stateMachine.SwitchState(new EnemyPacingState(stateMachine));
//         }
//         else if (IsAttackAnimationPlaying() || stateMachine.Animator.IsInTransition(0))
//         { 
//         //     stateMachine.SwitchState
//         // (
//         //     new EnemyAttackingState
//         //     (
//         //         stateMachine
//         //     )
//         // );
//             //PlayRandomAttackAnimation();
//         }else
//         {
//             PlayRandomAttackAnimation();
//         }
        
//     }

//     private bool IsAttackAnimationPlaying()
//     {
//     return stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName(attack.AnimationName);
//     }

//     private void PlayRandomAttackAnimation()
//     {
//         int randomIndex = Random.Range(0, stateMachine.Attacks.Length);
//         attack = stateMachine.Attacks[randomIndex];
//         stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
//     }

//     public override void Exit() { }
// }

