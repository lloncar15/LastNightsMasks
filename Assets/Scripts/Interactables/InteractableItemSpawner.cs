using LastNightsMasks.Items;
using UnityEngine;
using Yarn.Unity;

namespace LastNightsMasks.Interactable {
    public class InteractableItemSpawner : InteractableObject {
        [Header("Variable Storage")]
        [SerializeField] private InMemoryVariableStorage variableStorage;
        [SerializeField] private string unlockVariableName;

        public override void Interact() {
            base.Interact();
            
            ItemController.Instance.ItemDropToActivate(itemDropToActivate);
        }
        
        public override bool CanInteract() {
            return base.CanInteract() && HasBeenUnlocked();
        }
        
        private bool HasBeenUnlocked() {
            if (!variableStorage)
                return false;

            variableStorage.TryGetValue(unlockVariableName, out bool isUnlocked);
            
            return isUnlocked;
        }
    }
}