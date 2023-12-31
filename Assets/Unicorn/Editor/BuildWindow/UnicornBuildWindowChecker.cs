using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.Editor
{
    public partial class UnicornBuilder
    {
        private bool CheckMaxMediation()
        {
            string maxMediationControllerPath = Application.dataPath + "/_SDK/Services/Modules/Ads/Scripts/IronSourceController.cs";
            string text = File.ReadAllText(maxMediationControllerPath);
            var appkey = Regex.Match(text, "private const string appKey = \"(.+?)\"").Groups[1].Captures[0].Value;
            Debug.Log("appkey --- " + appkey);
            return !appkey.Equals("ffa5daaf8c43fef4");

        }

        #region Admob
        private bool CheckAdMob()
        {
            var appflyer = GetAdmobIS();
            var aoa = GetAdmobAOA();

            if (!aoa.Equals(appflyer))
                return false;

            if (aoa.Equals("ca-app-pub-6336405384015455~3589735923"))
                return false;

            return CheckAdmobTiers();
        }

        private static string GetAdmobAppflyer()
        {
            string adMob = Application.dataPath + "/MaxSdk/Resources/AppLovinSettings.asset";
            string text = File.ReadAllText(adMob);
            return Regex.Match(text, "adMobAndroidAppId: (.+)").Groups[1].Captures[0].Value;
        }

        private static string GetAdmobIS()
        {
            string adMob = Application.dataPath + "/Ironsource/Resources/IronSourceMediatedNetworkSettings.asset";
            string text = File.ReadAllText(adMob);
            return Regex.Match(text, "AdmobAndroidAppId: (.+)").Groups[1].Captures[0].Value;
        }

        private static string GetAdmobAOA()
        {
            string adMob = Application.dataPath + "/GoogleMobileAds/Resources/GoogleMobileAdsSettings.asset";
            string text = File.ReadAllText(adMob);
            return Regex.Match(text, "adMobAndroidAppId: (.+)").Groups[1].Captures[0].Value;
        }

        private bool CheckAdmobTiers()
        {
            string appOpenAdManagerPath = Application.dataPath + "/_SDK/AppOpenAdManager.cs";
            string text = File.ReadAllText(appOpenAdManagerPath);
            var tier1 = Regex.Match(text, "private const string AD_UNIT_ID_1 = \"(.+?)\"").Groups[1].Captures[0].Value; ;
            var tier2 = Regex.Match(text, "private const string AD_UNIT_ID_2 = \"(.+?)\"").Groups[1].Captures[0].Value; ;
            var tier3 = Regex.Match(text, "private const string AD_UNIT_ID_3 = \"(.+?)\"").Groups[1].Captures[0].Value; ;

            return !tier1.Equals("ca-app-pub-6336405384015455/2759350150")
                   && !tier2.Equals("ca-app-pub-6336405384015455/1254696796")
                   && !tier3.Equals("ca-app-pub-6336405384015455/4427655041");
        }
        #endregion

        private bool CheckFirebase()
        {
            var googleServices = File.ReadAllText(Application.dataPath +
                             "/Plugins/Android/FirebaseApp.androidlib/res/values/google-services.xml");

            var checkString =
                File.ReadAllText(Application.dataPath + "/Unicorn/Editor/BuildWindow/google-services-checker.xml");

            return !checkString.Equals(googleServices);
        }

        private bool CheckPackageName()
        {
            return !"com.unicorn.baby.squid.survival".Equals(RocketConfig.package_name);
        }

        private bool CheckSplash()
        {
            var splashPrefab = AssetDatabase.LoadAssetAtPath<LoadingStartManager>("Assets/Unicorn/Prefabs/AppOpen.prefab");
            var splash1 = splashPrefab.transform.GetChild(0).GetComponent<Image>().sprite;
            var splash2 = splashPrefab.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite;

            var splash1Path = Application.dataPath + "/../" + AssetDatabase.GetAssetPath(splash1);
            var splash2Path = Application.dataPath + "/../" + AssetDatabase.GetAssetPath(splash1);

            if (!splash1Path.Equals(splash2Path))
                return false;

            var checkerPath = Application.dataPath + "/Unicorn/Editor/BuildWindow/splash_roblock-checker.jpg";

            return !FilesAreEqual_Hash(new FileInfo(splash1Path), new FileInfo(checkerPath)); ;

        }

        private bool CheckIcon()
        {
            var icons = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Unknown);
            if (0 == icons.Length || icons.Length > 1)
                return false;

            var icon = icons[0];
            var iconPath = Application.dataPath + "/../" + AssetDatabase.GetAssetPath(icon);
            var checkerPath = Application.dataPath + "/Unicorn/Editor/BuildWindow/logo-checker.png";

            return !FilesAreEqual_Hash(new FileInfo(iconPath), new FileInfo(checkerPath));
        }

        static bool FilesAreEqual_Hash(FileInfo first, FileInfo second)
        {
            byte[] firstHash = MD5.Create().ComputeHash(first.OpenRead());
            byte[] secondHash = MD5.Create().ComputeHash(second.OpenRead());

            for (int i = 0; i < firstHash.Length; i++)
            {
                if (firstHash[i] != secondHash[i])
                    return false;
            }
            return true;
        }
    }
}