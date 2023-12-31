using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Unicorn.Editor
{
    public partial class UnicornBuildSettings
    {
        private const string maxAOA_KEY = RocketConfig.package_name + "|Max&AOA";

        const string maxAOARegex =
            @"INT: (?'INT'.*)
RV: (?'RV'.*)
BN: (?'BN'.*)

Admob: (?'Admob'.*)
Tier1: (?'Tier1'.*)
Tier2: (?'Tier2'.*)
Tier3: (?'Tier3'.*)";

        private void OnAOAAndMaxInfoChanged()
        {
            //maxAndAOAInfo = Regex.Replace(maxAndAOAInfo, @"\r\n|\r|\n", System.Environment.NewLine);
            //SaveMaxAndAOAString();
            //string message = null;
            //if (!ValidateMaxAOA(maxAndAOAInfo, ref message))
            //{
            //    return;
            //}

            //var matches = Regex.Match(maxAndAOAInfo, maxAOARegex);
            //InterstitialAdUnitId = matches.Groups["INT"].Value.Trim();
            //RewardedAdUnitId = matches.Groups["RV"].Value.Trim();
            //BannerAdUnitId = matches.Groups["BN"].Value.Trim();

            //Admob = matches.Groups["Admob"].Value.Trim();
            //AD_UNIT_ID_1 = matches.Groups["Tier1"].Value.Trim();
            //AD_UNIT_ID_2 = matches.Groups["Tier2"].Value.Trim();
            //AD_UNIT_ID_3 = matches.Groups["Tier3"].Value.Trim();

            //maxAndAOAInfo = Regex.Replace(maxAndAOAInfo, @"\r\n|\r|\n", "\n");
        }

        private void SaApplyMaxAndAoa()
        {
            SaveMaxAndAOAString();
            FillIronsourceMediation(AppKeyIronsource);
            FillAdMob(Admob);
            FillAppOpenTiers(AD_UNIT_ID_1, AD_UNIT_ID_2, AD_UNIT_ID_3);
            AssetDatabase.Refresh();
        }

        private void SaveMaxAndAOAString()
        {
            //EditorPrefs.SetString(maxAOA_KEY, maxAndAOAInfo);
        }

        private bool ValidateMaxAOA(string maxAOA, ref string message)
        {
            //maxAOA = Regex.Replace(maxAndAOAInfo, @"\r\n|\r|\n", System.Environment.NewLine);

            //if (string.IsNullOrWhiteSpace(maxAOA))
            //{
            //    message = "Copy đoạn chị Thảo rep anh Thắng vào đây nhé";
            //    return false;
            //}

            //if (!Regex.IsMatch(maxAOA, maxAOARegex, RegexOptions.Multiline))
            //{
            //    message = "Không đúng mẫu rồi.\nCopy đoạn chỉ Thảo rep anh Thắng vào đây hoặc nhập tay vào file MaxMediationController,...!";
            //    return false;
            //}

            return true;
        }

        private void FillMaxMediation(string inter, string reward, string banner)
        {
            if (string.IsNullOrWhiteSpace(inter) || string.IsNullOrWhiteSpace(reward) || string.IsNullOrWhiteSpace(banner))
                return;

            string maxMediationControllerPath = Application.dataPath + "/_SDK/Services/Modules/Ads/Scripts/IronSourceController.cs";
            string text = File.ReadAllText(maxMediationControllerPath);
            text = Regex.Replace(text, "private const string InterstitialAdUnitId = \".+?\"", $"private const string InterstitialAdUnitId = \"{inter}\"");
            text = Regex.Replace(text, "private const string RewardedAdUnitId = \".+?\"", $"private const string RewardedAdUnitId = \"{reward}\"");
            text = Regex.Replace(text, "private const string BannerAdUnitId = \".+?\"", $"private const string BannerAdUnitId = \"{banner}\"");
            File.WriteAllText(maxMediationControllerPath, text);

        }

        private void FillIronsourceMediation(string appKey)
        {
            if (string.IsNullOrWhiteSpace(appKey))
                return;

            string maxMediationControllerPath = Application.dataPath + "/_SDK/Services/Modules/Ads/Scripts/IronSourceController.cs";
            string text = File.ReadAllText(maxMediationControllerPath);
            text = Regex.Replace(text, "private const string appKey = \".+?\"", $"private const string appKey = \"{appKey}\"");

            File.WriteAllText(maxMediationControllerPath, text);

            string appkey = Application.dataPath + "/Ironsource/Resources/IronSourceMediationSettings.asset";
            string textTmp = File.ReadAllText(appkey);
#if UNITY_ANDROID
            textTmp = Regex.Replace(textTmp, "AndroidAppKey: .+", $"AndroidAppKey: {appKey}");
#else
            textTmp = Regex.Replace(textTmp, "IOSAppKey: .+", $"IOSAppKey: {appKey}");
#endif
            File.WriteAllText(appkey, textTmp);
        }

        private void FillAdMob(string admob)
        {
            if (string.IsNullOrWhiteSpace(admob))
                return;

            FillIronsourceAdmob(admob);
            FillGoogleAppOpen(admob);
        }

        private static void FillAppflyerAdmob(string admob)
        {
            string adMob = Application.dataPath + "/MaxSdk/Resources/AppLovinSettings.asset";
            string text = File.ReadAllText(adMob);
            text = Regex.Replace(text, "adMobAndroidAppId: .+", $"adMobAndroidAppId: {admob}");
            File.WriteAllText(adMob, text);
        }

        private static void FillIronsourceAdmob(string admob)
        {
            string adMob = Application.dataPath + "/Ironsource/Resources/IronSourceMediatedNetworkSettings.asset";
            string text = File.ReadAllText(adMob);
          
#if UNITY_ANDROID
            text = Regex.Replace(text, "AdmobAndroidAppId: .+", $"AdmobAndroidAppId: {admob}");
#else
            text = Regex.Replace(text, "AdmobIOSAppId: .+", $"AdmobIOSAppId: {admob}");
#endif
            File.WriteAllText(adMob, text);
        }

        private static void FillGoogleAppOpen(string admob)
        {
            string adMob = Application.dataPath + "/GoogleMobileAds/Resources/GoogleMobileAdsSettings.asset";
            string text = File.ReadAllText(adMob);
            text = Regex.Replace(text, "adMobAndroidAppId: .+", $"adMobAndroidAppId: {admob}");
            File.WriteAllText(adMob, text);
        }

        private void FillAppOpenTiers(string tier1, string tier2, string tier3)
        {
            if (string.IsNullOrWhiteSpace(tier1) || string.IsNullOrWhiteSpace(tier2) || string.IsNullOrWhiteSpace(tier3))
                return;

            string appOpenAdManagerPath = Application.dataPath + "/_SDK/AppOpenAdManager.cs";
            string text = File.ReadAllText(appOpenAdManagerPath);
            text = Regex.Replace(text, "private const string AD_UNIT_ID_1 = \".+?\"", $"private const string AD_UNIT_ID_1 = \"{tier1}\"");
            text = Regex.Replace(text, "private const string AD_UNIT_ID_2 = \".+?\"", $"private const string AD_UNIT_ID_2 = \"{tier2}\"");
            text = Regex.Replace(text, "private const string AD_UNIT_ID_3 = \".+?\"", $"private const string AD_UNIT_ID_3 = \"{tier3}\"");
            File.WriteAllText(appOpenAdManagerPath, text);
        }

        private void ResetAOAAndMAX()
        {
            //maxAndAOAInfo = EditorPrefs.GetString(maxAOA_KEY);
            //OnAOAAndMaxInfoChanged();
        }
    }
}