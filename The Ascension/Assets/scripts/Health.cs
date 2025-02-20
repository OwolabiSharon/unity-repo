using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    public int health;
    public PlayerStateMachine playerStateMachine;
    public EnemyStateMachine enemyStateMachine;

    private void Start()
    {
        if (GetComponent<EnemyStateMachine>())
        {
            
        }
        health = maxHealth;
    }


    public void DealDamage(int damage, int poiseDamage)
    {
        if (playerStateMachine != null  && 
        (playerStateMachine.currentState == "attacking" || playerStateMachine.currentState == "jumping"))
            {
                return;
            }
        var stateMachine = (StateMachine)enemyStateMachine ?? playerStateMachine;

        //Debug.Log(health + " " + damage);
        if (health == 0) { 
            
            if (enemyStateMachine != null)
            {
                enemyStateMachine.SwitchState(new EnemyDeadState(enemyStateMachine));
            }
            else
            {
                playerStateMachine.SwitchState(new PlayerDeadState(playerStateMachine));
            }
            return;
        }

        if (enemyStateMachine != null)
        {
            if (enemyStateMachine.currentState == "attacking" && enemyStateMachine.CurrentPoise >= 40)
            {
                enemyStateMachine.CurrentPoise = Mathf.Max(enemyStateMachine.CurrentPoise - poiseDamage, 0);
                return;
            }
            if (enemyStateMachine.currentState == "blocking")
            {
                enemyStateMachine.SwitchState(new EnemyEmoteState(enemyStateMachine, "blockingImpact"));
                return;
            }
            else if (enemyStateMachine.currentState != "staggered")
            {
                enemyStateMachine.SwitchState(new EnemyEmoteState(enemyStateMachine, "impact"));
                enemyStateMachine.CurrentPoise = Mathf.Max(enemyStateMachine.CurrentPoise - poiseDamage, 0);
            }
            health = Mathf.Max(health - damage, 0);

        }

        else
        {
            if (playerStateMachine.currentState == "attacking" && playerStateMachine.CurrentPoise >= 40)
            {
                return;
            }
            if (playerStateMachine.currentState == "blocking")
            {
                playerStateMachine.SwitchState(new PlayerEmoteState(playerStateMachine, "blockingImpact"));
                return;
            }else{
            playerStateMachine.SwitchState(new PlayerEmoteState(playerStateMachine, "impact"));
            }
            health = Mathf.Max(health - damage, 0);
        }
        
    }
    
}

