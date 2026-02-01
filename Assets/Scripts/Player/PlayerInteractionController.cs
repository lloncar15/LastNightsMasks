using System;
using System.Collections.Generic;
using LastNightsMasks.Input;
using LastNightsMasks.Interactable;
using NUnit.Framework;
using UnityEngine;

namespace LastNightsMasks.Player {
    public class PlayerInteractionController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private GameObject interactPromptUI;
        [SerializeField] private CameraZoomController cameraZoomController;
        [SerializeField] private PlayerInteractionController interactionController;

        [Header("Settings")] 
        [SerializeField] private float maxLookDistance = 10f;
        [SerializeField] private LayerMask interactableLayer;
        
        [Header("Debug")]
        [SerializeField] private bool showDebugRay = true;
        [SerializeField] private Color debugRayColorHit = Color.green;
        [SerializeField] private Color debugRayColorMiss = Color.red;

        private HashSet<IInteractable> _nearbyInteractables = new();
        private IInteractable _currentTarget;
        [SerializeField] private List<GameObject> debugList = new();
        
        private void OnEnable() {
            InputController.Instance.OnInteractPressed += TryToInteract;
            InputController.Instance.OnInteraction += OnInteraction;
            InteractableTrigger.OnPlayerEntered += RegisterNearbyInteractable;
            InteractableTrigger.OnPlayerExited += UnregisterNearbyInteractable;
        }

        private void OnDisable() {
            InputController.Instance.OnInteractPressed -= TryToInteract;
            InputController.Instance.OnInteraction -= OnInteraction;
            InteractableTrigger.OnPlayerEntered -= RegisterNearbyInteractable;
            InteractableTrigger.OnPlayerExited -= UnregisterNearbyInteractable;
        }

        private void Awake() {
            
        }

        private void Start() {
            interactPromptUI.SetActive(false);
        }

        private void Update() {
            CheckForInteractable();
        }

        private void CheckForInteractable() {
            if (_nearbyInteractables.Count == 0) {
                ClearCurrentTarget();
                if (showDebugRay) {
                    Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * maxLookDistance, debugRayColorMiss);
                }
                return;
            }
            
            Ray ray = new(playerCamera.transform.position, playerCamera.transform.forward);

            if (!Physics.Raycast(ray, out RaycastHit hit, maxLookDistance, interactableLayer)) {
                ClearCurrentTarget();
                if (showDebugRay) {
                    Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * maxLookDistance, debugRayColorMiss);
                }
                return;
            }
            
            if (showDebugRay) {
                Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * hit.distance, debugRayColorHit);
            }
            
            
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            if (interactable == null) {
                interactable = hit.collider.gameObject.GetComponentInParent<IInteractable>();
            }

            if (interactable == null || !_nearbyInteractables.Contains(interactable) || !interactable.CanInteract()) {
                ClearCurrentTarget();
                return;
            }

            if (interactable != _currentTarget) {
                _currentTarget?.OnHoverExit();
                _currentTarget = interactable;
                _currentTarget.OnHoverEnter();
            }

            interactPromptUI.SetActive(true);
        }

        public void RegisterNearbyInteractable(IInteractable interactable) {
            if (interactable == null)
                return;
            
            _nearbyInteractables.Add(interactable);
            debugList.Add(interactable.Transform.gameObject);
        }

        public void UnregisterNearbyInteractable(IInteractable interactable) {
            if (interactable == null)
                return;
            
            _nearbyInteractables.Remove(interactable);
            debugList.Remove(interactable.Transform.gameObject);

            ClearInteractableAsCurrentTarget(interactable);
        }

        private void OnInteraction() {
            InputController.Instance.SwitchToInputMode(InputMode.General);
        }

        private void TryToInteract() {
            _currentTarget?.Interact();
        }

        #region Helpers

        private void ClearInteractableAsCurrentTarget(IInteractable interactable) {
            if (interactable == null)
                return;

            if (interactable != _currentTarget)
                return;

            ClearCurrentTarget();
        }

        private void ClearCurrentTarget() {
            if (_currentTarget != null) {
                _currentTarget.OnHoverExit();
                _currentTarget = null;
            }
            
            interactPromptUI.SetActive(false);
        }
        
        public bool HasTarget => _currentTarget != null;
        public IInteractable CurrentTarget => _currentTarget;

        #endregion
    }
}