using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.ToolsMenu.GameBuilder {
    public class AndroidBuilder : GameBuilder{
        protected override BuildPlayerOptions SetBuildOptions(string[] scenes, bool buildAppBundle) {
            var buildOptions = new BuildPlayerOptions();
            buildOptions.target = BuildTarget.Android;
            buildOptions.scenes = scenes;
            buildOptions.options = BuildOptions.None;
            var extension = buildAppBundle ? ".aab" : ".apk";
            buildOptions.locationPathName = Path.Combine(buildPathBase + "/Android", GetCurrentBuildName("develop")) + extension;
            
            EditorUserBuildSettings.androidETC2Fallback = AndroidETC2Fallback.Quality32BitDownscaled;
            EditorUserBuildSettings.buildAppBundle = buildAppBundle;
            
            PlayerSettings.Android.keystoreName = $"{Application.dataPath}/../did.keystore";
            PlayerSettings.Android.keystorePass = "132poganycic";
            PlayerSettings.Android.keyaliasName = "did";
            PlayerSettings.Android.keyaliasPass = "132poganycic";

            PlayerSettings.SplashScreen.show = false;

            return buildOptions;
        }
    }
}
