using UnityEngine;

namespace LastNightsMasks.Player {
    [CreateAssetMenu(fileName = "Player Config", menuName = "LastNightsMasks/Player Config")]
    public class PlayerConfig : ScriptableObject {
        [Header("Movement")]
        [SerializeField] public float moveSpeed = 4f;
        [SerializeField] public float gravity = -9.81f;
        
        [Header("Look")]
        [SerializeField] public float lookSensitivity = 0.15f;
        [SerializeField] public float maxLookAngle = 85f;
    }
}