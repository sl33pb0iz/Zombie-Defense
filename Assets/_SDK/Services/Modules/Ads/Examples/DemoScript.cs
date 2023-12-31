using UnityEngine;
using UnityEngine.UI;
using System;
using RocketTeam.Sdk.Services.Manager;

namespace RocketTeam.Sdk.Services.Ads.Test
{
    public class DemoScript : MonoBehaviour
    {
        public Text inputPopupId;

        private Action<int> onAdReward;

        //public BubbleAdManager bubbleAdManager;

        void Start()
        {
            onAdReward = delegate (int reward) { OnAdReward(reward); };
            SdkManager.Instance.AdsManager.RegisterInterstitialListener(null, null, null, onAdReward);
            SdkManager.Instance.MoreGamesManager.RegisterListener(null, null, null, null, null, onAdReward);

            // init id

            LoadInterstitialAd(1);
            LoadInterstitialAd(2);
            LoadInterstitialAd(3);
            LoadInterstitialAd(4);

            PlayerPrefs.DeleteAll();
        }

        //void Update() {
        //    if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //        bubbleAdManager.LoadBubbleAd();
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha2))
        //    {
        //        bubbleAdManager.ShowBubbleAd();
        //    }
        //}

        #region Interstitial
        public void LoadInterstitialAd(int id)
        {
            Debug.Log("LoadInterstitialAd");
            SdkManager.Instance.AdsManager.LoadInterstitial(id);
        }

        public void ShowInterstitialAd(int id)
        {
            Debug.Log("ShowInterstitialAd");
            SdkManager.Instance.AdsManager.ShowInterstitial("",id);
        }



        private void OnAdOpening()
        {
            Debug.Log("OnAdOpening ");
        }

        private void OnAdClosed()
        {
            Debug.Log("OnAdClosed ");
        }

        private void OnAdReward(int reward)
        {
            Debug.Log("OnAdReward " + reward);
        }
        #endregion

        #region MoreGame
        public void LoadAdsMoreGame()
        {
            Debug.Log("MoreGame_LoadAds");
            SdkManager.Instance.MoreGamesManager.LoadMoreGames();
        }

        public void ShowMoreGame()
        {
            Debug.Log("MoreGame_ShowAds");
            SdkManager.Instance.MoreGamesManager.ShowMoreGames();
        }
        #endregion

        #region BubbleAd
        public void LoadBubbleAd()
        {
            Debug.Log("LoadBubbleAd");
            SdkManager.Instance.BubbleAdManager.LoadBubbleAd();
        }

        public void ShowBubbleAd()
        {
            Debug.Log("ShowBubbleAd");
            SdkManager.Instance.BubbleAdManager.ShowBubbleAd();
        }

        public void HideBubbleAd()
        {
            Debug.Log("HideBubbleAd");
            SdkManager.Instance.BubbleAdManager.HideBubbleAd();
        }
        #endregion

        #region Banner
        public void ShowBanner()
        {
            Debug.Log("Show banner");
            //AdManager.Instance.ShowBanner();
        }

        //public void ShowRewardAd()
        //{
        //    Debug.Log("Show reward ad");
        //    AdManager.Instance.ShowRewardVideo();
        //}
        #endregion
    }
}
