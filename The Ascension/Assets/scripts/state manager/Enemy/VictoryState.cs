using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : EnemyBaseState
{
    private string[] emoteAnimations = { "Victory-3", "Victory" }; // Replace with your animation names

    public VictoryState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        int randomIndex = Random.Range(0, emoteAnimations.Length);
        string chosenAnimation = emoteAnimations[randomIndex];
        stateMachine.Animator.CrossFadeInFixedTime(chosenAnimation, 0.2f);
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {

    }
}
