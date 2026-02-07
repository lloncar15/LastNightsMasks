using UnityEngine;

namespace LastNightsMasks.Utils {
    public class Singleton<T> : MonoBehaviour where T : Component {
        private static T _instance;
        public static bool HasInstance =>  _instance != null;
        
        public static T Instance {
            get {
                if (_instance == null) {
                    _instance = FindAnyObjectByType<T>();
                    if (_instance == null) {
                        GameObject go = new GameObject(typeof(T).Name + " Auto-Generated");
                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
        
        protected virtual void Awake() {
            if (!Application.isPlaying) return;
            
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
                return;
            }
            
            _instance = this as T;
        }
        
        protected virtual void OnDestroy() {
            if (_instance == this)
                _instance = null;
        }
    }
}