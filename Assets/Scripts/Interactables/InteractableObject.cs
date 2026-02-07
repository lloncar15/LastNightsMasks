using System;
using LastNightsMasks.Items;
using UnityEngine;

namespace LastNightsMasks.Interactable {
    [RequireComponent(typeof(InteractableTrigger))]
    public class InteractableObject : MonoBehaviour, IInteractable {
        [SerializeField] protected Transform lookAtPoint;
        [SerializeField] protected ItemDrop itemDropToActivate;
        
        protected bool HasAlreadyBeenInteracted;
        protected bool IsBeingLookedAt;
        private bool _canBeInteracted;

        public static Action<Transform> InteractedWithObject;
        public static Action FinishedInteractingWithObject;

        public virtual void Interact() {
            if (!IsBeingLookedAt)
                return;
            
            HasAlreadyBeenInteracted = true;
            IsBeingLookedAt = false;
        }
        
        public void OnHoverEnter() {
            if (!CanInteract())
                return;

            IsBeingLookedAt = true;
        }
        public void OnHoverExit() {
            if (!CanInteract())
                return;

            IsBeingLookedAt = false;
        }
        public void OnRangeEnter() {
            if (HasAlreadyBeenInteracted)
                return;
            
            _canBeInteracted = true;
        }
        public void OnRangeExit() {
            _canBeInteracted = false;
        }

        public Transform Transform => transform;

        public virtual bool CanInteract() {
            return _canBeInteracted && !HasAlreadyBeenInteracted;
        }
    }
}