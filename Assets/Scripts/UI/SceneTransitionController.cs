using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using LastNightsMasks.Utils;

namespace LastNightsMasks.UI {
    public class SceneTransitionController : PersistentSingleton<SceneTransitionController> {
        [SerializeField] private Image blackCircle;
        [SerializeField] private float transitionDuration = 0.8f;

        private void Start() {
            blackCircle.transform.localScale = Vector3.zero;
        }

        public void LoadScene(string sceneName) {
            blackCircle.gameObject.SetActive(true);
            blackCircle.transform.DOScale(Vector3.one * 10f, transitionDuration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => {
                    SceneManager.LoadScene(sceneName);
                    CloseTransition();
                });
        }

        private void CloseTransition() {
            blackCircle.transform.DOScale(Vector3.zero, transitionDuration)
                .SetEase(Ease.InOutQuad);
        }
    }
}