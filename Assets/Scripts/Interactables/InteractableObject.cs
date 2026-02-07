using System;
using LastNightsMasks.Items;
using LastNightsMasks.Sound;
using UnityEngine;

namespace LastNightsMasks.Interactable {
    [RequireComponent(typeof(InteractableTrigger))]
    public class InteractableObject : MonoBehaviour, IInteractable {
        [SerializeField] protected Transform lookAtPoint;
        [SerializeField] protected ItemDrop itemDropToActivate;
        [SerializeField] protected AudioClip interactSound;
        [SerializeField] protected float volume = 0.6f;
        
        protected bool HasAlreadyBeenInteracted;
        protected bool IsBeingLookedAt;
        private bool _canBeInteracted;

        public static Action<Transform> InteractedWithObject;
        public static Action FinishedInteractingWithObject;

        public virtual void Interact(Transform interactingTransform) {
            if (!IsBeingLookedAt)
                return;
            
            HasAlreadyBeenInteracted = true;
            IsBeingLookedAt = false;
            
            SoundController.Instance.PlaySound(interactSound, volume);
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