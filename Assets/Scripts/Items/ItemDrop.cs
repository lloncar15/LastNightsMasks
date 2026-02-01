using System;
using DG.Tweening;
using UnityEngine;

namespace LastNightsMasks.Items {
    public class ItemDrop : MonoBehaviour {
        [SerializeField] private ItemData itemData;
        [SerializeField] private float bounceHeight = 0.3f;
        [SerializeField] private float bounceDuration = 1f;

        private Vector3 _startPosition;
        private Tween _bounceTween;
        private bool _isCollected;

        public static Action<ItemData> OnItemPickup;
        
        public void Appear() {
            _startPosition = transform.position;
            gameObject.SetActive(true);
            StartBouncing();
        }

        public void Start() {
            StartBouncing();
        }

        private void StartBouncing()
        {
            _bounceTween = transform.DOMoveY(_startPosition.y + bounceHeight, bounceDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit) {
            // This is called on the player, not the item
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
            
            transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => Destroy(gameObject));
        }

        private void OnDestroy() {
            _bounceTween?.Kill();
        }
    }
}