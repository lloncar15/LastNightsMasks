using System;
using System.Collections;
using LastNightsMasks.Input;
using UnityEngine;

namespace LastNightsMasks.Interactable {
    [RequireComponent(typeof(InteractableTrigger))]
    public class InteractableObject : MonoBehaviour, IInteractable {
        [SerializeField] protected Transform lookAtPoint;
        protected bool hasAlreadyBeenInteracted;
        protected bool isBeingLookedAt;
        private bool _canBeInteracted;

        public static Action<Transform> InteractedWithObject;
        public static Action FinishedInteractingWithObject;

        public virtual void Interact() {
            if (!isBeingLookedAt)
                return;
            
            InputController.Instance.SwitchToInputMode(InputMode.Interact);
            
            InteractedWithObject?.Invoke(lookAtPoint);

            StartCoroutine(Test());
        }

        private IEnumerator Test() {
            hasAlreadyBeenInteracted = true;
            isBeingLookedAt = false;
            yield return new WaitForSeconds(3f);
            FinishedInteractingWithObject?.Invoke();
            InputController.Instance.SwitchToInputMode(InputMode.General);
        }
        
        public void OnHoverEnter() {
            if (!CanInteract())
                return;

            isBeingLookedAt = true;
        }
        public void OnHoverExit() {
            if (!CanInteract())
                return;

            isBeingLookedAt = false;
        }
        public void OnRangeEnter() {
            if (hasAlreadyBeenInteracted)
                return;
            
            _canBeInteracted = true;
        }
        public void OnRangeExit() {
            _canBeInteracted = false;
        }

        public Transform Transform => transform;

        public bool CanInteract() {
            return _canBeInteracted && !hasAlreadyBeenInteracted;
        }
    }
}