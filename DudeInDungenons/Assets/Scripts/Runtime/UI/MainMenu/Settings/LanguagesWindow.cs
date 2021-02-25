using System.Collections.Generic;
using Runtime.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu.Settings {
    public class LanguagesWindow : UiWindow {
        [SerializeField]
        private Dictionary<SystemLanguage, Button> _languageButtons;
        
        protected override void Awake() {
            base.Awake();
            Hide();
        }
    }
}