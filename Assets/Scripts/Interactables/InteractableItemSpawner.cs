using System;
using DG.Tweening;
using LastNightsMasks.Input;
using LastNightsMasks.Items;
using UnityEngine;
using Yarn.Unity;

namespace LastNightsMasks.Interactable {
    public class InteractableItemSpawner : InteractableObject {
        [Header("Variable Storage")]
        [SerializeField] private InMemoryVariableStorage variableStorage;
        [SerializeField] private string unlockVariableName;
        [SerializeField] private float lookAtDropItemDuration = 0.4f;

        public static Action<Vector3, float, Ease> LookAtItem;

        public override void Interact(Transform interactingTransform) {
            base.Interact(interactingTransform);
            
            ItemController.Instance.ItemDropToActivate(itemDropToActivate);
            LookAtDropItem(interactingTransform);
        }

        private void LookAtDropItem(Transform interactingTransform) {
            InputController.Instance.SwitchToInputMode(InputMode.Interact, false);
            
            LookAtItem?.Invoke(itemDropToActivate.transform.position, lookAtDropItemDuration, Ease.InOutSine);
            
            Vector3 lookDirection = itemDropToActivate.transform.position - interactingTransform.position;
            lookDirection.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

            interactingTransform
                .DORotateQuaternion(targetRotation, lookAtDropItemDuration)
                .SetEase(Ease.InOutSine)
                .SetLink(interactingTransform.gameObject)
                .OnComplete(() => {
                    InputController.Instance.SwitchToInputMode(InputMode.General, false);
                });
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