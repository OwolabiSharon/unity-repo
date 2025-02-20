using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State 
{
    protected PlayerStateMachine stateMachine;
    protected float lookRotationSpeed = 6f;

    public PlayerBaseState(PlayerStateMachine stateMachine)
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

    protected Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }

    protected Vector3 CalculateNonInputMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * -1 + right * 0;
    }

    protected void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping
        );
    }
    protected void FaceTarget(float deltaTime)
{
    if (stateMachine.Targeter.currentTarget == null) { return; }

    // Calculate the direction to look at
    Vector3 lookDirection = stateMachine.Targeter.currentTarget.transform.position - stateMachine.transform.position;
    lookDirection.y = 0f; // Keep the y-axis fixed

    // Calculate the target rotation
    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

    // Smoothly rotate towards the target using Slerp for more natural movement
    stateMachine.transform.rotation = Quaternion.Slerp(
        stateMachine.transform.rotation,
        targetRotation,
        deltaTime * lookRotationSpeed // Adjust rotation speed here
    );
}

protected void FaceClosestEnemyWithTag()
    {
        if (stateMachine.Targeter.currentTarget != null) { return; }
        if (stateMachine.closestEnemy == null){return;}

        Vector3 enemyPosition = stateMachine.closestEnemy.transform.position;
        Vector3 currentPosition = stateMachine.transform.position;
        float distanceToEnemy = Vector3.Distance(currentPosition, enemyPosition);

        if (distanceToEnemy > stateMachine.faceTargetRange)
        {
            return; // Return if the closest enemy is beyond the specified range.
        }
        Vector3 LookPosition = stateMachine.closestEnemy.transform.position - stateMachine.transform.position;
        LookPosition.y = 0f;
        //stateMachine.transform.rotation = Quaternion.LookRotation(LookPosition);
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(LookPosition), Time.deltaTime * lookRotationSpeed);
        // stateMachine.transform.rotation = Quaternion.Lerp(
        //     stateMachine.transform.rotation,
        //     Quaternion.LookRotation(pos),
        //     deltaTime * stateMachine.RotationDamping
        // );
    }

 protected void CamFaceTarget(float deltaTime)
    {
        if (stateMachine.Targeter.currentTarget == null){return;}
        Vector3 LookPosition = stateMachine.Targeter.currentTarget.transform.position - stateMachine.transform.position;
        LookPosition.y = 0f;
        //stateMachine.cameraFocus.rotation = Quaternion.LookRotation(LookPosition);
        Quaternion targetRotation = Quaternion.LookRotation(LookPosition);

        // Smoothly rotate towards the target using Slerp for more natural movement
        stateMachine.cameraFocus.rotation = Quaternion.Slerp(
            stateMachine.cameraFocus.rotation,
            targetRotation,
            deltaTime * lookRotationSpeed // Adjust rotation speed here
        );
    }

 protected void HandleAttack(int lightAttackIndex = 0, int heavyAttackIndex = 0)
{
    // Check if a light attack is being triggered
    if (stateMachine.InputReader.IsAttacking)
    {
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, lightAttackIndex, isHeavy: false));
        return;
    }

    // Check if a heavy attack is being triggered
    if (stateMachine.InputReader.IsHeavyAttacking)
    {
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, heavyAttackIndex, isHeavy: true));
        return;
    }
}



    protected void FindClosestEnemyWithTag()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(stateMachine.transform.position, enemy.transform.position);

            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                stateMachine.closestEnemy = enemy;
            }
        }

    }


    private void RegeneratePoise(float deltaTime)
    {
        if (stateMachine.CurrentPoise < stateMachine.MaxPoise)
        {
            stateMachine.CurrentPoise += stateMachine.PoiseRegenRate * deltaTime;
            stateMachine.CurrentPoise = Mathf.Min(stateMachine.CurrentPoise, stateMachine.MaxPoise); // Clamp to MaxPoise
        }
    }

}

    // protected void FaceTarget()
    // {
    //     if (stateMachine.closestEnemy == null){return;}
    //     Vector3 enemyPosition = stateMachine.closestEnemy.transform.position;
    //     Vector3 currentPosition = stateMachine.transform.position;
    //     float distanceToEnemy = Vector3.Distance(currentPosition, enemyPosition);

    //     if (distanceToEnemy > stateMachine.faceTargetRange)
    //     {
    //         return; // Return if the closest enemy is beyond the specified range.
    //     }
    //     Vector3 LookPosition = stateMachine.closestEnemy.transform.position - stateMachine.transform.position;
    //     LookPosition.y = 0f;
    //     //stateMachine.transform.rotation = Quaternion.LookRotation(LookPosition);
    //     stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(LookPosition), Time.deltaTime * lookRotationSpeed);
    //     // stateMachine.transform.rotation = Quaternion.Lerp(
    //     //     stateMachine.transform.rotation,
    //     //     Quaternion.LookRotation(pos),
    //     //     deltaTime * stateMachine.RotationDamping
    //     // );
    // }