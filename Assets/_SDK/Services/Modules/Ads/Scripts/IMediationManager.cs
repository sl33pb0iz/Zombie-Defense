using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Unicorn
{
    public interface IMediationManager
    {
        void Init();

        bool IsLoadInterstitial();
        void LoadInterstitial();
        void ShowInterstitial(string placement);

        bool IsLoadRewardedAd();
        void LoadRewardedAd();
        void ShowRewardedAd(string placement);

        void HideBanner();
        bool ShowBanner();

        void SetInterBackup();
    }
}
