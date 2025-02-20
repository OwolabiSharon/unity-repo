using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    // [field: SerializeField] public WeaponDamage Weapon { get; private set; }

    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
     [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public Attack[] Attacks {get; private set;}
    [field: SerializeField] public Attack[] Combos {get; private set;}
    [field: SerializeField] public Emote[] Emotes {get; set;}
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public WeaponDamage[] Weapons { get; private set; }

    [field: SerializeField] public string currentState { get; set; }

    public GameObject Player { get; private set; }
    [field: SerializeField] public Transform pointMove { get; private set; }
    public Transform MainCameraTransform {get; private set;}

    // Poise meter properties
    [field: SerializeField] public float MaxPoise { get; private set; } = 100f;
    [field: SerializeField] public float PoiseRegenRate { get; private set; } = 5f;
    [field: SerializeField] public float CurrentPoise { get; set; } = 100f;

    private void Start()
    {
        MainCameraTransform = Camera.main.transform;
        Player = GameObject.FindGameObjectWithTag("Player");

        Agent.updatePosition = false;
        Agent.updateRotation = true;

        SwitchState(new EnemyIdleState(this));
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }
}


