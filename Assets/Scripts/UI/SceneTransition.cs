using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace LastNightsMasks.UI {
    public class SceneTransition : MonoBehaviour {
        public static SceneTransition _instance;
        
        public static SceneTransition Instance {
            get {
                if (_instance == null) {
                    _instance = FindAnyObjectByType<SceneTransition>();
                    if (_instance == null) {
                        GameObject go = new GameObject("InputController");
                        _instance = go.AddComponent<SceneTransition>();
                    }
                }

                return _instance;
            }
        }
    
        [SerializeField] private Image blackCircle;
        [SerializeField] private float transitionDuration = 0.8f;
    
        private void Awake()
        {
            if (!Application.isPlaying)
                return;

            _instance = this;
            
            DontDestroyOnLoad(blackCircle.gameObject);
            DontDestroyOnLoad(blackCircle.transform.parent.gameObject);
        }

        private void Start()
        {
            // Start with circle closed (showing the scene)
            blackCircle.transform.localScale = Vector3.zero;
        }

        public void LoadScene(string sceneName)
        {
            blackCircle.gameObject.SetActive(true);
            // Expand circle to cover screen
            blackCircle.transform.DOScale(Vector3.one * 10f, transitionDuration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    // Load the new scene
                    SceneManager.LoadScene(sceneName);
                
                    // After scene loads, close the circle
                    CloseTransition();
                });
        }

        private void CloseTransition()
        {
            // Shrink circle back to center
            blackCircle.transform.DOScale(Vector3.zero, transitionDuration)
                .SetEase(Ease.InOutQuad);
        }
    }
}