using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    } 

    protected void FacePlayer()
    {
        if (stateMachine.Player == null) { return; }

        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected bool IsInChaseRange()
    {
        
        if (stateMachine.Player == null || !PlayerStatusInfo.Instance.isAlive) { return false; }
        Debug.Log("The first Herrroo");
        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        
        return playerDistanceSqr <= stateMachine.PlayerChasingRange * stateMachine.PlayerChasingRange;
    }

    protected bool IsInRange(float range)
    {
        if (stateMachine.Player == null || !PlayerStatusInfo.Instance.isAlive) 
        { 
            return false; 
        }

        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return playerDistanceSqr <= range * range;
    }


     protected bool IsInAttackRange()
    {
        if (stateMachine.Player == null || !PlayerStatusInfo.Instance.isAlive) { return false; }
        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

    protected void celebrateIf()
    {
        if (!PlayerStatusInfo.Instance.isAlive) 
        { 
            stateMachine.SwitchState(new VictoryState(stateMachine));
            return;
        }
    }

    protected void RegeneratePoise(float deltaTime)
    {
        if (stateMachine.CurrentPoise < stateMachine.MaxPoise)
        {
            stateMachine.CurrentPoise += stateMachine.PoiseRegenRate * deltaTime;
            stateMachine.CurrentPoise = Mathf.Min(stateMachine.CurrentPoise, stateMachine.MaxPoise); // Clamp to MaxPoise
        }
    }

    protected void HandlePoiseBreak()
    {
        if (stateMachine.CurrentPoise <= 1)
        {
            stateMachine.SwitchState(new BrokenPoiseState(stateMachine, 3.0f, "injured idle"));
            stateMachine.CurrentPoise = stateMachine.MaxPoise;
            return;
        }
    }
 
}

