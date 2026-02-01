using UnityEngine;

namespace LastNightsMasks.Items {
    [CreateAssetMenu(fileName = "Item", menuName = "LastNightsMasks/ItemData")]
    public class ItemData : ScriptableObject {
        public string itemName;
        public string itemText;
        public int itemId;
    }
}