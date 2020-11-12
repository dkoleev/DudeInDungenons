using Runtime.Logic.Core.SaveEngine;
using Runtime.Logic.GameProgress;
using UnityEditor;
using UnityEngine;

namespace Editor.ToolsMenu {
    public static class ProgressEditor {
        [MenuItem("Avocado/Save/Reset")]
        public static void EraseSave() {
            SaveEngine<GameProgress>.Delete();
            Debug.LogWarning("Save file deleted");
           // PlayerPrefs.DeleteAll();
          //  PlayerPrefs.Save();
        }
    }
}