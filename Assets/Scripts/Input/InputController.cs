using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LastNightsMasks.Input {
    public class InputController : MonoBehaviour {
        [SerializeField] public InputMode currentInputMode = InputMode.General;
        
        private PlayerInputActions _inputActions;
    
        public Vector2 MoveInput {get; private set;}
        public Vector2 LookInput {get; private set;}

        public event Action OnInteractPressed;
        public event Action OnSettingsPressed;

        private void Awake() {
            _inputActions = new PlayerInputActions();
        }

        private void OnEnable() {
            _inputActions.General.Enable();
            _inputActions.UI.Enable();
            _inputActions.General.Interact.performed += HandleGeneralInteract;
            _inputActions.UI.Settings.performed += HandleSettings;
        }

        private void OnDisable() {
            _inputActions.General.Interact.performed -= HandleGeneralInteract;
            _inputActions.UI.Settings.performed += HandleSettings;
            _inputActions.Disable();
        }

        private void Update() {
            switch (currentInputMode) {
                case InputMode.General: {
                    HandleMoveInputs();
                    break;
                }
                case InputMode.Interact: {
                    HandleInteractInputs();
                    break;
                }
            }
            
        }

        #region General Inputs

        private void HandleMoveInputs() {
            MoveInput = _inputActions.General.Move.ReadValue<Vector2>();
            LookInput = _inputActions.General.Look.ReadValue<Vector2>();
        }
        /// <summary>
        /// Fire of the Interact pressed event if the General input mode is enabled
        /// </summary>
        /// <param name="context"></param>
        private void HandleGeneralInteract(InputAction.CallbackContext context) {
            if (currentInputMode != InputMode.General)
                return;
            
            OnInteractPressed?.Invoke();
        }
        
        private void HandleSettings(InputAction.CallbackContext context) {
            OnSettingsPressed?.Invoke();
        }

        #endregion

        #region Interact Inputs

        private void HandleInteractInputs() {
            
        }

        #endregion
        
        public void SwitchInputMode(InputMode mode) {
            if (currentInputMode == InputMode.General) {
                
            }
            else {
                
            }
        }

        /// <summary>
        /// Call this to disable all player input during interactions
        /// </summary>
        public void DisablePlayerInput() {
            _inputActions.General.Disable();
            MoveInput = Vector2.zero;
            LookInput = Vector2.zero;
        }

        public void EnablePlayerInput() {
            _inputActions.General.Enable();
        }
    }

    public enum InputMode {
        General,
        Interact
    }
}
