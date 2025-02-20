using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStatusInfo : MonoBehaviour
{
    // Static reference to the instance of the Singleton.
    public static PlayerStatusInfo Instance { get; private set; }

    // Example variable
    public bool inEnemyRange = false;
    public bool IsInAttackRange = false;
    public string playerState = ""; 
    public bool isAlive = true;
    public float totalGuys = 2f;
    public Transform center;
    public GameObject enemy;
    public float range;

    private void Awake()
    {
        // Check if an instance already exists.
        if (Instance == null)
        {
            // If not, set this instance as the Singleton.
            Instance = this;
        }
        else
        {
            // If an instance already exists, destroy this GameObject.
            Debug.LogWarning("Another instance of SingletonExample already exists. Destroying this one.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (totalGuys == 2) { return; }
        if (totalGuys < 2)
        {
            InstantiateObjectAtRandomPosition(range);
            totalGuys = totalGuys + 1;
        }
    }

    void InstantiateObjectAtRandomPosition(float range) {
        // Generate a random position within the specified range
        Vector3 randomPosition = center.position + Random.insideUnitSphere * range;
        randomPosition.y = 0;
        Debug.Log(randomPosition);
        // Check if the random position is on the NavMesh (optional)
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, 100f, NavMesh.AllAreas)) {
            Vector3 nearestNavMeshPoint = hit.position;
            // Instantiate the object at the valid position
            GameObject newObject = Instantiate(enemy, nearestNavMeshPoint , Quaternion.identity);
            //Instantiate(objectToInstantiate, nearestNavMeshPoint, Quaternion.identity);
        } else {
            // Handle the case where no valid position was found within maxDistance
        }
    }
}
