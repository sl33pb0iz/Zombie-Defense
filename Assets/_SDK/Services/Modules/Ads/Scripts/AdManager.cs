using UnityEngine;
using System;

public enum RewardAdStatus
{
    NoInternet,
    ShowVideoReward,
    ShowInterstitialReward,
    NoVideoNoInterstitialReward
}

namespace Unicorn
{
    public class AdManager : MonoBehaviour
    {
        public static AdManager Instance { get; set; }

        private IMediationManager Mediation;

        public Action onLoaded { get; set; }
        public Action<string> onFailedToLoad { get; set; }
        public Action onOpening { get; set; }
        public Action onClosed { get; set; }
        public Action onLeavingApplication { get; set; }
        public Action onGetReward { get; set; }

        void Awake()
        {
            if (Instance == null)
            {

                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                if (this != Instance)
                {
                    Destroy(gameObject);
                }
            }
        }


        public void Init()
        {
            GameObject ad = this.transform.GetChild(0).gameObject;
            Mediation = ad.GetComponent<IMediationManager>();

            Mediation.Init();
        }



        private void RegisterInterstitialListener(Action onOpened, Action onClosed, Action onLeavingApplication,
            Action onGetReward)
        {
            this.onOpening = onOpened;
            this.onClosed = onClosed;
            this.onLeavingApplication = onLeavingApplication;
            this.onGetReward = onGetReward;
        }

        public bool IsInterstitialLoaded(AdEnums.ShowType type)
        {
            switch (type)
            {
                case AdEnums.ShowType.NO_AD:
                    if (onClosed != null)
                    {
                        onClosed();
                    }

                    return true;

                case AdEnums.ShowType.INTERSTITIAL:
                    if (!Mediation.IsLoadInterstitial())
                    {
                        Mediation.LoadInterstitial();

                        return false;
                    }
                    else
                    {
                        return true;
                    }

                case AdEnums.ShowType.VIDEO_REWARD:

                    if (!Mediation.IsLoadRewardedAd())
                    {
                        Mediation.LoadRewardedAd();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    return false;
            }
        }

        public bool ShowAd(string _placement, AdEnums.ShowType type)
        {
            if (!IsInterstitialLoaded(type))
            {
                if (type == AdEnums.ShowType.INTERSTITIAL)
                {
                    Analytics.LogEventByName("Monetize_no_interstitial");
                }
                return false;
            }

            switch (type)
            {
                case AdEnums.ShowType.NO_AD:

                    return true;

                case AdEnums.ShowType.INTERSTITIAL:
                    {
                        Mediation.ShowInterstitial(_placement);
                        AppOpenAdManager.Instance.IsShowAds = false;
                        return true;
                    }


                case AdEnums.ShowType.VIDEO_REWARD:
                    {
                        Mediation.ShowRewardedAd(_placement);
                        AppOpenAdManager.Instance.IsShowAds = false;
                        return true;
                    }

                default:
                    return false;
            }
        }

        public bool ShowAd(string _placement, AdEnums.ShowType type, Action onOpened = null, Action onClosed = null,
            Action onLeavingApplication = null, Action onGetReward = null)
        {
            RegisterInterstitialListener(onOpened, onClosed, onLeavingApplication,
                onGetReward);

            return ShowAd(_placement, type);
        }

        internal bool IsReady()
        {
            var result = Mediation.IsLoadRewardedAd();
            if (!result)
            {
                Mediation.LoadInterstitial();
            }

            return result;

        }

        public RewardAdStatus ShowAdsReward(Action OnRewarded, string _placement, bool isAutoLog = true)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {

                if (isAutoLog)
                {
                    Analytics.LogEventByName("Monetize_reward_no_internet");
                    Analytics.LogEventByName("Monetize_interstitial_no_internet");
                }
                return RewardAdStatus.NoInternet;
            }

            if (IsReady())
            {


                ShowAd(_placement, AdEnums.ShowType.VIDEO_REWARD, null, null, null, () =>
                {
                    if (OnRewarded != null)
                    {
                        Analytics.LogEventWatchVideo(_placement);
                        OnRewarded();

                    }

                }
                );

                return RewardAdStatus.ShowVideoReward;
            }
            else
            {
                if (isAutoLog)
                {
                    Analytics.LogEventByName("Monetize_no_reward");
                }


                if (Mediation.IsLoadInterstitial())
                {
                    Mediation.SetInterBackup();
                    ShowAd(_placement, AdEnums.ShowType.INTERSTITIAL, null, null, null, () =>
                    {
                        if (OnRewarded != null)
                        {
                            OnRewarded();

                        }

                    });

                    return RewardAdStatus.ShowInterstitialReward;
                }
                else
                {
                    if (isAutoLog)
                    {
                        Analytics.LogEventByName("Monetize_no_interstitial");
                        Analytics.LogEventByName("Monetize_no_reward_no_interstitial");
                    }
                    return RewardAdStatus.NoVideoNoInterstitialReward;
                }

            }
        }


        #region Banner

        public bool ShowBanner()
        {
            try
            {
                return Mediation.ShowBanner();
            }
            catch (Exception ex)
            {
                DebugCustom.Log(ex);
                return false;
            }
        }



        public bool HideBanner()
        {
            Mediation.HideBanner();
            return true;
        }


        #endregion
    }
}