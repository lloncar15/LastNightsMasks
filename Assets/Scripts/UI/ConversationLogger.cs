using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace LastNightsMasks.UI {
    public class ConversationLogger : MonoBehaviour {
        [SerializeField] public DialogueRunner dialogueRunner;
        [SerializeField] public List<LogEntry> conversationLog = new();

        [System.Serializable]
        public class LogEntry {
            public string characterName;
            public string lineText;
            public string nodeTitle;
        }

        private void Start() {
            if (dialogueRunner != null) {
                dialogueRunner.onDialogueStart?.AddListener(OnDialogueStart);
            }
        }

        private void OnDestroy() {
            if (dialogueRunner != null) {
                dialogueRunner.onDialogueStart?.RemoveListener(OnDialogueStart);
            }
        }

        private void OnDialogueStart() {
            // mark new convesation
        }

        /// <summary>
        /// Adds the character, text and current node name to the Log
        /// </summary>
        public void AddToLog(string character, string text) {
            conversationLog.Add(new LogEntry {
                characterName = character,
                lineText = text,
                nodeTitle = dialogueRunner.Dialogue.CurrentNode
            });
        }

        /// <summary>
        /// Called from yar with a custom command
        /// </summary>
        void LogLine(string[] parameters) {
            if (parameters.Length != 2) {
                // Called from yarn with <<logline CharacterName LineText>>
                AddToLog(parameters[0], parameters[1]);
            }
        }
    }
}