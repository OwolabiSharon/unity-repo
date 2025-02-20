using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeadState : EnemyBaseState
{
    private float destroyTimer = 10.0f; // Adjust the time (in seconds) as needed
    private float elapsedTimer = 0.0f;

    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        
        // stateMachine.Player.GetComponent<PlayerStateMachine>().closestEnemy = null;
        // PlayerStatusInfo.Instance.totalGuys = PlayerStatusInfo.Instance.totalGuys - 1;
        // stateMachine.Ragdoll.ToggleRagdoll(true);
        PlayerStatusInfo.Instance.inEnemyRange = false;
        PlayerStatusInfo.Instance.IsInAttackRange = false;
        //stateMachine.Weapon.gameObject.SetActive(false);
        stateMachine.Animator.CrossFadeInFixedTime("Falling Forward Death", 0.2f);

        // Remove the tag (assuming you want to remove the tag from the GameObject)
        stateMachine.gameObject.tag = "Untagged";
        elapsedTimer = 0.0f;
    }

    public override void Tick(float deltaTime)
    {
        elapsedTimer += deltaTime;

        if (elapsedTimer >= destroyTimer)
        {
            // Destroy the object that this script is attached to
            GameObject.Destroy(stateMachine.gameObject);
        }
    }

    public override void Exit() { }
}

