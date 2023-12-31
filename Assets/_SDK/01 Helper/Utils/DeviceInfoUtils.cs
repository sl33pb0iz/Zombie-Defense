using System;
using UnityEngine;
//using UnityEngine.iOS;

namespace RocketTeam.Sdk.Utils
{
    public class DeviceInfoUtils
    {
        private static string adsId = string.Empty;
        private static Action<string> onGetAdsId;

//        public static string GetDeviceManufacturer()
//        {
//            string manufacturer = string.Empty;

//            try
//            {
//#if UNITY_EDITOR
//                manufacturer = SystemInfo.graphicsDeviceVendor;
//#elif UNITY_IOS
//                manufacturer = "Apple";
//#elif UNITY_ANDROID
//                using (var build = new AndroidJavaClass("android.os.Build"))
//                {
//                    manufacturer = build.GetStatic<string>("MANUFACTURER");
//                }
//#else
//                manufacturer = SystemInfo.graphicsDeviceVendor;
//#endif
//            }
//            catch (Exception e)
//            {
//                Debug.LogError(e);
//            }

//            return manufacturer;
//        }

//        public static string GetOperatingSystem()
//        {
//            string operatingSystem = string.Empty;

//            try
//            {
//#if UNITY_EDITOR
//                operatingSystem = SystemInfo.operatingSystemFamily.ToString();
//#elif UNITY_IOS
//                operatingSystem = "iOS";
//#elif UNITY_ANDROID
//                operatingSystem = "Android";
//#else
//                operatingSystem = SystemInfo.operatingSystemFamily.ToString();
//#endif
//            }
//            catch (Exception e)
//            {
//                Debug.LogError(e);
//            }

//            return operatingSystem;
//        }

//        public static string GetOperatingSystemVersion()
//        {
//            string operatingSystemVersion = string.Empty;

//            try
//            {
//#if UNITY_EDITOR
//                operatingSystemVersion = SystemInfo.operatingSystem;
//#elif UNITY_IOS
//                operatingSystemVersion = Device.systemVersion;
//#elif UNITY_ANDROID
//                using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
//                {
//                    operatingSystemVersion = version.GetStatic<string>("RELEASE");
//                }
//#else
//                operatingSystemVersion = SystemInfo.operatingSystem;
//#endif
//            }
//            catch (Exception e)
//            {
//                Debug.LogError(e);
//            }

//            return operatingSystemVersion;
//        }

//        public static string GetDeviceId()
//        {
//            string deviceId = string.Empty;

////            try
////            {
////#if UNITY_EDITOR
////                deviceId = SystemInfo.deviceUniqueIdentifier;
////#elif UNITY_ANDROID
////                AndroidJavaClass androidUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
////                AndroidJavaObject unityPlayerActivity = androidUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
////                AndroidJavaObject unityPlayerResolver = unityPlayerActivity.Call<AndroidJavaObject>("getContentResolver");
////                AndroidJavaClass androidSettingsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
////                deviceId = androidSettingsSecure.CallStatic<string>("getString", unityPlayerResolver, "android_id");
////#else
////                deviceId = SystemInfo.deviceUniqueIdentifier;
////#endif
////            }
////            catch (Exception e)
////            {
////                Debug.LogError(e);
////            }

//            return deviceId;
//        }

//        public static void StartGetAdsId(Action<string> callback)
//        {
//            onGetAdsId = callback;
//            Application.RequestAdvertisingIdentifierAsync(AdvertisingIdentifierCallback);
//        }

//        public static string GetAdsId()
//        {
//            return adsId;
//        }

        private static void AdvertisingIdentifierCallback(string advertisingId, bool trackingEnabled, string errorMsg)
        {
            adsId = advertisingId;
            if (onGetAdsId != null)
            {
                onGetAdsId(adsId);
            }
        }
    }
}