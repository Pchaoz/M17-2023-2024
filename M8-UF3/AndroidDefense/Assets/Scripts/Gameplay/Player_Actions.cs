//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Scripts/Gameplay/Player_Actions.inputactions
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

public partial class @Player_Actions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Player_Actions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player_Actions"",
    ""maps"": [
        {
            ""name"": ""Android"",
            ""id"": ""991d3e24-9c25-46fc-a72c-01174ba5e2b9"",
            ""actions"": [
                {
                    ""name"": ""TapPosition"",
                    ""type"": ""Value"",
                    ""id"": ""6d5ff712-1814-4e74-ae1f-9c2af5463774"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Tap"",
                    ""type"": ""Button"",
                    ""id"": ""e545960d-9c6a-4b40-a02c-0f2dea5621e1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ec1f8d36-c57b-4a6e-ad43-b92279f6ee0d"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TapPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6aff3213-cb01-4a6b-b5d9-60f37902ae3f"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TapPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""71b8bc5a-6e7b-4fe8-a7f5-b1933f823cd1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9de943ac-11b4-4191-a701-a2f92266b32e"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Android
        m_Android = asset.FindActionMap("Android", throwIfNotFound: true);
        m_Android_TapPosition = m_Android.FindAction("TapPosition", throwIfNotFound: true);
        m_Android_Tap = m_Android.FindAction("Tap", throwIfNotFound: true);
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

    // Android
    private readonly InputActionMap m_Android;
    private List<IAndroidActions> m_AndroidActionsCallbackInterfaces = new List<IAndroidActions>();
    private readonly InputAction m_Android_TapPosition;
    private readonly InputAction m_Android_Tap;
    public struct AndroidActions
    {
        private @Player_Actions m_Wrapper;
        public AndroidActions(@Player_Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @TapPosition => m_Wrapper.m_Android_TapPosition;
        public InputAction @Tap => m_Wrapper.m_Android_Tap;
        public InputActionMap Get() { return m_Wrapper.m_Android; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AndroidActions set) { return set.Get(); }
        public void AddCallbacks(IAndroidActions instance)
        {
            if (instance == null || m_Wrapper.m_AndroidActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_AndroidActionsCallbackInterfaces.Add(instance);
            @TapPosition.started += instance.OnTapPosition;
            @TapPosition.performed += instance.OnTapPosition;
            @TapPosition.canceled += instance.OnTapPosition;
            @Tap.started += instance.OnTap;
            @Tap.performed += instance.OnTap;
            @Tap.canceled += instance.OnTap;
        }

        private void UnregisterCallbacks(IAndroidActions instance)
        {
            @TapPosition.started -= instance.OnTapPosition;
            @TapPosition.performed -= instance.OnTapPosition;
            @TapPosition.canceled -= instance.OnTapPosition;
            @Tap.started -= instance.OnTap;
            @Tap.performed -= instance.OnTap;
            @Tap.canceled -= instance.OnTap;
        }

        public void RemoveCallbacks(IAndroidActions instance)
        {
            if (m_Wrapper.m_AndroidActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IAndroidActions instance)
        {
            foreach (var item in m_Wrapper.m_AndroidActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_AndroidActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public AndroidActions @Android => new AndroidActions(this);
    public interface IAndroidActions
    {
        void OnTapPosition(InputAction.CallbackContext context);
        void OnTap(InputAction.CallbackContext context);
    }
}
