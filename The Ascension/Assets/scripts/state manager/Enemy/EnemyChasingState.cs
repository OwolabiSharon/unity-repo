using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("movement");
    private readonly int ForwardHash = Animator.StringToHash("movementForward");
    private readonly int RightHash = Animator.StringToHash("movementRight");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private float stateSwitchDuration = 5f; 
    private float stateTimer = 0f;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine) { }


    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        RegeneratePoise(deltaTime);
        HandlePoiseBreak();
        stateTimer += deltaTime;

        celebrateIf();
        PlayerStatusInfo.Instance.inEnemyRange = true;

        if ((stateTimer >= stateSwitchDuration) && !IsInRange(stateMachine.AttackRange))
        {
            // PlayerStatusInfo.Instance.inEnemyRange = false;
            stateMachine.SwitchState(new EnemyPacingState(stateMachine));
            return;
        }
        else if (IsInRange(stateMachine.AttackRange))
        {
            //
            //PlayerStatusInfo.Instance.inEnemyRange = false;
            //stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine, "combo"));
            return;
        }

        MoveToPlayer(deltaTime);

        FacePlayer();

        stateMachine.Animator.SetFloat(ForwardHash, 1f, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat(RightHash, 0, AnimatorDampTime, deltaTime);
        if (PlayerStatusInfo.Instance.IsInAttackRange)
        {
            stateMachine.SwitchState(new EnemyPacingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    private void MoveToPlayer(float deltaTime)
    {
        stateMachine.Agent.destination = stateMachine.Player.transform.position;

        Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class EnemyChasingState : EnemyBaseState
// {
//     private readonly int LocomotionHash = Animator.StringToHash("movement");
//     private readonly int ForwardHash = Animator.StringToHash("movementForward");
//     private readonly int RightHash = Animator.StringToHash("movementRight");

//     private const float CrossFadeDuration = 0.1f;
//     private const float AnimatorDampTime = 0.1f;

//     private Vector3[] semiCirclePoints;
//     private int currentWaypointIndex = 0;
//     private float waypointSwitchCooldown = 10f; // Delay between switches
//     private float cooldownTimer = 0f;

//     public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

//     public override void Enter()
//     {
//         stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
        
//         // Define semi-circle points around the target
//         CreateSemiCirclePoints();
//     }

//     public override void Tick(float deltaTime)
//     {
//         cooldownTimer += deltaTime;

//         if (!IsInChaseRange())
//         {
//             stateMachine.SwitchState(new EnemyIdleState(stateMachine));
//             return;
//         }
//         else if (IsInAttackRange())
//         {
//             stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
//             return;
//         }

//         MoveSemiCircle(deltaTime);

//         stateMachine.Animator.SetFloat(ForwardHash, 1f, AnimatorDampTime, deltaTime);
//         stateMachine.Animator.SetFloat(RightHash, 0, AnimatorDampTime, deltaTime);
//     }

//     public override void Exit()
//     {
//         stateMachine.Agent.ResetPath();
//         stateMachine.Agent.velocity = Vector3.zero;
//     }

//     private void CreateSemiCirclePoints()
//     {
//         int pointsCount = 5;  // Number of points in the semi-circle
//         float radius = 5f;    // Radius around the player to keep distance

//         semiCirclePoints = new Vector3[pointsCount];
//         for (int i = 0; i < pointsCount; i++)
//         {
//             // Calculate points in a semi-circle
//             float angle = Mathf.Lerp(-90f, 90f, (float)i / (pointsCount - 1));
//             Vector3 offset = Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
//             semiCirclePoints[i] = stateMachine.Player.transform.position + offset;
//         }
//     }

//     private void MoveSemiCircle(float deltaTime)
//     {
//         if (cooldownTimer >= waypointSwitchCooldown)
//         {
//             // Set destination to the current waypoint
//             stateMachine.Agent.destination = semiCirclePoints[currentWaypointIndex];

//             // Check if agent is close enough to switch to the next waypoint
//             if (Vector3.Distance(stateMachine.Agent.transform.position, semiCirclePoints[currentWaypointIndex]) < 1f)
//             {
//                 // Go to next waypoint
//                 currentWaypointIndex = (currentWaypointIndex + 1) % semiCirclePoints.Length;
//                 cooldownTimer = 0f;
//             }
//         }

//         Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
//         stateMachine.Agent.velocity = stateMachine.Controller.velocity;
//     }
// }
