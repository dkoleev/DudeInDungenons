using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor.ToolsMenu.GameBuilder {
    public abstract class GameBuilder {
        protected readonly string buildPathBase = $"{Application.dataPath}/../Builds";

        public BuildSummary Build(bool buildAddressable = false, bool buildAppBundle = false, BuildOptions buildOptions = BuildOptions.None) {
            EditorUtility.DisplayProgressBar("Building", "Preparing...", 0f);

            if (buildAddressable) {
                AddressableAssetSettings.BuildPlayerContent();
            }
                
            var scenes = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes) {
                scenes.Add(scene.path);
            }

            var options = SetBuildOptions(scenes.ToArray(), buildAppBundle, buildOptions);
            var build = BuildPipeline.BuildPlayer(options);

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

            return summary;
        }

        protected abstract BuildPlayerOptions SetBuildOptions(string[] scenes, bool buildAppBundle, BuildOptions buildOptions = BuildOptions.None);

        protected string GetCurrentBuildName(string branch) {
            return $"di_{branch}_v.{PlayerSettings.bundleVersion}";
        }
    }
}