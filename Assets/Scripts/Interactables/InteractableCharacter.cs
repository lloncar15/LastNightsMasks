using System;
using System.Threading;
using UnityEngine;
using LastNightsMasks.Input;
using Yarn.Unity;

namespace LastNightsMasks.Interactable {
    public class InteractableCharacter : InteractableObject {
        [SerializeField] private DialogueReference dialogue = new();
        [SerializeField] private DialogueRunner dialogueRunner;

        public void OnValidate() {
            if (dialogueRunner == null) {
                dialogueRunner = FindAnyObjectByType<DialogueRunner>();
            }
            
            if (dialogueRunner != null && dialogueRunner.YarnProject != null && dialogue.project == null) {
                dialogue.project = dialogueRunner.YarnProject;
            }
        }

        public async override void Interact() {
            if (!isBeingLookedAt)
                return;
            
            InputController.Instance.SwitchToInputMode(InputMode.Interact);
            InteractedWithObject?.Invoke(lookAtPoint);

            await StartInteraction();
            
            FinishedInteractingWithObject?.Invoke();
            InputController.Instance.SwitchToInputMode(InputMode.General);
        }

        private async YarnTask StartInteraction() {
            if (dialogue == null)
                return;

            if (dialogueRunner == null) {
                Debug.LogError($"Can't run dialogue {dialogue}: dialogue runner not set");
                return;
            }
            
            if (!dialogue.IsValid || dialogue.nodeName == null) {
                Debug.LogError($"Can't run dialogue {dialogue}: not a valid dialogue reference");
                return;
            }
            
            if (dialogueRunner.IsDialogueRunning) {
                Debug.LogError($"Can't run dialogue {dialogue}: dialogue runner is already running");
                return;
            }

            await dialogueRunner.StartDialogue(dialogue.nodeName);

            CancellationToken destroyCancellation = destroyCancellationToken;

            await dialogueRunner.DialogueTask;
        }

        [YarnCommand("interact")]
        public void YarnInteractEnded() {
            Debug.Log($"{gameObject.name} has finished interaction");
            hasAlreadyBeenInteracted =  true;
            isBeingLookedAt = false;
        }
    }
}