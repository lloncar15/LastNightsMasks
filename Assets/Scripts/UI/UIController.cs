using System;
using LastNightsMasks.Input;
using UnityEngine;

namespace LastNightsMasks.UI {
    public class UIController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private InputController inputController;

        private void OnEnable() {
            inputController.OnSettingsPressed += OnSettingsPressed;
        }

        private void OnDisable() {
            inputController.OnSettingsPressed -= OnSettingsPressed;
        }

        private void OnSettingsPressed() {
#if UNITY_EDITOR 
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}