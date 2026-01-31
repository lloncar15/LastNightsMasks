using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastNightsMasks.UI {
    public class MainMenuController : MonoBehaviour {
        public void PlayButtonPressed() {
            SceneTransition.Instance.LoadScene("World");
        }

        public void CreditsButtonPressed() {
            
        }

        public void SettingsButtonPressed() {
            
        }

        public void QuitButtonPressed() {
#if UNITY_EDITOR 
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}