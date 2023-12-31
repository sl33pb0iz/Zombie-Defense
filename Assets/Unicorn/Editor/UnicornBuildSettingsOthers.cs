
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.Editor
{
    public partial class UnicornBuildSettings
    {
        private void ResetOthers()
        {
            icon = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Unknown)[0];

            var splashPrefab = AssetDatabase.LoadAssetAtPath<LoadingStartManager>("Assets/Unicorn/Prefabs/AppOpen.prefab").GetComponentInChildren<Image>();
            splash = splashPrefab.sprite;
            previewSplash = splash;
        }

        private void SaveOthers()
        {
            SaveIcon();
            SaveSplash();
            SaveGoogleServicesJson();
            
            AssetDatabase.Refresh();
        }

        private void SaveIcon()
        {
            if (icon != null)
            {
                PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new[] {icon});
            }
        }

        private void SaveSplash()
        {
            if (splash)
            {
                var splashPrefab = AssetDatabase.LoadAssetAtPath<LoadingStartManager>("Assets/Unicorn/Prefabs/AppOpen.prefab");
                splashPrefab.transform.GetChild(0).GetComponent<Image>().sprite = splash;
                splashPrefab.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = splash;

                EditorUtility.SetDirty(splashPrefab);
                AssetDatabase.SaveAssets();
            }
        }

        private void SaveGoogleServicesJson()
        {
            if (string.IsNullOrWhiteSpace(googleServices))
                return;

            if (!File.Exists(googleServices))
                return;

            string pathToGoogleServiceJson = Application.dataPath + "/_SDK/google-services.json";

            if (pathToGoogleServiceJson.Equals(googleServices))
                return;

            File.Copy(googleServices, pathToGoogleServiceJson, true);
        }
    }
}