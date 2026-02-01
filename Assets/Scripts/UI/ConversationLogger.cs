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
        private void AddToLog(string character, string text) {
            conversationLog.Add(new LogEntry {
                characterName = character,
                lineText = text,
            });
        }

        /// <summary>
        /// Called from yarn with a custom command
        /// </summary>
        [YarnCommand("log")]
        public void LogLine(string charName, string text) {
            Debug.Log("Logged:  " + charName + ": " + text);
            AddToLog(charName, text);
        }
    }
}