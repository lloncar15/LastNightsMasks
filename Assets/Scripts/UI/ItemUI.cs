using LastNightsMasks.Items;
using UnityEngine;

namespace LastNightsMasks.UI {
    public class ItemUI : MonoBehaviour {
        [SerializeField] private ItemUIElement itemUIElementPrefab;
        [SerializeField] private Transform contentParent;

        private void OnEnable() {
            ItemController.Instance.OnItemCollected += OnItemCollected;
        }

        private void OnDisable() {
            ItemController.Instance.OnItemCollected -= OnItemCollected;
        }

        private void OnItemCollected(ItemData item) {
            ItemUIElement element = Instantiate(itemUIElementPrefab, contentParent);
            element.Setup(item);
        }
    }
}