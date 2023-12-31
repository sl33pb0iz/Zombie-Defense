using System;
using Sirenix.OdinInspector.Editor;
using Unicorn.Editor;
using UnityEditor;
using UnityEngine;

namespace Unicorn.Unicorn.Editor
{
    public class UnicornBuildHelper: OdinMenuEditorWindow
    {
        private UnicornBuildSettings unicornBuildSettings;
        private UnicornBuilder unicornBuilder;
        private UnicornAPKInstaller unicornAPKInstaller;

        [MenuItem("Unicorn/Build Helper")]
        private static void ShowWindow()
        {
            var window = GetWindow<UnicornBuildHelper>();
            window.titleContent = new GUIContent("Build Helper");
            window.Show();
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            
            unicornBuildSettings = CreateInstance<UnicornBuildSettings>();
            tree.Add("Settings", unicornBuildSettings);
            unicornBuilder = CreateInstance<UnicornBuilder>();
            tree.Add("Build", unicornBuilder);
            unicornAPKInstaller = CreateInstance<UnicornAPKInstaller>();
            tree.Add("Apk installer", unicornAPKInstaller);
            return tree;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (unicornBuildSettings)
            {
                DestroyImmediate(unicornBuildSettings);
            }

            if (unicornBuilder)
            {
                DestroyImmediate(unicornBuilder);
            }

            if (unicornAPKInstaller)
            {
                DestroyImmediate(unicornAPKInstaller);
            }
        }
    }
}