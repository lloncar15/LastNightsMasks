using System;
using System.Collections;
using LastNightsMasks.Input;
using LastNightsMasks.Items;
using UnityEngine;

namespace LastNightsMasks.Interactable {
    [RequireComponent(typeof(InteractableTrigger))]
    public class InteractableObject : MonoBehaviour, IInteractable {
        [SerializeField] protected Transform lookAtPoint;
        [SerializeField] protected ItemDrop itemDropToActivate;
        protected bool hasAlreadyBeenInteracted;
        protected bool isBeingLookedAt;
        private bool _canBeInteracted;

        public static Action<Transform> InteractedWithObject;
        public static Action FinishedInteractingWithObject;

        public virtual void Interact() {
            if (!isBeingLookedAt)
                return;
            
            hasAlreadyBeenInteracted = true;
            isBeingLookedAt = false;
            
            ItemController.Instance.ItemDropToActivate(itemDropToActivate);
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