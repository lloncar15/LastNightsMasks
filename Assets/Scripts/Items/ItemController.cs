using System;
using System.Collections.Generic;
using LastNightsMasks.Sound;
using LastNightsMasks.Utils;
using UnityEngine;
using Yarn.Unity;

namespace LastNightsMasks.Items {
    public class ItemController : GenericSingleton<ItemController> {
        [SerializeField] private AudioClip allItemsCollectedSound;
        public static event Action<ItemData> OnItemCollected;

        private readonly HashSet<int> _collectedItemIds = new();
        private readonly List<ItemData> _collectedItems = new();
        
        [SerializeField] private InMemoryVariableStorage _storage;

        public IReadOnlyList<ItemData> CollectedItems => _collectedItems;

        public static Action OnAllItemsCollected;
        
        private const int MAX_ITEMS = 4;

        private void Start() {
            _storage = FindAnyObjectByType<InMemoryVariableStorage>();
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

            SetYarnVariable(item.yarnId);
            
            _collectedItems.Add(item);
            OnItemCollected?.Invoke(item);

            if (_collectedItems.Count == MAX_ITEMS) {
                SoundController.Instance.PlaySound(allItemsCollectedSound);
                OnAllItemsCollected?.Invoke();
            }
        }

        public void ItemDropToActivate(ItemDrop item) {
            item.Appear();
        }

        private void SetYarnVariable(string yarnId) {
            _storage.SetValue(yarnId, true);
        }
    }
}