using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    private PlayerInput _playerInput;
    private InputAction touchPosAction;
    private InputAction touchPressAction;
    
    public Vector3 previousPos { get; set; }
    public Vector3 touchWorldPos;
    public float touchSize;
    public LayerMask canDragMask;
    public Transform objectToDrag;
    public float dragLerp;
    public UnityEvent OnTouchRelease { get; set; } = new();

    private float pressTime;
    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
        }

        instance = this;
        _playerInput = GetComponent<PlayerInput>();
        touchPosAction = _playerInput.actions["TouchPos"];    
        touchPressAction = _playerInput.actions["Tap"];   
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressActionOnPerformed;
        touchPressAction.canceled += TouchPressActionOnCancelled;
    }



    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressActionOnPerformed;
        touchPressAction.canceled += TouchPressActionOnCancelled;
    }

    private void Update()
    {
        previousPos = touchWorldPos;
        touchWorldPos = _playerInput.camera.ScreenToWorldPoint(touchPosAction.ReadValue<Vector2>());
        touchWorldPos.z = 0;
        if (objectToDrag)
        {        
            objectToDrag.position = Vector3.Lerp(objectToDrag.position, touchWorldPos,dragLerp);
        }
    }

    private void TouchPressActionOnPerformed(InputAction.CallbackContext obj)
    {
        float value = obj.ReadValue<float>();
        touchWorldPos = _playerInput.camera.ScreenToWorldPoint(touchPosAction.ReadValue<Vector2>());
        touchWorldPos.z = 0;
        previousPos = touchWorldPos;
        if (Physics.SphereCast(touchWorldPos + Vector3.back * 3,touchSize,Vector3.forward, out RaycastHit hit, 6))
        {
            
            Debug.Log("Hit");
            if (hit.transform.TryGetComponent(out PlantManager plant))
            {
                plant.isRotating = true;
            }
            else
            {
                objectToDrag = hit.transform;
            }
        }
        //Debug.Log("Pressed at:" + touchWorldPos);
    }
    
    private void TouchPressActionOnCancelled(InputAction.CallbackContext obj)
    {
        if (objectToDrag)
            objectToDrag = null;
        
        previousPos = touchWorldPos;
        touchWorldPos = _playerInput.camera.ScreenToWorldPoint(touchPosAction.ReadValue<Vector2>());
        touchWorldPos.z = 0;
        if (OnTouchRelease != null)
            OnTouchRelease.Invoke();
        //Debug.Log("Released at:" + touchWorldPos);
    }



}
