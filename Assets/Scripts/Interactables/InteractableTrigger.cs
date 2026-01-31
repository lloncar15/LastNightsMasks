using System;
using UnityEngine;

namespace LastNightsMasks.Interactable {
    [RequireComponent(typeof(SphereCollider))]
    public class InteractableTrigger : MonoBehaviour {
        [SerializeField] private float interactionRadius = 3f;
        private SphereCollider _triggerCollider;
        private IInteractable _interactable;

        public static event Action<IInteractable> OnPlayerEntered;
        public static event Action<IInteractable> OnPlayerExited;

        private void Awake() {
            _triggerCollider = GetComponent<SphereCollider>();
            _triggerCollider.radius =  interactionRadius;
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

        public void SetInteractionRadius(float radius) {
            interactionRadius = radius;
            _triggerCollider.radius = interactionRadius;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = new Color(0f, 1f,  0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
    }
}