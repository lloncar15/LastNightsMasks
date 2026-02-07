using System;
using LastNightsMasks.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LastNightsMasks.Input {
    public class InputController : PersistentSingleton<InputController> {
        [SerializeField] public InputMode currentInputMode = InputMode.General;
        
        private PlayerInputActions _inputActions;
    
        public Vector2 MoveInput {get; private set;}
        public Vector2 LookInput {get; private set;}

        public static event Action OnInteractPressed;
        public static event Action OnInteraction;
        public static event Action OnSettingsPressed;
        public static event Action OnRestartPressed;

        private bool _isUIInputEnabled = true;

        protected override void Awake() {
            base.Awake();
            
            _inputActions = new PlayerInputActions();
        }

        private void Start() {
            SwitchToInputMode(InputMode.Interact);
        }

        private void OnEnable() {
            _inputActions.UI.Enable();
            _inputActions.General.Interact.performed += HandleGeneralInteract;
            _inputActions.Interact.Interact.performed += HandleInteracted;
            _inputActions.UI.Settings.performed += HandleSettings;
            _inputActions.UI.Restart.performed += HandleRestart;
        }

        private void OnDisable() {
            _inputActions.General.Interact.performed -= HandleGeneralInteract;
            _inputActions.Interact.Interact.performed -= HandleInteracted;
            _inputActions.UI.Settings.performed -= HandleSettings;
            _inputActions.UI.Restart.performed -= HandleRestart;
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
        private void HandleGeneralInteract(InputAction.CallbackContext context) {
            if (currentInputMode != InputMode.General)
                return;
            
            OnInteractPressed?.Invoke();
        }
        
        public void DisableGeneralInput() {
            _inputActions.General.Disable();
            MoveInput = Vector2.zero;
            LookInput = Vector2.zero;
        }

        public void EnableGeneralInput() {
            _inputActions.General.Enable();
        }

        #endregion
        
        #region UI Inputs
        
        private void HandleSettings(InputAction.CallbackContext context) {
            if (!_isUIInputEnabled)
                return;
            
            OnSettingsPressed?.Invoke();
        }

        private void HandleRestart(InputAction.CallbackContext context) {
            if (!_isUIInputEnabled)
                return;
            
            SwitchCursorLockMode(CursorLockMode.None);
            OnRestartPressed?.Invoke();
        }

        public void EnableUIInputs() {
            _isUIInputEnabled = true;
            _inputActions.UI.Enable();
        }

        public void DisableUIInputs() {
            _isUIInputEnabled = false;
            _inputActions.UI.Disable();
        }
        
        #endregion

        #region Interact Inputs

        private void HandleInteractInputs() {
            
        }

        private void HandleInteracted(InputAction.CallbackContext context) {
            if (currentInputMode != InputMode.Interact)
                return;
            
            OnInteraction?.Invoke();
        }

        public void EnableInteractInputs() {
            _inputActions.Interact.Enable();
        }

        public void DisableInteractInputs() {
            _inputActions.Interact.Disable();
        }

        #endregion
        
        public void SwitchToInputMode(InputMode mode) {
            switch (mode) {
                case InputMode.General: {
                    EnableGeneralInput();
                    DisableInteractInputs();
                    SwitchCursorLockMode(CursorLockMode.Locked);
                    break;
                }
                case InputMode.Interact: {
                    DisableGeneralInput();
                    EnableInteractInputs();
                    SwitchCursorLockMode(CursorLockMode.None);
                    break;
                }
            }

            currentInputMode = mode;
        }

        public void SwitchCursorLockMode(CursorLockMode mode) {
            Cursor.lockState = mode;
            Cursor.visible = mode != CursorLockMode.Locked;
        }
    }

    public enum InputMode {
        General,
        Interact
    }
}
