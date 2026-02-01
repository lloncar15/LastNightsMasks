using UnityEngine;

namespace LastNightsMasks.Player {
    public class SoundController : MonoBehaviour {
        private static SoundController _instance;
        
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;

        [Header("Volume Settings")]
        [Range(0f, 1f)] public float masterVolume = 1f;
        [Range(0f, 1f)] public float musicVolume = 1f;
        [Range(0f, 1f)] public float sfxVolume = 1f;

        [Header("Music Fade Settings")]
        [SerializeField] private float musicFadeDuration = 1f;

        [Header("Background Music")] 
        [SerializeField] private AudioClip backgroundMusic;
        
        public static SoundController Instance {
            get {
                if (_instance == null) {
                    _instance = FindAnyObjectByType<SoundController>();
                    if (_instance == null) {
                        GameObject go = new GameObject("InputController");
                        _instance = go.AddComponent<SoundController>();
                    }
                }

                return _instance;
            }
        }
        
        private void Awake() {
            if (!Application.isPlaying) 
                return;

            _instance = this;
        }
        
        private void Start() {
            if (musicSource == null) 
                return;
            
            musicSource.loop = true;
            UpdateMusicVolume();
            PlayMusic(backgroundMusic);
        }

        public void PlayMusic(AudioClip clip) {
            if (musicSource == null || clip == null)
                return;

            musicSource.clip = clip;
            musicSource.volume = GetMusicVolume();
            musicSource.Play();
        }

        public void StopMusic() {
            if (musicSource == null)
                return;

            musicSource.Stop();
        }
        
        public void PlaySound(AudioSource source, AudioClip clip) {
            if (source == null || clip == null)
                return;

            source.volume = GetSFXVolume();
            source.PlayOneShot(clip);
        }

        public void PlaySound(AudioSource source, AudioClip clip, float volumeMultiplier) {
            if (source == null || clip == null)
                return;

            source.PlayOneShot(clip, GetSFXVolume() * volumeMultiplier);
        }

        private float GetMusicVolume()
        {
            return masterVolume * musicVolume;
        }

        private float GetSFXVolume()
        {
            return masterVolume * sfxVolume;
        }

        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            UpdateMusicVolume();
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            UpdateMusicVolume();
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
        }

        private void UpdateMusicVolume()
        {
            if (musicSource != null && musicSource.isPlaying)
            {
                musicSource.volume = GetMusicVolume();
            }
        }
    }
}