using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPacingState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("movement");
    private readonly int ForwardHash = Animator.StringToHash("movementForward");
    private readonly int RightHash = Animator.StringToHash("movementRight");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private float pacingRadius = 7f;

    private Vector3[] semiCirclePoints;
    private int currentWaypointIndex = 0;
    private float waypointSwitchCooldown = 1f; // Delay between switches
    private float cooldownTimer = 0f;

    private float stateSwitchDuration = 5f;
    private float stateTimer = 0f;

    public EnemyPacingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
        CreateSemiCirclePoints();
    }

    public override void Tick(float deltaTime)
    {
        cooldownTimer += deltaTime;
        stateTimer += deltaTime;

        RegeneratePoise(deltaTime);
        HandlePoiseBreak();

        // Switch to chasing state if pacing duration ends
        if (stateTimer >= stateSwitchDuration)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        // Attack logic
        if (IsInRange(stateMachine.AttackRange))
        {
            if (Random.Range(0, 100) < 80)
                stateMachine.SwitchState(new EnemyAttackingState(stateMachine, "single"));
            else
                stateMachine.SwitchState(new EnemyBlockingState(stateMachine));

            return;
        }

        MoveSemiCircle(deltaTime);
        FacePlayer(); 
        UpdateMovementAnimation(deltaTime);

        // Recalculate points if the player moves significantly
        if (Vector3.Distance(stateMachine.Player.transform.position, semiCirclePoints[currentWaypointIndex]) > pacingRadius * 1.2f)
        {
            CreateSemiCirclePoints();
        }
    }

    private void CreateSemiCirclePoints()
    {
        int pointsCount = 5;  
        float radius = pacingRadius;  

        semiCirclePoints = new Vector3[pointsCount];
        Vector3 playerPosition = stateMachine.Player.transform.position;
        Vector3 playerForward = stateMachine.Player.transform.forward;

        for (int i = 0; i < pointsCount; i++)
        {
            float angle = Mathf.Lerp(-70f, 70f, (float)i / (pointsCount - 1)); 
            Vector3 offset = Quaternion.Euler(0, angle, 0) * playerForward * radius;
            semiCirclePoints[i] = playerPosition + offset + Vector3.up * 0.5f; // Slight height adjustment to avoid terrain issues
        }

        currentWaypointIndex = 0;
    }

    private void MoveSemiCircle(float deltaTime)
    {
        if (cooldownTimer >= waypointSwitchCooldown)
        {
            stateMachine.Agent.SetDestination(semiCirclePoints[currentWaypointIndex]);

            if (!stateMachine.Agent.pathPending && stateMachine.Agent.remainingDistance < 1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % semiCirclePoints.Length;
                cooldownTimer = 0f;
            }
        }

        // Apply smooth movement
        Vector3 moveDirection = (stateMachine.Agent.desiredVelocity).normalized;
        Move(moveDirection * stateMachine.MovementSpeed, deltaTime);
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }

    private void UpdateMovementAnimation(float deltaTime)
    {
        Vector3 localVelocity = stateMachine.transform.InverseTransformDirection(stateMachine.Agent.velocity);
        float forwardValue = Mathf.Clamp(localVelocity.z, -1f, 1f);
        float rightValue = Mathf.Clamp(localVelocity.x, -1f, 1f);

        stateMachine.Animator.SetFloat(ForwardHash, forwardValue, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat(RightHash, rightValue, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }
}



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class EnemyPacingState : EnemyBaseState
// {
//     //public float CrossFadeDuration = 0.3f;
//     private readonly int LocomotionHash = Animator.StringToHash("movement");
//     private readonly int ForwardHash = Animator.StringToHash("movementForward");
//     private readonly int RightHash = Animator.StringToHash("movementRight");

//     private const float CrossFadeDuration = 0.1f;
//     private const float AnimatorDampTime = 0.1f;
//    // string[] animations = new string[] { "wide leg idle", "bounce idle" };
//     Vector3 randomPosition;
//      private Vector3[] semiCirclePoints;
//     private int currentWaypointIndex = 0;
//     private float waypointSwitchCooldown = 1f; // Delay between switches
//     private float cooldownTimer = 0f;
//     public EnemyPacingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

//     public override void Enter()
//     {
//         //int randomIndex = Random.Range(0, animations.Length);
//         //string animationToPlay = animations[randomIndex];
//         //stateMachine.Animator.CrossFadeInFixedTime(animationToPlay, CrossFadeDuration);
//         stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);

        
//     }

//     public override void Tick(float deltaTime)
//     {
//         cooldownTimer += deltaTime;
//         // celebrateIf();
//         // if (!PlayerStatusInfo.Instance.IsInAttackRange)
//         // {
//         //     stateMachine.SwitchState(new EnemyChasingState(stateMachine));
//         // }
//         CreateSemiCirclePoints();
//         MoveSemiCircle(deltaTime);

//         FacePlayer();
//         UpdateMovementAnimation(deltaTime);

//     }

//      private void CreateSemiCirclePoints()
//     {
//         int pointsCount = 2;  
//         float radius = 5f;   

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
    

//     private void UpdateMovementAnimation(float deltaTime)
//     {
//         Vector3 localVelocity = stateMachine.transform.InverseTransformDirection(stateMachine.Agent.velocity);
//         float forwardValue = Mathf.Clamp(localVelocity.z, -1f, 1f);
//         float rightValue = Mathf.Clamp(localVelocity.x, -1f, 1f);

//         // Set animator parameters based on the agent's forward and right movement
//         stateMachine.Animator.SetFloat(ForwardHash, forwardValue, AnimatorDampTime, deltaTime);
//         stateMachine.Animator.SetFloat(RightHash, rightValue, AnimatorDampTime, deltaTime);
//     }

//     public override void Exit()
//     {
//         stateMachine.Agent.ResetPath();
//         stateMachine.Agent.velocity = Vector3.zero;
//     }
// }



// void SetRandomDestination(float deltaTime)
    // {
    //     stateMachine.Agent.destination = randomPosition;
    //     Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

    //     stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    // }

    // Vector3 GetRandomPositionAroundObject(Vector3 center, float radius)
    // {
    //     Vector2 randomCircle = Random.insideUnitCircle * radius;
    //     Vector3 randomDirection = new Vector3(randomCircle.x, 0f, randomCircle.y);
    //     Vector3 randomPosition = center + randomDirection;

    //     NavMeshHit navMeshHit;
    //     NavMesh.SamplePosition(randomPosition, out navMeshHit, radius, NavMesh.AllAreas);

    //     return navMeshHit.position;
    // }