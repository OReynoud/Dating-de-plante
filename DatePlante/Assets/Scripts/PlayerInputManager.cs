using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction touchPosAction;
    private InputAction touchPressAction;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        touchPosAction = _playerInput.actions["TouchPos"];    
        touchPressAction = _playerInput.actions["Tap"];   
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressActionOnPerformed;
    }
    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressActionOnPerformed;
    }
    private void TouchPressActionOnPerformed(InputAction.CallbackContext obj)
    {
        float value = obj.ReadValue<float>();
        
        Vector3 position = touchPosAction.ReadValue<Vector2>();
        _playerInput.camera.ScreenToWorldPoint(position);
        position.z = 0;
    }
    private void TouchPosActionOnPerformed(InputAction.CallbackContext obj)
    {
        
    }




}
