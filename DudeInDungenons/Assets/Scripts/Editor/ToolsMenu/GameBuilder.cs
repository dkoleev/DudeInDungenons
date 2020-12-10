using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor.ToolsMenu {
    public class GameBuilder : MonoBehaviour {
        private static readonly string BuildPath = $"{Application.dataPath}/../Builds";
        private static readonly string BuildPathIOS = $"{Application.dataPath}/../Builds/iOS";

        [MenuItem("Avocado/Build/Android", false, 10)]
        private static void BuildAndroidApk() {
            BuildAndroid(false);
        }
        
        [MenuItem("Avocado/Build/Android (with App Bundle)", false, 10)]
        private static void BuildAndroidWithBundles() {
            BuildAndroid(true);
        }
        
        [MenuItem("Avocado/Build/iOS", false, 20)]
        private static void BuildIOSRelease() {
            BuildIOS();
        }
        
          private static void BuildIOS() {
            try {
                EditorUtility.DisplayProgressBar("Building", "Preparing...", 0f);
            
                var scenes = new List<string>();
                foreach (var scene in EditorBuildSettings.scenes) {
                    scenes.Add(scene.path);
                }
            
                var buildOptions = new BuildPlayerOptions();
                buildOptions.target = BuildTarget.iOS;
                buildOptions.scenes = scenes.ToArray();
                buildOptions.options = BuildOptions.None;
                buildOptions.locationPathName = BuildPathIOS;
            
                PlayerSettings.Android.keystoreName = $"{Application.dataPath}/../did.keystore";
                PlayerSettings.Android.keystorePass = "132poganycic";
                PlayerSettings.Android.keyaliasName = "did";
                PlayerSettings.Android.keyaliasPass = "132poganycic";

                PlayerSettings.SplashScreen.show = false;

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

        private static void BuildAndroid(bool buildAppBundle) {
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
                var extension = buildAppBundle ? ".aab" : ".apk";
                buildOptions.locationPathName = Path.Combine(BuildPath, GetCurrentBuildName("develop")) + extension;
            
                PlayerSettings.Android.keystoreName = $"{Application.dataPath}/../did.keystore";
                PlayerSettings.Android.keystorePass = "132poganycic";
                PlayerSettings.Android.keyaliasName = "did";
                PlayerSettings.Android.keyaliasPass = "132poganycic";

                PlayerSettings.SplashScreen.show = false;
            
                EditorUserBuildSettings.androidETC2Fallback = AndroidETC2Fallback.Quality32BitDownscaled;
                EditorUserBuildSettings.buildAppBundle = buildAppBundle;

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
        
        public static string GetCurrentBuildName(string branch) {
            return $"rd_{branch}_v.{PlayerSettings.bundleVersion}";
        }
    }
}
