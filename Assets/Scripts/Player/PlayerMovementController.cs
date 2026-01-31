using System;
using LastNightsMasks.Input;
using UnityEngine;

namespace LastNightsMasks.Player {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private InputController inputController;
        [SerializeField] private Transform cameraHolder;
        [SerializeField] private PlayerConfig playerConfig;
        
        private CharacterController _characterController;
        private float _verticalVelocity;
        private float _cameraPitch;

        private void Awake() {
            _characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update() {
            HandleLook();
            HandleMovement();
        }

        private void HandleLook() {
            Vector2 lookInput = inputController.LookInput;
            
            transform.Rotate(Vector3.up, lookInput.x * playerConfig.lookSensitivity);
            
            _cameraPitch -= lookInput.y * playerConfig.lookSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -playerConfig.maxLookAngle, playerConfig.maxLookAngle);
            cameraHolder.localEulerAngles = new Vector3(_cameraPitch, 0, 0);
        }

        private void HandleMovement() {
            Vector2 moveInput = inputController.MoveInput;
            
            Vector3 direction = transform.right * moveInput.x + transform.forward * moveInput.y;

            if (_characterController.isGrounded && _verticalVelocity < 0f) {
                _verticalVelocity = -2f;
            }
            
            _verticalVelocity += playerConfig.gravity * Time.deltaTime;
            direction.y = _verticalVelocity;
            
            _characterController.Move(direction * (playerConfig.moveSpeed * Time.deltaTime));
        }
    }
}
