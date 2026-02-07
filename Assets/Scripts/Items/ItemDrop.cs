using DG.Tweening;
using LastNightsMasks.Sound;
using UnityEngine;

namespace LastNightsMasks.Items {
    public class ItemDrop : MonoBehaviour {
        [SerializeField] private ItemData itemData;
        [SerializeField] private float bounceHeight = 0.3f;
        [SerializeField] private float bounceDuration = 1f;
        [SerializeField] private AudioClip itemUnlocked;
        [SerializeField] private AudioClip itemPickup;
        
        private Vector3 _startPosition;
        private Tween _bounceTween;
        private bool _isCollected;
        
        public void Appear() {
            _startPosition = transform.position;
            gameObject.SetActive(true);
            StartBouncing();
        }
        
        public void Start() {
            StartBouncing();
        }

        private void StartBouncing() {
            SoundController.Instance.PlaySound(itemUnlocked);
            
            _bounceTween = transform.DOMoveY(_startPosition.y + bounceHeight, bounceDuration)
                .SetEase(Ease.InOutQuad)
                .SetLink(gameObject)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnTriggerEnter(Collider other) {
            if (_isCollected)
                return;

            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller == null)
                controller = other.GetComponentInParent<CharacterController>();

            if (controller != null) {
                Collect();
            }
        }

        private void Collect() {
            _isCollected = true;
            _bounceTween?.Kill();

            ItemController.Instance.CollectItem(itemData);
            SoundController.Instance.PlaySound(itemPickup);
            
            transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .SetLink(gameObject)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}