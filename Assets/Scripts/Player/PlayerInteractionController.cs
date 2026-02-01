using System;
using System.Collections.Generic;
using LastNightsMasks.Input;
using LastNightsMasks.Interactable;
using LastNightsMasks.Items;
using LastNightsMasks.UI;
using UnityEngine;
using Yarn.Unity;

namespace LastNightsMasks.Player {
    public class PlayerInteractionController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private GameObject interactPromptUI;
        [SerializeField] private DialogueRunner  dialogueRunner;
        [SerializeField] private DialogueReference dialogue;

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

        private bool _isInteracting;
        
        private void OnEnable() {
            InputController.Instance.OnInteractPressed += TryToInteract;
            InputController.Instance.OnInteraction += OnInteraction;
            InteractableTrigger.OnPlayerEntered += RegisterNearbyInteractable;
            InteractableTrigger.OnPlayerExited += UnregisterNearbyInteractable;
            InteractableObject.InteractedWithObject += OnBeginInteraction;
            InteractableObject.FinishedInteractingWithObject += OnFinishInteraction;
            ItemController.OnAllItemsCollected += OnAllItemsCollected;
        }

        private void OnDisable() {
            InputController.Instance.OnInteractPressed -= TryToInteract;
            InputController.Instance.OnInteraction -= OnInteraction;
            InteractableTrigger.OnPlayerEntered -= RegisterNearbyInteractable;
            InteractableTrigger.OnPlayerExited -= UnregisterNearbyInteractable;
            InteractableObject.InteractedWithObject -= OnBeginInteraction;
            InteractableObject.FinishedInteractingWithObject -= OnFinishInteraction;
            ItemController.OnAllItemsCollected -= OnAllItemsCollected;
        }

        private async void OnAllItemsCollected() {
            if (dialogue == null) {
                return;
            }
            
            
            if (!dialogue.IsValid || dialogue.nodeName == null) {
                Debug.LogError($"Can't run dialogue {dialogue}: not a valid dialogue reference");
                return;
            }
            
            InputController.Instance.SwitchToInputMode(InputMode.Interact);
            await dialogueRunner.StartDialogue(dialogue.nodeName);
            await dialogueRunner.DialogueTask;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            SceneTransition.Instance.LoadScene("MainMenuScene");
        }

        private void OnBeginInteraction(Transform _) {
            ClearCurrentTarget();
            _isInteracting = true;
        }

        private void OnFinishInteraction() {
            _isInteracting = false;
        }

        private void Start() {
            SetInteractPromptUI(false);
        }

        private void Update() {
            if (!_isInteracting) {
                CheckForInteractable();
            }
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

            SetInteractPromptUI(true);
        }

        private void SetInteractPromptUI(bool active) {
            interactPromptUI.SetActive(active);
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
                SetInteractPromptUI(false);
            }
        }
        
        public bool HasTarget => _currentTarget != null;
        public IInteractable CurrentTarget => _currentTarget;

        #endregion
    }
}