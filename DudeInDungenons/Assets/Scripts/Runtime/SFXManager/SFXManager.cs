using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.SFXManager {
    public class SFXManager : MonoBehaviour {
        private static SFXManager _instance;

        public static SFXManager Instance {
            get {
                if (_instance == null)
                    _instance = FindObjectOfType<SFXManager>();

                return _instance;
            }
        }

        [HorizontalGroup("AudioSource")]
        [SerializeField]
        private AudioSource defaultAudioSource;

        [TabGroup("UI")]
        [SerializeField]
        private AudioSource _audioSourceUI;
        [TabGroup("UI")]
        [AssetList(Path = "/Configuration/Audio/UI_SFX", AutoPopulate = true)]
        public List<SFXClip> uiSFX;

        [TabGroup("Ambient")]
        [SerializeField]
        private AudioSource _audioSourceAmbient;
        [TabGroup("Ambient")]
        [AssetList(Path = "/Configuration/Audio/Ambient_SFX", AutoPopulate = true)]
        public List<SFXClip> ambientSFX;

        [TabGroup("Weapons")]
        [SerializeField]
        private AudioSource _audioSourceWeapons;
        [TabGroup("Weapons")]
        [AssetList(Path = "/Configuration/Audio/Weapon_SFX", AutoPopulate = true)]
        public List<SFXClip> weaponSFX;

        public static void PlaySFX(SFXClip sfx, bool waitToFinish = true, AudioSource audioSource = null) {
            if (audioSource == null) {
                audioSource = Instance.GetAudioSourceByType(sfx.sfxType);
            }

            if (audioSource == null) {
                Debug.LogError("You forgot to add a default audio source!");
                return;
            }

            if (!audioSource.isPlaying || !waitToFinish) {
                audioSource.clip = sfx.clip;
                audioSource.volume = sfx.volume + Random.Range(-sfx.volumeVariation, sfx.volumeVariation);
                audioSource.pitch = sfx.pitch + Random.Range(-sfx.pitchVariation, sfx.pitchVariation);
                audioSource.loop = sfx.loop;
                audioSource.Play();
            }
        }

        public void StopAll() {
            Stop(_audioSourceWeapons);
            Stop(_audioSourceAmbient);
            Stop(_audioSourceUI);
            Stop(defaultAudioSource);
        }

        private void Stop(AudioSource source) {
            if (source != null && source.isPlaying) {
                source.Stop();
            }
        }

        private AudioSource GetAudioSourceByType(SFXType type) {
            AudioSource source = null;
            switch (type) {
                case SFXType.UI:
                    source = _audioSourceUI;
                    break;
                case SFXType.Ambient:
                    source = _audioSourceAmbient;
                    break;
                case SFXType.Weapons:
                    source = _audioSourceWeapons;
                    break;
            }

            if (source == null) {
                source = defaultAudioSource;
            }

            return source;
        }

        [HorizontalGroup("AudioSource")]
        [ShowIf("@defaultAudioSource == null")]
        [GUIColor(1f, 0.5f, 0.5f, 1f)]
        [Button]
        private void AddAudioSource() {
            defaultAudioSource = this.gameObject.GetComponent<AudioSource>();

            if (defaultAudioSource == null)
                defaultAudioSource = this.gameObject.AddComponent<AudioSource>();
        }

        public enum SFXType {
            UI,
            Ambient,
            Weapons
        }
    }
}