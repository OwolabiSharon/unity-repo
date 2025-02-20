using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMovement : MonoBehaviour
{
    public float speed = 2.0f;
    public float distance = 3.0f;

    private Vector3 startPosition;
    private bool movingForward = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float movement = speed * Time.deltaTime;
        
        if (movingForward)
        {
            transform.position += transform.forward * movement;

            if (Vector3.Distance(startPosition, transform.position) >= distance)
            {
                movingForward = false;
            }
        }
        else
        {
            transform.position -= transform.forward * movement;

            if (Vector3.Distance(startPosition, transform.position) <= 0.1f)
            {
                movingForward = true;
            }
        }
    }
}
