using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.SFXManager {
    [System.Serializable]
    public class SFX {
        [LabelText("SFX Type")]
        [LabelWidth(100)]
        [OnValueChanged("SFXChange")]
        public SFXManager.SFXType sfxType = SFXManager.SFXType.UI;

        [LabelText("$sfxLabel")]
        [LabelWidth(100)]
        [ValueDropdown("SFXType")]
        [OnValueChanged("SFXChange")]
        [InlineButton("SelectSFX")]
        [InlineButton("Stop")]
        [InlineButton("Play")]
        public SFXClip sfxToPlay;
        private string sfxLabel = "SFX";

        [SerializeField]
        private bool showSettings = false;

        [ShowIf("showSettings")]
        [SerializeField]
        private bool editSettings = false;

        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        [ShowIf("showSettings")]
        [EnableIf("editSettings")]
        [SerializeField]
        private SFXClip _sfxBase;

        [Title("Audio Source")]
        [ShowIf("showSettings")]
        [EnableIf("editSettings")]
        [SerializeField]
        private bool waitToPlay = true;

        [ShowIf("showSettings")]
        [EnableIf("editSettings")]
        [SerializeField]
        private bool useDefault = true;

        [DisableIf("useDefault")]
        [ShowIf("showSettings")]
        [EnableIf("editSettings")]
        [SerializeField]
        private AudioSource audiosource;

        private void SFXChange() {
            if (sfxToPlay == null) {
                return;
            }

            sfxLabel = sfxToPlay.sfxType + " SFX";
            _sfxBase = sfxToPlay;
        }

        private void SelectSFX() {
            UnityEditor.Selection.activeObject = sfxToPlay;
        }

        private List<SFXClip> SFXType() {
            List<SFXClip> sfxList;

            switch (sfxType) {
                case SFXManager.SFXType.UI:
                    sfxList = SFXManager.Instance.uiSFX;
                    break;
                case SFXManager.SFXType.Ambient:
                    sfxList = SFXManager.Instance.ambientSFX;
                    break;
                case SFXManager.SFXType.Weapons:
                    sfxList = SFXManager.Instance.weaponSFX;
                    break;
                default:
                    sfxList = SFXManager.Instance.uiSFX;
                    break;
            }

            return sfxList;
        }

        public void Play() {
            if (useDefault || audiosource == null)
                SFXManager.PlaySFX(sfxToPlay, waitToPlay, null);
            else
                SFXManager.PlaySFX(sfxToPlay, waitToPlay, audiosource);
        }
        
        public void Stop() {
            SFXManager.Instance.StopAll();
        }
    }
}