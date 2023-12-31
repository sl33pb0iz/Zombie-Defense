using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google;
using UnityEditor;
using UnityEngine;

namespace _SDK.Editor
{
    public class MaxSDKLoadDomainSupporter
    {
        [RuntimeInitializeOnLoadMethod]
        private static void InitializeForDisabledDomainReload()
        {
            if (EditorSettings.enterPlayModeOptionsEnabled 
                && !EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableDomainReload)
                || !EditorSettings.enterPlayModeOptionsEnabled)
            {
                return;
            }
            
            ResetMaxSdkUnityEditorFields();
            ResetMaxSdkCallbacks();
            InitializeMaxSdk();
        }
        
        private static void ResetMaxSdkUnityEditorFields()
        {
            //HashSet<string> requestedAdUnits = (HashSet<string>) typeof(MaxSdkUnityEditor)
            //    .GetField("RequestedAdUnits", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(typeof(HashSet<string>));
            //requestedAdUnits?.Clear();
            
            //HashSet<string> readyAdUnits = (HashSet<string>) typeof(MaxSdkUnityEditor)
            //    .GetField("ReadyAdUnits", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(typeof(HashSet<string>));
            //readyAdUnits?.Clear();
            
            //Dictionary<string, GameObject> stubBanners = (Dictionary<string, GameObject>) typeof(MaxSdkUnityEditor)
            //    .GetField("StubBanners", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(typeof(Dictionary<string, GameObject>));
            //stubBanners?.Clear();
        }

        private static void InitializeMaxSdk()
        {
            //typeof(MaxSdkBase).GetMethod("InitCallbacks", BindingFlags.Static | BindingFlags.NonPublic)?.Invoke(null, new object[] { });
        }

        private static void ResetMaxSdkCallbacks()
        {
            //var fieldInfos = typeof(MaxSdkCallbacks).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            //foreach (var fieldInfo in fieldInfos)
            //{
            //    fieldInfo?.SetValue(null, null);
            //}
        }

    }
}