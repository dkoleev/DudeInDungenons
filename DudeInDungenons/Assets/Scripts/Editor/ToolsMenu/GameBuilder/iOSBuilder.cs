using UnityEditor;

namespace Editor.ToolsMenu.GameBuilder {
    public class iOSBuilder : GameBuilder {
        protected override BuildPlayerOptions SetBuildOptions(string[] scenes, bool buildAppBundle) {
            var buildOptions = new BuildPlayerOptions();
            buildOptions.target = BuildTarget.iOS;
            buildOptions.scenes = scenes;
            buildOptions.options = BuildOptions.None;
            buildOptions.locationPathName = buildPathBase +"/iOS";
            
            PlayerSettings.SplashScreen.show = false;

            return buildOptions;
        }
    }
}