using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Target> OnDestroyed;
    // Start is called before the first frame update
    void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
