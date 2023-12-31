using DG.Tweening;
using RocketTeam.Sdk.Services.Ads;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.UI
{
    public class LuckyWheel : UICanvas
    {
        public Transform wheel;
        [SerializeField] private Button btnFreeSpin;
        [SerializeField] private Button btnVideoSpin;

        private int slotWheel;
        private Vector3 angleTarget;

        private const int NUMBER_ROTATE_AROUND = 5;
        private const int NUMBER_PIECE_OF_WHEEL = 8;
        private const float TIME_ROTATE = 3f;

        [SerializeField] private List<RewardLuckyWheel> listItemRewards;
        //[SerializeField] private List<ItemFreeReward> listItemFreeSpin;
        [SerializeField] private TextMeshProUGUI txtNumberDaily;

        //[SerializeField] private RectTransform rectPointNumberWatchVideoSpin;
        //[SerializeField] private TextMeshProUGUI txtNumberWatchVideo;
        //[SerializeField] private Image imgBarProcessWatchVideo;
        [SerializeField] private TextMeshProUGUI txtClockFreeSpin;

        private float WidthBar = 755;
        private float totalTimeReset;
        private bool isCoutdown;
        private float _time;

        private void Start()
        {
            btnFreeSpin.onClick.AddListener(OnClickBtnFreeSpin);
            btnVideoSpin.onClick.AddListener(OnClickBtnVideoSpin);

        }
        private void Update()
        {
            if (!isShow && !isCoutdown)
            {
                return;
            }

            _time += Time.unscaledDeltaTime;
            if (_time >= 1)
            {
                totalTimeReset -= 1;

                if (totalTimeReset <= 0)
                {
                    totalTimeReset = 0;
                    isCoutdown = false;
                    btnFreeSpin.interactable = true;
                }
                
                var timeSpan = TimeSpan.FromSeconds(totalTimeReset);
                txtClockFreeSpin.text = Helper.FormatTime(timeSpan.Minutes, timeSpan.Seconds, true);
                _time = 0;
            }
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (isShow)
                Init();
        }

        public void Init()
        {
            isCoutdown = false;
            for (int i = 0; i < GameManager.Instance.PlayerDataManager.DataLuckyWheel.ListDataRewrds.Count; i++)
            {
                listItemRewards[i].Init(GameManager.Instance.PlayerDataManager.DataLuckyWheel.ListDataRewrds[i]);
            }

            btnFreeSpin.interactable = PlayerDataManager.Instance.GetFreeSpin();

            //SetupFreeRewards();
            SetupTimeWaitFreeSpin();

            var timeLogin = PlayerDataManager.Instance.GetTimeLoginSpinVideo();
            var isNewDay = Helper.CheckNewDay(timeLogin, false);
            if (isNewDay)
            {
                PlayerDataManager.Instance.SetNumberWatchDailyVideo(GameManager.Instance.PlayerDataManager.DataLuckyWheel
                    .NumberSpinDaily);
                PlayerDataManager.Instance.SetTimeLoginSpinVideo(DateTime.Now.ToString());
            }

            SetLayoutBtnVideoSpin();
        }

        private void SetupTimeWaitFreeSpin()
        {
            if (!PlayerDataManager.Instance.GetFreeSpin())
            {
                string time = PlayerDataManager.Instance.GetTimeLoginSpinFreeWheel();
                DateTime timeLogin = DateTime.Parse(time);

                long tickTimeNow = DateTime.Now.Ticks;
                long tickTimeOld = timeLogin.Ticks;

                long elapsedTicks = tickTimeNow - tickTimeOld;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

                float totalSeconds = (float)elapsedSpan.TotalSeconds;

                totalTimeReset = 30 * 60 - totalSeconds;
                if (totalTimeReset <= 0)
                {
                    totalTimeReset = 0;
                    /*objClockFreeSpin.SetActive(false);*/
                    btnVideoSpin.interactable = true;
                }
                else
                {
                    isCoutdown = true;
                    var timeSpan = TimeSpan.FromSeconds(totalTimeReset);
                    txtClockFreeSpin.text = Helper.FormatTime(timeSpan.Minutes, timeSpan.Seconds, true);

                    /*objClockFreeSpin.SetActive(true);*/
                }

            }
        }
        private void SetLayoutBtnVideoSpin()
        {
            int numberWatchDailyVideo = GameManager.Instance.PlayerDataManager.GetNumberWatchDailyVideo();
            txtNumberDaily.text = numberWatchDailyVideo.ToString();

            btnVideoSpin.interactable = numberWatchDailyVideo > 0 ? true : false;
        }
        //private void SetupFreeRewards()
        //{
        //    for (int i = 0; i < listItemFreeSpin.Count; i++)
        //    {
        //        var reward = GameManager.Instance.PlayerDataManager.DataLuckyWheel.ListDataReceiveFrees[i];
        //        listItemFreeSpin[i].Init(reward);
        //    }

        //    int numberWatchVideo = PlayerDataManager.Instance.GetNumberWatchVideoSpin();
        //    txtNumberWatchVideo.text = numberWatchVideo.ToString();


        //    var ratio = (float)numberWatchVideo / 10f;
        //    ratio = ratio > 1 ? 1 : ratio;
        //    rectPointNumberWatchVideoSpin.anchoredPosition = new Vector2(ratio * WidthBar, 55);
        //    imgBarProcessWatchVideo.fillAmount = ratio;


        //}
        public void StartRotateWheel(bool isFree = true)
        {
            SoundManager.Instance.PlaySoundSpin();

            btnFreeSpin.interactable = false;
            btnVideoSpin.interactable = false;

            slotWheel = GameManager.Instance.PlayerDataManager.DataLuckyWheel.GetIdLuckyWheel();

            angleTarget = CalculateAngle(NUMBER_PIECE_OF_WHEEL - slotWheel);

            wheel.DORotate(angleTarget, TIME_ROTATE)
                .SetEase(Ease.InOutCubic)
                .OnComplete(OnCompleteRotate);

            if (!isFree)
            {
                int numberWatchDailyVideo = GameManager.Instance.PlayerDataManager.GetNumberWatchDailyVideo();
                numberWatchDailyVideo--;
                if (numberWatchDailyVideo <= 0)
                    numberWatchDailyVideo = 0;

                PlayerDataManager.Instance.SetNumberWatchDailyVideo(numberWatchDailyVideo);
            }

            int numberWatchVideo = PlayerDataManager.Instance.GetNumberWatchVideoSpin();
            numberWatchVideo++;
            PlayerDataManager.Instance.SetNumberWatchVideoSpin(numberWatchVideo);
        }
        private Vector3 CalculateAngle(int indexPrize)
        {
            return new Vector3(0, 0, -360 * NUMBER_ROTATE_AROUND - (indexPrize) * (360 / NUMBER_PIECE_OF_WHEEL));
        }
        private void OnCompleteRotate()
        {
            // complete wheel
            var playerData = GameManager.Instance.PlayerDataManager;
            var dataReward = GameManager.Instance.PlayerDataManager.DataLuckyWheel.ListDataRewrds[slotWheel];
            switch (dataReward.Type)
            {
                case TypeGift.GOLD:
                    {
                        GameManager.Instance.Profile.AddGold(dataReward.Amount, "lucky_wheel");

                        SoundManager.Instance.PlaySoundReward();
                    }
                    break;

                case TypeGift.GEM:
                    {
                        GameManager.Instance.Profile.AddGem(dataReward.Amount, "lucky_wheel");

                        SoundManager.Instance.PlaySoundReward();
                    }
                    break;

                case TypeGift.HAT:
                    {
                        if (PlayerDataManager.Instance.GetUnlockSkin(TypeEquipment.Hat, dataReward.IdType))
                        {
                            GameManager.Instance.Profile.AddGold(dataReward.NumberCoinReplace, "lucky_wheel");
                            SoundManager.Instance.PlaySoundReward();
                        }
                        else
                        {


                            RewardEndGame reward = new RewardEndGame();
                            reward.Type = TypeEquipment.Hat;
                            reward.Id = dataReward.IdType;

                            GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                            PlayerDataManager.Instance.SetUnlockSkin(TypeEquipment.Hat, dataReward.IdType);
                        }
                    }
                    break;
                case TypeGift.SKIN:
                    {
                        if (PlayerDataManager.Instance.GetUnlockSkin(TypeEquipment.Skin, dataReward.IdType))
                        {
                            GameManager.Instance.Profile.AddGold(dataReward.NumberCoinReplace, "lucky_wheel");
                            SoundManager.Instance.PlaySoundReward();

                        }
                        else
                        {
                            RewardEndGame reward = new RewardEndGame();
                            reward.Type = TypeEquipment.Skin;
                            reward.Id = dataReward.IdType;

                            GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                            PlayerDataManager.Instance.SetUnlockSkin(TypeEquipment.Skin, dataReward.IdType);
                        }
                    }
                    break;
                case TypeGift.PET:
                    {
                        if (PlayerDataManager.Instance.GetUnlockSkin(TypeEquipment.Pet, dataReward.IdType))
                        {
                            GameManager.Instance.Profile.AddGold(dataReward.NumberCoinReplace, "lucky_wheel");
                            SoundManager.Instance.PlaySoundReward();
                        }
                        else
                        {
                            RewardEndGame reward = new RewardEndGame();
                            reward.Type = TypeEquipment.Pet;
                            reward.Id = dataReward.IdType;

                            GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);
                            PlayerDataManager.Instance.SetUnlockSkin(TypeEquipment.Pet, dataReward.IdType);
                        }
                    }
                    break;
                default:
                    break;
            }
            Init();
        }

        private void OnClickBtnFreeSpin()
        {
            SoundManager.Instance.PlaySoundButton();

            PlayerDataManager.Instance.SetFreeSpin(false);
            PlayerDataManager.Instance.SetTimeLoginSpinFreeWheel(DateTime.Now.ToString());
            StartRotateWheel();

            totalTimeReset = 30 * 60;
            isCoutdown = true;

        }

        private void OnClickBtnVideoSpin()
        {
            SoundManager.Instance.PlaySoundButton();

#if UNITY_EDITOR
            StartRotateWheel(false);
#else
        //AdManager.Instance.ShowAdsReward(OnRewardVideo, Helper.video_reward_lucky_wheel);
        WaitingCanvas.Instance.Show("");
        Observable.FromCoroutine(ActionWatchVideo).Subscribe().AddTo(this.gameObject);
#endif
        }

        private IEnumerator ActionWatchVideo()
        {
            float _tmp1 = 0;
            float _tmp2 = 1;
            RewardAdStatus _rewardAdStatus = RewardAdStatus.NoVideoNoInterstitialReward;
            while (_tmp1 < 2)
            {
                _tmp1 += Time.deltaTime;
                _tmp2 += Time.deltaTime;
                if (_tmp2 > 0.5f)
                {
                    _tmp2 = 0;
                    _rewardAdStatus =
                        AdManager.Instance.ShowAdsReward(OnRewardVideo, Helper.video_reward_lucky_wheel, false);
                    switch (_rewardAdStatus)
                    {
                        case RewardAdStatus.NoInternet:
                            WaitingCanvas.Instance.Hide();
                            PopupDialogCanvas.Instance.Show("No Internet!");

                            Analytics.LogEventByName("Monetize_reward_no_internet");
                            Analytics.LogEventByName("Monetize_interstitial_no_internet");
                            yield break;
                        case RewardAdStatus.NoVideoNoInterstitialReward:
                            break;
                        default:
                            WaitingCanvas.Instance.Hide();
                            yield break;
                    }
                }

                yield return null;
            }

            if (_rewardAdStatus == RewardAdStatus.NoVideoNoInterstitialReward)
            {
                WaitingCanvas.Instance.Hide();
                PopupDialogCanvas.Instance.Show("No Video!");
                Analytics.LogEventByName("Monetize_no_reward");
                Analytics.LogEventByName("Monetize_no_reward_no_interstitial");
            }
        }

        private void OnRewardVideo()
        {
            if (!isShow)
                return;

            StartRotateWheel(false);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            SoundManager.Instance.PlaySoundButton();
        }
    }

}