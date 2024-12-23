using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class CustomInputManager : MonoBehaviour
    {
        public static CustomInputManager Instance { get; private set; }

        private PlayerInputActions inputActions;

        public delegate void ClickAction(Vector2 position);
        public event ClickAction OnClick;

        public delegate void ReleaseAction(Vector2 position);
        public event ReleaseAction OnRelease;
        
        public delegate void DragAction(Vector2 position);
        public event DragAction OnDrag;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            inputActions = new PlayerInputActions();
            inputActions.Gameplay.Enable();
        
            inputActions.Gameplay.Click.performed += HandleClick;
            inputActions.Gameplay.PointerPosition.performed += HandleDrag;
            inputActions.Gameplay.Release.performed += HandleRelease;
        }

        private void HandleClick(InputAction.CallbackContext context)
        {
            Debug.Log($"Click {context.phase}");
            if (context.phase == InputActionPhase.Performed && OnClick != null)
            {
                Vector2 position = inputActions.Gameplay.PointerPosition.ReadValue<Vector2>();
                OnClick?.Invoke(position);
            }
        }

        private void HandleDrag(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed && OnDrag != null)
            {
                Vector2 position = context.ReadValue<Vector2>();
                OnDrag?.Invoke(position);
            }
        }

        private void HandleRelease(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed && OnRelease != null)
            {
                Vector2 position = inputActions.Gameplay.PointerPosition.ReadValue<Vector2>();
                Debug.Log("Release detected at: " + position);
                OnRelease?.Invoke(position);
            }
        }
        
        private void OnDestroy()
        {
            inputActions.Disable();
            inputActions.Dispose();
        }
    }

}