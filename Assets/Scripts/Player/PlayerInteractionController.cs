using LastNightsMasks.Input;
using UnityEngine;

namespace LastNightsMasks.Player {
    public class PlayerInteractionController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private InputController inputController;
        
        private void OnEnable() {
            inputController.OnInteractPressed += OnInteractPressed;
            inputController.OnInteraction += OnInteraction;
        }

        private void OnDisable() {
            inputController.OnInteractPressed += OnInteractPressed;
            inputController.OnInteraction -= OnInteraction;
        }

        private void OnInteraction() {
            inputController.SwitchToInputMode(InputMode.General);
        }

        private void OnInteractPressed() {
            inputController.SwitchToInputMode(InputMode.Interact);
        }
    }
}