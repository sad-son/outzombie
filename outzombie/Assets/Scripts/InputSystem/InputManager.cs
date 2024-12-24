namespace Systems
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class InputManager : IInputSystemInstance
    {
        private readonly PlayerInputActions _inputActions;

        public delegate void ClickAction(Vector2 position);
        public event ClickAction onClick;

        public delegate void ReleaseAction(Vector2 position);
        public event ReleaseAction onRelease;
        
        public delegate void DragAction(Vector2 position);
        public event DragAction onDrag;

        public InputManager()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Gameplay.Enable();
        
            _inputActions.Gameplay.Click.performed += HandleClick;
            _inputActions.Gameplay.PointerPosition.performed += HandleDrag;
            _inputActions.Gameplay.Release.performed += HandleRelease;
        }
        
        public void Dispose()
        {
            _inputActions.Disable();
            _inputActions.Dispose();
        }
        
        private void HandleClick(InputAction.CallbackContext context)
        {
            Debug.Log($"Click {context.phase}");
            if (context.phase == InputActionPhase.Performed && onClick != null)
            {
                Vector2 position = _inputActions.Gameplay.PointerPosition.ReadValue<Vector2>();
                onClick?.Invoke(position);
            }
        }

        private void HandleDrag(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed && onDrag != null)
            {
                Vector2 position = context.ReadValue<Vector2>();
                onDrag?.Invoke(position);
            }
        }

        private void HandleRelease(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed && onRelease != null)
            {
                Vector2 position = _inputActions.Gameplay.PointerPosition.ReadValue<Vector2>();
                Debug.Log("Release detected at: " + position);
                onRelease?.Invoke(position);
            }
        }
    }

}