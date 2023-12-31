using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;

public class AppOpenAdManager
{
#if UNITY_ANDROID || UNITY_EDITOR
    private const string AD_UNIT_ID_1 = "ca-app-pub-6336405384015455/8052918982";
    private const string AD_UNIT_ID_2 = "ca-app-pub-6336405384015455/7861347294 ";
    private const string AD_UNIT_ID_3 = "ca-app-pub-6336405384015455/7186559871";

#elif UNITY_IOS
    private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/5662855259";
#else
    private const string AD_UNIT_ID = "unexpected_platform";
#endif

    private static AppOpenAdManager instance;

    private AppOpenAd ad;

    private bool isShowingAd = false;
    private int numberRequest = 0;
    private bool isFirtShow = false;
    public bool IsShowAds = true;

    public static AppOpenAdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }

    private bool IsAdAvailable
    {
        get
        {
            return ad != null;
        }
    }

    public void LoadAd()
    {
        if (numberRequest == 0)
        {
            LoadAd(AD_UNIT_ID_1);
        }
        else if (numberRequest == 1)
        {
            LoadAd(AD_UNIT_ID_2);
        }
        else if (numberRequest == 2)
        {
            LoadAd(AD_UNIT_ID_3);
        }
    }

    private void LoadAd(string id)
    {
        AdRequest request = new AdRequest.Builder().Build();

        // Load an app open ad for portrait orientation
        AppOpenAd.LoadAd(id, Screen.orientation, request, ((appOpenAd, error) =>
        {
            if (error != null)
            {
                // Handle the error.
                Debug.LogFormat("Failed to load the ad. (reason: {0})", error.LoadAdError.GetMessage());

                numberRequest++;
                LoadAd();

                return;
            }

            // App open ad is loaded.
            ad = appOpenAd;
            if (!isFirtShow)
            {
                ShowAdIfAvailable();
            }
        }));
    }

    public void ShowAdIfAvailable()
    {
        if (PlayerDataManager.Instance.IsNoAds() || isShowingAd)
        {
            return;
        }

        if (!IsAdAvailable)
        {
            numberRequest = 0;
            LoadAd();

            return;
        }
        isFirtShow = true;

        ad.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
        ad.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
        ad.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
        ad.OnAdDidRecordImpression += HandleAdDidRecordImpression;
        ad.OnPaidEvent += HandlePaidEvent;

        ad.Show();
    }

    private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;

        numberRequest = 0;
        LoadAd();
    }

    private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
    {
        Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;

        numberRequest = 0;
        LoadAd();

    }

    private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Displayed app open ad");
        isShowingAd = true;
    }

    private void HandleAdDidRecordImpression(object sender, EventArgs args)
    {
        Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
                args.AdValue.CurrencyCode, args.AdValue.Value);
    }

}
