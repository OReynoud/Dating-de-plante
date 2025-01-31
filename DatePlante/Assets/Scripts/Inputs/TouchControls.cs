//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/Inputs/TouchControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @TouchControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @TouchControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TouchControls"",
    ""maps"": [
        {
            ""name"": ""TouchMap"",
            ""id"": ""1386d4e3-0b3b-4569-afcb-20aec6a9ad57"",
            ""actions"": [
                {
                    ""name"": ""Tap"",
                    ""type"": ""Button"",
                    ""id"": ""eb3e9780-63ff-44fb-9ef0-91505dd75baf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TouchPos"",
                    ""type"": ""Value"",
                    ""id"": ""682d9147-e85b-4678-ba42-2969ad7cf7fb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Swipe"",
                    ""type"": ""Button"",
                    ""id"": ""837c44ae-6f3c-40cc-b7ed-812d9b2ed3f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""999305cf-45c1-45a0-9eec-9cb31c82342f"",
                    ""path"": ""<Touchscreen>/Press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f46411df-8c3c-4117-be60-3af3750bf1f9"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3b779c2-60c0-45d9-ac56-fa2511393a60"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swipe"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // TouchMap
        m_TouchMap = asset.FindActionMap("TouchMap", throwIfNotFound: true);
        m_TouchMap_Tap = m_TouchMap.FindAction("Tap", throwIfNotFound: true);
        m_TouchMap_TouchPos = m_TouchMap.FindAction("TouchPos", throwIfNotFound: true);
        m_TouchMap_Swipe = m_TouchMap.FindAction("Swipe", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // TouchMap
    private readonly InputActionMap m_TouchMap;
    private List<ITouchMapActions> m_TouchMapActionsCallbackInterfaces = new List<ITouchMapActions>();
    private readonly InputAction m_TouchMap_Tap;
    private readonly InputAction m_TouchMap_TouchPos;
    private readonly InputAction m_TouchMap_Swipe;
    public struct TouchMapActions
    {
        private @TouchControls m_Wrapper;
        public TouchMapActions(@TouchControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Tap => m_Wrapper.m_TouchMap_Tap;
        public InputAction @TouchPos => m_Wrapper.m_TouchMap_TouchPos;
        public InputAction @Swipe => m_Wrapper.m_TouchMap_Swipe;
        public InputActionMap Get() { return m_Wrapper.m_TouchMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TouchMapActions set) { return set.Get(); }
        public void AddCallbacks(ITouchMapActions instance)
        {
            if (instance == null || m_Wrapper.m_TouchMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TouchMapActionsCallbackInterfaces.Add(instance);
            @Tap.started += instance.OnTap;
            @Tap.performed += instance.OnTap;
            @Tap.canceled += instance.OnTap;
            @TouchPos.started += instance.OnTouchPos;
            @TouchPos.performed += instance.OnTouchPos;
            @TouchPos.canceled += instance.OnTouchPos;
            @Swipe.started += instance.OnSwipe;
            @Swipe.performed += instance.OnSwipe;
            @Swipe.canceled += instance.OnSwipe;
        }

        private void UnregisterCallbacks(ITouchMapActions instance)
        {
            @Tap.started -= instance.OnTap;
            @Tap.performed -= instance.OnTap;
            @Tap.canceled -= instance.OnTap;
            @TouchPos.started -= instance.OnTouchPos;
            @TouchPos.performed -= instance.OnTouchPos;
            @TouchPos.canceled -= instance.OnTouchPos;
            @Swipe.started -= instance.OnSwipe;
            @Swipe.performed -= instance.OnSwipe;
            @Swipe.canceled -= instance.OnSwipe;
        }

        public void RemoveCallbacks(ITouchMapActions instance)
        {
            if (m_Wrapper.m_TouchMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITouchMapActions instance)
        {
            foreach (var item in m_Wrapper.m_TouchMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TouchMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TouchMapActions @TouchMap => new TouchMapActions(this);
    public interface ITouchMapActions
    {
        void OnTap(InputAction.CallbackContext context);
        void OnTouchPos(InputAction.CallbackContext context);
        void OnSwipe(InputAction.CallbackContext context);
    }
}
