using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LastNightsMasks.UI {
    public class ConversationLogUI : MonoBehaviour {
        [Header("References")]
        public ConversationLogger logger;
        public GameObject logPanel;
        public Transform logContentParent; // The Content object inside ScrollView
        public GameObject logEntryPrefab;
        public Yarn.Unity.DialogueRunner dialogueRunner;

        [Header("Scroll Settings")]
        public ScrollRect scrollRect;
        public bool autoScrollToBottom = true;

        private int _lastLogCount = 0;

        void Start() {
            // Make sure panel is closed at start
            logPanel.SetActive(false);
            
            // Subscribe to dialogue events to control when log can be opened
            if (dialogueRunner != null) {
                dialogueRunner.onDialogueComplete?.AddListener(OnDialogueComplete);
            }
        }

        void Update() {
            // Check if new entries have been added
            if (logger.conversationLog.Count > _lastLogCount) {
                AddNewEntries();
                _lastLogCount = logger.conversationLog.Count;
            }
        }

        void AddNewEntries() {
            // Add only the new entries since last check
            for (int i = _lastLogCount; i < logger.conversationLog.Count; i++) {
                CreateLogEntry(logger.conversationLog[i]);
            }

            // Auto-scroll to bottom if enabled and panel is open
            if (autoScrollToBottom && logPanel.activeSelf) {
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }

        void CreateLogEntry(ConversationLogger.LogEntry entry) {
            GameObject newEntry = Instantiate(logEntryPrefab, logContentParent);
            ConversationLogItem logItem = newEntry.GetComponent<ConversationLogItem>();
            
            if (logItem != null) {
                logItem.SetContent(entry.characterName, entry.lineText);
            }
        }

        public void OpenLog() {
            // Only open if dialogue is not running
            if (dialogueRunner != null && dialogueRunner.IsDialogueRunning) {
                Debug.Log("Cannot open log while dialogue is running");
                return;
            }

            logPanel.SetActive(true);
            
            // Scroll to bottom when opening
            if (autoScrollToBottom) {
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }

        public void CloseLog() {
            logPanel.SetActive(false);
        }

        void OnDialogueComplete() {
            // Optional: You could auto-open the log here if desired
            // OpenLog();
        }

        void OnDestroy() {
            if (dialogueRunner != null) {
                dialogueRunner.onDialogueComplete?.RemoveListener(OnDialogueComplete);
            }
        }
    }
}