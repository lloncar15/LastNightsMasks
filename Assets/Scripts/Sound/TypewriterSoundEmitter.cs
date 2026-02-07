using System.Threading;
using TMPro;
using UnityEngine;
using Yarn.Markup;
using Yarn.Unity;

namespace LastNightsMasks.Sound {
    public class TypewriterSoundEmitter : ActionMarkupHandler {
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Vector2 pitchRange = new(0.5f, 0.6f);
        [SerializeField] [Range(0f, 1f)] private float volume = 0.5f;
        [SerializeField] private int characterStep = 3;

        public override YarnTask OnCharacterWillAppear(int currentCharacterIndex, MarkupParseResult line, CancellationToken cancellationToken) {
            if (currentCharacterIndex % characterStep != 0) {
                return YarnTask.Yield();
            }
            
            audioSource.pitch = UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
            SoundController.Instance.PlaySound(audioSource, clip, volume);
            
            return YarnTask.Yield();
        }
        
        public override void OnPrepareForLine(MarkupParseResult line, TMP_Text text) {}
        public override void OnLineDisplayBegin(MarkupParseResult line, TMP_Text text) {}
        public override void OnLineDisplayComplete() {}
        public override void OnLineWillDismiss() {}
    }
}