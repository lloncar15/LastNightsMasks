using LastNightsMasks.Input;
using LastNightsMasks.Sound;
using UnityEngine;

namespace LastNightsMasks.Player {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Transform cameraHolder;
        [SerializeField] private PlayerConfig playerConfig;
        
        [Header("Footsteps")]
        [SerializeField] private AudioSource footstepsSource;
        [SerializeField] private AudioClip[] footstepsClips;
        [SerializeField] private float stepInterval = 0.4f;
        [SerializeField] private Vector2 pitchRange = new(0.9f, 1.1f);
        [SerializeField] private float volume = 1.1f;
        
        private CharacterController _characterController;
        private float _verticalVelocity;
        private float _cameraPitch;
        private int _currentFootsteps;
        private float _stepTimer;

        private void Awake() {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update() {
            HandleLook();
            
            Vector2 moveInput = InputController.Instance.MoveInput;
            HandleMovement(moveInput);
            HandleFootsteps(moveInput);
        }

        private void HandleLook() {
            Vector2 lookInput = InputController.Instance.LookInput;
            
            transform.Rotate(Vector3.up, lookInput.x * playerConfig.lookSensitivity);
            
            _cameraPitch -= lookInput.y * playerConfig.lookSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -playerConfig.maxLookAngle, playerConfig.maxLookAngle);
            cameraHolder.localEulerAngles = new Vector3(_cameraPitch, 0, 0);
        }

        private void HandleMovement(Vector2 moveInput) {
            Vector3 direction = transform.right * moveInput.x + transform.forward * moveInput.y;

            if (_characterController.isGrounded && _verticalVelocity < 0f) {
                _verticalVelocity = -2f;
            }
            
            _verticalVelocity += playerConfig.gravity * Time.deltaTime;
            direction.y = _verticalVelocity;
            
            _characterController.Move(direction * (playerConfig.moveSpeed * Time.deltaTime));
        }

        private void HandleFootsteps(Vector2 moveInput) {
            bool isMoving = moveInput.sqrMagnitude > 0.01f;
            if (!isMoving) {
                _stepTimer = 0f;
                return;
            }
            
            _stepTimer += Time.deltaTime;
            if (_stepTimer >= stepInterval) {
                _stepTimer = 0f;
                PlayFootstep();
            }
        }

        private void PlayFootstep() {
            footstepsSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
            SoundController.Instance.PlaySound(footstepsSource, footstepsClips[_currentFootsteps], volume);
            _currentFootsteps = (_currentFootsteps +1) % footstepsClips.Length;
        }
    }
}
