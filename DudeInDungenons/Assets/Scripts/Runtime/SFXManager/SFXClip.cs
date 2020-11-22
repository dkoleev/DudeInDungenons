using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.SFXManager {
    [CreateAssetMenu(menuName = "Avocado/New SFX Clip", fileName = "NewSFXClip")]
    public class SFXClip : ScriptableObject {
        [Space]
        [LabelText("SFX Type")]
        [LabelWidth(100)]
        [InlineButton("Stop")]
        [InlineButton("Play")]
        public SFXManager.SFXType sfxType = SFXManager.SFXType.UI;
        
        [Title("Audio Clip")]
        [Required]
        public AudioClip clip;

        [Title("Clip Settings")]
        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(0f, 0.2f)]
        public float volumeVariation = 0.05f;

        [Range(0f, 2f)]
        public float pitch = 1f;

        [Range(0f, 0.2f)]
        public float pitchVariation = 0.05f;

        public bool loop = false;
        
        [UsedImplicitly]
        public void Play() {
            SFXManager.PlaySFX(this, false);
        }
        
        [UsedImplicitly]
        public void Stop() {
            SFXManager.Instance.StopAll();
        }
    }
}