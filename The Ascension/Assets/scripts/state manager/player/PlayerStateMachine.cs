using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine 
{
    [field: SerializeField] public InputReader InputReader {get; private set;}
    [field: SerializeField] public CharacterController Controller {get; private set;}
    [field: SerializeField] public Animator Animator {get; private set;}
    [field: SerializeField] public GameObject closestEnemy;
    [field: SerializeField] public float FreeLookMovementSpeed {get; private set;}
    [field: SerializeField] public float AttackMovementSpeed {get; private set;}
    [field: SerializeField] public float JumpSpeed {get; private set;}
    [field: SerializeField] public float RotationDamping {get; private set;}
    [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
    //[field: SerializeField] public bool isInAttackState {get; private set;}
    [field: SerializeField] public Attack[] Attacks {get; private set;}
    [field: SerializeField] public Attack[] HeavyAttacks {get; private set;}

     [field: SerializeField] public string[] Deaths {get; private set;}
     [field: SerializeField] public Emote[] Emotes {get; set;}
     [field: SerializeField] public WeaponDamage[] Weapons { get; private set; }
    public Transform MainCameraTransform {get; private set;}
    [field: SerializeField] public float faceTargetRange { get; private set; }
    [field: SerializeField] public Targeter Targeter {get; private set;}
    [field: SerializeField] public float TargetingMovementSpeed {get; private set;}

    // Poise meter properties
    [field: SerializeField] public float MaxPoise { get; private set; } = 100f;
    [field: SerializeField] public float PoiseRegenRate { get; private set; } = 5f; // Poise regen per second
    [field: SerializeField] public float CurrentPoise { get; set; }

    [field: SerializeField] public Transform cameraFocus {get; private set;}
    [field: SerializeField] public string currentState { get; set; }
    // Start is called before the first frame update
    void Start() 
    {
        MainCameraTransform = Camera.main.transform;
        SwitchState(new PlayerFreeLookState(this));
        InputReader.JumpEvent += JumpFunction;
    }

    void JumpFunction() 
    {
        Debug.Log(PlayerStatusInfo.Instance);
        if (!PlayerStatusInfo.Instance.isAlive) { return; }
        if (IsJumpAnimationPlaying() || Animator.IsInTransition(0))
        {
            return;
        }
        SwitchState(new PlayerJumpingState(this));
    }
    private bool IsJumpAnimationPlaying()
    {
        return Animator.GetCurrentAnimatorStateInfo(0).IsName("dark souls roll");
    }
}
