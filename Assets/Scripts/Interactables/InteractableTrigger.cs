using System;
using UnityEngine;

namespace LastNightsMasks.Interactable {
    [RequireComponent(typeof(BoxCollider))]
    public class InteractableTrigger : MonoBehaviour {
        private BoxCollider _triggerCollider;
        private IInteractable _interactable;

        public static event Action<IInteractable> OnPlayerEntered;
        public static event Action<IInteractable> OnPlayerExited;

        private void Awake() {
            _triggerCollider = GetComponent<BoxCollider>();
            _triggerCollider.isTrigger = true;
            
            _interactable = GetComponentInParent<IInteractable>();
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player"))
                return;
            
            OnPlayerEntered?.Invoke(_interactable);
            _interactable.OnRangeEnter();
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player"))
                return;
            
            OnPlayerExited?.Invoke(_interactable);
            _interactable.OnRangeExit();
        }
    }
}