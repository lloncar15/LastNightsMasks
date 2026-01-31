using TMPro;
using UnityEngine;

namespace LastNightsMasks.UI {
    public class ConversationLogItem : MonoBehaviour {
        [Header("Text Components")]
        public TextMeshProUGUI characterNameText;
        public TextMeshProUGUI dialogueText;

        public void SetContent(string characterName, string dialogue) {
            if (characterNameText != null) {
                characterNameText.text = characterName;
            }

            if (dialogueText != null) {
                dialogueText.text = dialogue;
            }
        }
    }
}