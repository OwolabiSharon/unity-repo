using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Targeter : MonoBehaviour
{
    private List<Target> targets = new List<Target>();
    [SerializeField] private CinemachineTargetGroup cinemachineTarget;
    public Target currentTarget {get; private set; }
    Camera mainCamera;

    private void Start() 
    {
        mainCamera = Camera.main;
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            targets.Add(target);
            target.OnDestroyed += RemoveTarget;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            RemoveTarget(target);
        }
    }

    public bool SelectTarget()
    {
        if (targets.Count > 0)
        {
            Target closestTarget = null;
            float closestTargetDistance = Mathf.Infinity;
            foreach (Target target in targets)
            {
                Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

                if(viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
                {
                    continue;
                }
                Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
                if(toCenter.sqrMagnitude < closestTargetDistance)
                {
                    closestTarget = target;
                    closestTargetDistance = toCenter.sqrMagnitude;
                }
            }
            currentTarget = closestTarget;
            cinemachineTarget.AddMember(currentTarget.transform, 0.4f, 2f);
            return true;
        }
        else{return false;}

    }

    public void CancelTarget()
    {
        if(currentTarget)
        {
            cinemachineTarget.RemoveMember(currentTarget.transform);
            currentTarget = null;
        }
        
    }

    void RemoveTarget(Target target)
    {
        if (currentTarget == target)
        {
            cinemachineTarget.RemoveMember(currentTarget.transform);
            currentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}
