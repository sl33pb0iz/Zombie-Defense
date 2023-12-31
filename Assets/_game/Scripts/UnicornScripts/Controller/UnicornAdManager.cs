using System;
using Unicorn.UI;
using UnityEngine;

namespace Unicorn
{
    /// <summary>
    /// Nếu cần gọi ads thì dùng class này.
    /// </summary>
    public static class UnicornAdManager
    {
        public static void Init()
        {
            AdManager.Instance.Init();

        }

        public static void LoadBannerAds()
        {
            if (PlayerDataManager.Instance.IsNoAds())
                return;

            AdManager.Instance.ShowBanner();
        }

        public static void ShowInterstitial(string _placement)
        {
            if (PlayerDataManager.Instance.IsNoAds())
                return;

            AdManager.Instance.ShowAd(_placement, AdEnums.ShowType.INTERSTITIAL);
        }

        public static RewardAdStatus ShowAdsReward(Action onRewarded, string placement, bool isAutoLog = true)
        {
#if UNITY_EDITOR
            onRewarded();
            return RewardAdStatus.ShowVideoReward;
#endif


            var adStatus = AdManager.Instance.ShowAdsReward(onRewarded, placement, isAutoLog);
            switch (adStatus)
            {
                case RewardAdStatus.NoInternet:
                    PopupDialogCanvas.Instance.Show("No Internet!");
                    break;
                case RewardAdStatus.ShowVideoReward:
                    break;
                case RewardAdStatus.ShowInterstitialReward:
                    break;
                case RewardAdStatus.NoVideoNoInterstitialReward:
                    PopupDialogCanvas.Instance.Show("No Video!");
                    break;
                default:
                    break;
            }

            return adStatus;
        }
    }
}