using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor.ToolsMenu {
    public class GameBuilder : MonoBehaviour {
        private static readonly string BuildPath = $"{Application.dataPath}/../Builds";
        
        [MenuItem("Avocado/Build/Android/Apk", false,10)]
        private static void BuildAndroidApk() {
            try {
                EditorUtility.DisplayProgressBar("Building", "Preparing...", 0f);
            
                var scenes = new List<string>();
                foreach (var scene in EditorBuildSettings.scenes) {
                    scenes.Add(scene.path);
                }
            
                var buildOptions = new BuildPlayerOptions();
                buildOptions.target = BuildTarget.Android;
                buildOptions.scenes = scenes.ToArray();
                buildOptions.options = BuildOptions.None;
                buildOptions.locationPathName = Path.Combine(BuildPath, GetCurrentBuildName("develop", SystemInfo.deviceName)) + ".apk";
            
                PlayerSettings.Android.keystoreName = $"{Application.dataPath}/../did.keystore";
                PlayerSettings.Android.keystorePass = "132poganycic";
                PlayerSettings.Android.keyaliasName = "did";
                PlayerSettings.Android.keyaliasPass = "132poganycic";

                PlayerSettings.SplashScreen.show = false;
            
                EditorUserBuildSettings.androidETC2Fallback = AndroidETC2Fallback.Quality32BitDownscaled;

                var build = BuildPipeline.BuildPlayer(buildOptions);

                var summary = build.summary;
                switch (summary.result) {
                    case BuildResult.Succeeded:
                        var scenesString = new StringBuilder();
                        scenesString.Append("Scenes in build:\n");
                        foreach (var scene in scenes) {
                            scenesString.Append(scene);
                            scenesString.Append(", ");
                        }

                        Debug.Log("Build succeed! time: " + summary.totalTime + "; build size: " + summary.totalSize 
                                  + "; \npath: " + summary.outputPath + "\n" + scenesString);
                        break;
                    case BuildResult.Failed:
                        Debug.LogError("Build failed!");
                        break;
                }
            } finally {
                EditorUtility.ClearProgressBar();
            }
        }
        
        public static string GetCurrentBuildName(string branch, string machine) {
            return $"rd_{branch}_{machine}_v.{PlayerSettings.bundleVersion}";
        }

        [MenuItem("Avocado/Build/Settings", false, 20)]
        private static void Inspect() {
            StaticInspectorWindow.InspectType(typeof(GameBuilder)); 
        }
    }
}
