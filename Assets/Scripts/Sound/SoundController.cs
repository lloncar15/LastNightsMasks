using LastNightsMasks.Utils;
using UnityEngine;
using Yarn.Unity;

namespace LastNightsMasks.Sound {
    public class SoundController : PersistentSingleton<SoundController> {
        [Header("Audio Sources")]
        [SerializeField] public AudioSource musicSource;
        [SerializeField] public AudioSource sfxSource;

        [Header("Volume Settings")]
        [Range(0f, 1f)] public float masterVolume = 1f;
        [Range(0f, 0.5f)] public float musicVolume = 0.3f;
        [Range(0f, 1f)] public float sfxVolume = 1f;

        [Header("Background Music")] 
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] public AudioClip seen;
        
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

        public void PlaySound(AudioClip clip) {
            if (clip == null)
                return;
            
            sfxSource.volume = GetSFXVolume();
            sfxSource.PlayOneShot(clip);
        }

        public void PlaySound(AudioSource source, AudioClip clip, float volumeMultiplier) {
            if (source == null || clip == null)
                return;

            source.PlayOneShot(clip, GetSFXVolume() * volumeMultiplier);
        }

        private float GetMusicVolume() {
            return masterVolume * musicVolume;
        }

        private float GetSFXVolume() {
            return masterVolume * sfxVolume;
        }

        public void SetMasterVolume(float volume) {
            masterVolume = Mathf.Clamp01(volume);
            UpdateMusicVolume();
        }

        public void SetMusicVolume(float volume) {
            musicVolume = Mathf.Clamp01(volume);
            UpdateMusicVolume();
        }

        public void SetSFXVolume(float volume) {
            sfxVolume = Mathf.Clamp01(volume);
        }

        private void UpdateMusicVolume() {
            if (musicSource != null && musicSource.isPlaying) {
                musicSource.volume = GetMusicVolume();
            }
        }

        [YarnCommand("seen")]
        public void Seen() {
            Instance.PlaySound(sfxSource, seen);
        }
    }
}