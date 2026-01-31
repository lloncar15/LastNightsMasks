using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LastNightsMasks.Input {
    public class InputController : MonoBehaviour {
        private PlayerInputActions _inputActions;
    
        public Vector2 MoveInput {get; private set;}
        public Vector2 LookInput {get; private set;}

        public event Action OnInteractPressed;

        private void Awake() {
            _inputActions = new PlayerInputActions();
        }

        private void OnEnable() {
            _inputActions.Enable();
            _inputActions.Player.Interact.performed += HandleInteract;
        }

        private void OnDisable() {
            _inputActions.Player.Interact.performed -= HandleInteract;
            _inputActions.Player.Disable();
        }

        private void Update() {
            MoveInput = _inputActions.Player.Move.ReadValue<Vector2>();
            LookInput = _inputActions.Player.Look.ReadValue<Vector2>();
        }

        private void HandleInteract(InputAction.CallbackContext context) {
            OnInteractPressed?.Invoke();
        }

        /// <summary>
        /// Call this to disable all player input during interactions
        /// </summary>
        public void DisablePlayerInput() {
            _inputActions.Player.Disable();
            MoveInput = Vector2.zero;
            LookInput = Vector2.zero;
        }

        public void EnablePlayerInput() {
            _inputActions.Player.Enable();
        }
    }
}
