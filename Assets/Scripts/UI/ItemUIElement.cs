using DG.Tweening;
using LastNightsMasks.Items;
using TMPro;
using UnityEngine;

namespace LastNightsMasks.UI {
    public class ItemUIElement : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI itemNameText;
        
        public void Setup(ItemData item) {
            itemNameText.text = item.itemName;
            PlayAppearAnimation();
        }

        private void PlayAppearAnimation() {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }
    }
}