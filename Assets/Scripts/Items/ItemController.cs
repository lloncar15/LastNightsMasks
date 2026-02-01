using System;
using System.Collections.Generic;
using UnityEngine;

namespace LastNightsMasks.Items {
    public class ItemController : MonoBehaviour {
        private static ItemController _instance;
        
        public static ItemController Instance {
            get {
                if (_instance == null) {
                    _instance = FindAnyObjectByType<ItemController>();
                    if (_instance == null) {
                        GameObject go = new GameObject("InputController");
                        _instance = go.AddComponent<ItemController>();
                    }
                }

                return _instance;
            }
        }
        
        public event Action<ItemData> OnItemCollected;

        private HashSet<int> _collectedItemIds = new();
        private List<ItemData> _collectedItems = new();

        public IReadOnlyList<ItemData> CollectedItems => _collectedItems;

        private void Awake() {
            if (!Application.isPlaying) 
                return;

            _instance = this;
        }

        public bool HasItem(ItemData item) {
            return _collectedItemIds.Contains(item.itemId);
        }

        public bool HasItem(int itemId) {
            return _collectedItemIds.Contains(itemId);
        }

        public void CollectItem(ItemData item) {
            if (!_collectedItemIds.Add(item.itemId))
                return;

            _collectedItems.Add(item);
            OnItemCollected?.Invoke(item);
        }

        public void ItemDropToActivate(ItemDrop item) {
            item.Appear();
        }
    }
}