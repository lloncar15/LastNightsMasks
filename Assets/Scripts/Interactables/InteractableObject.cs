using System.Collections;
using LastNightsMasks.Input;
using UnityEngine;

namespace LastNightsMasks.Interactable {
    [RequireComponent(typeof(InteractableTrigger))]
    public class InteractableObject : MonoBehaviour, IInteractable {
        private bool _hasAlreadyBeenInteracted;
        private bool _isBeingLookedAt;
        private bool _canBeInteracted;

        public void Interact() {
            if (!_isBeingLookedAt)
                return;
            
            InputController.Instance.SwitchToInputMode(InputMode.Interact);

            StartCoroutine(Test());
        }

        private IEnumerator Test() {
            _hasAlreadyBeenInteracted = true;
            _isBeingLookedAt = false;
            yield return new WaitForSeconds(3f);
            InputController.Instance.SwitchToInputMode(InputMode.General);
        }
        
        public void OnHoverEnter() {
            if (!CanInteract())
                return;

            _isBeingLookedAt = true;
        }
        public void OnHoverExit() {
            if (!CanInteract())
                return;

            _isBeingLookedAt = false;
        }
        public void OnRangeEnter() {
            if (_hasAlreadyBeenInteracted)
                return;
            
            _canBeInteracted = true;
        }
        public void OnRangeExit() {
            _canBeInteracted = false;
        }

        public Transform Transform => transform;

        public bool CanInteract() {
            return _canBeInteracted && !_hasAlreadyBeenInteracted;
        }
    }
}