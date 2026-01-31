using UnityEngine;

namespace LastNightsMasks.Interactable {
    public interface IInteractable {
        void Interact();
        
        /// <summary>
        /// Called when the player looks at this object while in range
        /// </summary>
        void OnHoverEnter();
        
        /// <summary>
        /// Called when the player looks away from this object
        /// </summary>
        void OnHoverExit();
        
        /// <summary>
        /// Called when the player enters the interaction radius trigger
        /// </summary>
        void OnRangeEnter();
        
        /// <summary>
        /// Called when the player exits the interaction radius trigger
        /// </summary>
        void OnRangeExit();
        
        bool CanInteract();
        Transform Transform { get; }
    }
}