using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue{get; private set;}
    public bool IsAttacking{get; private set;}
    public bool IsHeavyAttacking{get; private set;}
    public bool IsBlocking{get; private set;}
    public event Action JumpEvent;
    public event Action dodgeEvent;
    public event Action TargetEvent;
    public event Action BlockEvent;
    private Controls controls;
    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy() {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed){ return; }
        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(!context.performed){ return; }
        dodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
    }
    public void OnTarget(InputAction.CallbackContext context)
    {
        if(!context.performed){ return; }
        TargetEvent?.Invoke();
    }
    public void OnRes(InputAction.CallbackContext context)
    {
        if(!context.performed){ return; }
        BlockEvent?.Invoke();
        int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the scene with the same build index to restart it
        SceneManager.LoadScene(currentSceneBuildIndex);
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if(context.performed)
        { 
            IsBlocking = true;
        }
        else if(context.canceled)
        {
            IsBlocking = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        { 
            IsAttacking = true;
        }
        else if(context.canceled)
        {
            IsAttacking = false;
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        { 
            IsHeavyAttacking = true;
        }
        else if(context.canceled)
        {
            IsHeavyAttacking = false;
        }
    }
}
