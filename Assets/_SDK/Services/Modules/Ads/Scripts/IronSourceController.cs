
using AppsFlyerSDK;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;

public class IronSourceController : MonoBehaviour, IMediationManager
{
    private bool isCompeleteAds = false;
    private bool isInterBackup = false;

    private const string appKey = "1a7ac56f5";

    public void Init()
    {

        IronSource.Agent.shouldTrackNetworkState(true);
        IronSourceConfig.Instance.setClientSideCallbacks(true);
        //IronSource.Agent.setUserId(AppsFlyer.getAppsFlyerId());

        if (PlayerPrefs.GetInt("agreeGDPR") == 1)
            IronSource.Agent.setConsent(true);

        else
            IronSource.Agent.setConsent(false);
        //IronSource.Agent.setMetaData("do_not_sell", "true");

        InitBannerAd();
        InitRewardedVideoAd();
        InitInterstitialAd();

#if GAME_ROCKET
        IronSource.Agent.setAdaptersDebug(true);
        IronSource.Agent.validateIntegration();
#endif

        //Debug.Log("unity-script: unity version" + IronSource.unityVersion());

        // SDK init
        //Debug.Log("unity-script: IronSource.Agent.init");
        IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);
    }

    void OnEnable()
    {
        //Add Init Event
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;


        //Add ImpressionSuccess Event
        IronSourceEvents.onImpressionDataReadyEvent += ImpressionSuccessEvent;
        //IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;

    }

    private void InitBannerAd()
    {
        if (PlayerDataManager.Instance.IsNoAds())
            return;

        IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
        IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
        IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
        IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
        IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
        IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;

        //IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
    }

    private void InitInterstitialAd()
    {
        if (PlayerDataManager.Instance.IsNoAds())
            return;

        // Add Interstitial Events
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        //LoadInterstitial();
    }

    private void InitRewardedVideoAd()
    {
        //Add Rewarded Video Events
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
    }

    public void HideBanner()
    {
        IronSource.Agent.hideBanner();
    }



    public bool IsLoadInterstitial()
    {
        return IronSource.Agent.isInterstitialReady();
    }

    public bool IsLoadRewardedAd()
    {
        return IronSource.Agent.isRewardedVideoAvailable();
    }

    public void LoadInterstitial()
    {
        IronSource.Agent.loadInterstitial();
    }

    public void LoadRewardedAd()
    {

    }

    public bool ShowBanner()
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        return true;
    }

    public void ShowInterstitial(string placement)
    {
        IronSource.Agent.showInterstitial(placement);
    }

    public void ShowRewardedAd(string placement)
    {
        IronSource.Agent.showRewardedVideo(placement);
    }



    #region Init callback handlers

    void SdkInitializationCompletedEvent()
    {
        Debug.Log("unity-script: I got SdkInitializationCompletedEvent");

        if (PlayerDataManager.Instance.IsNoAds())
            return;


        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        LoadInterstitial();
    }

    #endregion

    #region RewardedAd callback handlers

    void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
    {
        Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
    }

    void RewardedVideoAdOpenedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
    }

    void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());

        isCompeleteAds = true;
        if (AdManager.Instance.onGetReward != null)
            AdManager.Instance.onGetReward();

    }

    void RewardedVideoAdClosedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
    }

    void RewardedVideoAdStartedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
    }

    void RewardedVideoAdEndedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
    }

    void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + ssp.getRewardName());
    }

    /************* RewardedVideo DemandOnly Delegates *************/

    void RewardedVideoAdLoadedDemandOnlyEvent(string instanceId)
    {

        Debug.Log("unity-script: I got RewardedVideoAdLoadedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {

        Debug.Log("unity-script: I got RewardedVideoAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdOpenedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got RewardedVideoAdOpenedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdRewardedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got RewardedVideoAdRewardedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdClosedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClosedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdClickedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClickedDemandOnlyEvent for instance: " + instanceId);
    }


    #endregion



    #region Interstitial callback handlers

    void InterstitialAdReadyEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdReadyEvent");
    }

    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
        //StartCoroutine(WaitForLoadInterstitialAgain(1f));
        IronSource.Agent.loadInterstitial();
    }

    private IEnumerator WaitForLoadInterstitialAgain(float time)
    {
        yield return new WaitForSeconds(time);
        IronSource.Agent.loadInterstitial();
    }

    void InterstitialAdShowSucceededEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
    }

    void InterstitialAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void InterstitialAdClickedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClickedEvent");
    }

    void InterstitialAdOpenedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
    }

    void InterstitialAdClosedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClosedEvent");
        if (isInterBackup)
        {
            isInterBackup = false;
            if (AdManager.Instance.onGetReward != null)
                AdManager.Instance.onGetReward();
        }

        LoadInterstitial();
    }

    /************* Interstitial DemandOnly Delegates *************/

    void InterstitialAdReadyDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdReadyDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", error code: " + error.getCode() + ",error description : " + error.getDescription());
    }

    void InterstitialAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", error code :  " + error.getCode() + ",error description : " + error.getDescription());
    }

    void InterstitialAdClickedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdClickedDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdOpenedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdOpenedDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdClosedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdClosedDemandOnlyEvent for instance: " + instanceId);
    }




    #endregion

    #region Banner callback handlers

    void BannerAdLoadedEvent()
    {
        Debug.Log("unity-script: I got BannerAdLoadedEvent");
    }

    void BannerAdLoadFailedEvent(IronSourceError error)
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        //StartCoroutine(WaitForLoadBannerAgain(1));
        Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
    }

    void BannerAdClickedEvent()
    {
        Debug.Log("unity-script: I got BannerAdClickedEvent");
    }

    void BannerAdScreenPresentedEvent()
    {
        Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
    }

    void BannerAdScreenDismissedEvent()
    {
        Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
    }

    void BannerAdLeftApplicationEvent()
    {
        Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
    }

    private IEnumerator WaitForLoadBannerAgain(float time)
    {
        yield return new WaitForSeconds(time);
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
    }

    #endregion


    #region Offerwall callback handlers

    void OfferwallOpenedEvent()
    {
        Debug.Log("I got OfferwallOpenedEvent");
    }

    void OfferwallClosedEvent()
    {
        Debug.Log("I got OfferwallClosedEvent");
    }

    void OfferwallShowFailedEvent(IronSourceError error)
    {
        Debug.Log("I got OfferwallShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void OfferwallAdCreditedEvent(Dictionary<string, object> dict)
    {
        Debug.Log("I got OfferwallAdCreditedEvent, current credits = " + dict["credits"] + " totalCredits = " + dict["totalCredits"]);

    }

    void GetOfferwallCreditsFailedEvent(IronSourceError error)
    {
        Debug.Log("I got GetOfferwallCreditsFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void OfferwallAvailableEvent(bool canShowOfferwal)
    {
        Debug.Log("I got OfferwallAvailableEvent, value = " + canShowOfferwal);

    }

    #endregion

    #region ImpressionSuccess callback handler

    void ImpressionSuccessEvent(IronSourceImpressionData impressionData)
    {
        Debug.Log("unity - script: I got ImpressionSuccessEvent ToString(): " + impressionData.ToString());
        Debug.Log("unity - script: I got ImpressionSuccessEvent allData: " + impressionData.allData);

        if (impressionData != null && !string.IsNullOrEmpty(impressionData.adNetwork))
        {
            AnalyticsRevenueAds.SendEvent(impressionData);
        }
    }



    #endregion

    void OnApplicationPause(bool isPaused)
    {
        Debug.Log("unity-script: OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }

    public void SetInterBackup()
    {
        isInterBackup = true;
    }
}
