using UnityEditor;

namespace Editor.ToolsMenu.GameBuilder {
    public static class GameBuilderMenu {
        private static GameBuilder _androidBuilder;
        private static GameBuilder _iosBuilder;
        
        static GameBuilderMenu() {
            _androidBuilder = new AndroidBuilder();
            _iosBuilder = new iOSBuilder();
        }
        
        [MenuItem("Avocado/Build/Android/Release APK", false, 10)]
        private static void BuildAndroidApk() {
            _androidBuilder.Build();
        }
        
        [MenuItem("Avocado/Build/Android/Release APK (rebuild assets)", false, 10)]
        private static void BuildAndroidApkRebuildAddressable() {
            _androidBuilder.Build(true);
        }
        
        [MenuItem("Avocado/Build/Android/Release AAB", false, 10)]
        private static void BuildAndroidWithBundles() {
            _androidBuilder.Build(false, true);
        }
        
        [MenuItem("Avocado/Build/Android/Release AAB (rebuild assets)", false, 10)]
        private static void BuildAndroidWithBundlesRebuildAddressable() {
            _androidBuilder.Build(true, true);
        }
        
        [MenuItem("Avocado/Build/iOS/Release", false, 20)]
        private static void BuildIOSRelease() {
            _iosBuilder.Build();
        }
        
        [MenuItem("Avocado/Build/iOS/Release (rebuild assets)", false, 20)]
        private static void BuildIOSReleaseRebuildAddresable() {
            _iosBuilder.Build(true);
        }
    }
}
