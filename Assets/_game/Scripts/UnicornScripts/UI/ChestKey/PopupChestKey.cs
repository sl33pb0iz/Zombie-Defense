using RocketTeam.Sdk.Services.Ads;
using System.Collections;
using System.Collections.Generic;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.UI
{
    public class PopupChestKey : UICanvas
    {
       /* [SerializeField] private List<GameObject> listObjKeys;
        [SerializeField] private List<ItemChest> listChest;
        [SerializeField] private Image imgIconReward;
        [SerializeField] private Button btnVideo;
        [SerializeField] private Button btnClose;
        [SerializeField] private Button btnDone;
        private RewardEndGame reward;
        public int NumberWatchVideo;

        public bool IsOpenPrize;

        // Start is called before the first frame update
        void Start()
        {
            btnClose.onClick.AddListener(OnClickBtnClose);
            btnVideo.onClick.AddListener(OnClickBtnVideo);
            btnDone.onClick.AddListener(OnClickBtnDone);
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (!isShow)
            {
                GameManager.Instance.UiController.UiMainLobby.ActiveMainLobby();
            }
        }

        public void Init(RewardEndGame _reward)
        {
            SoundManager.Instance.PlaySoundReward();

            IsOpenPrize = false;
            btnDone.gameObject.SetActive(false);
            var playerData = GameManager.Instance.PlayerDataManager;
            reward = _reward;
            NumberWatchVideo = 0;
            btnVideo.gameObject.SetActive(false);
            for (int i = 0; i < listObjKeys.Count; i++)
            {
                listObjKeys[i].SetActive(true);
            }

            if (PlayerDataManager.Instance.GetUnlockSkin(reward.Type, reward.Id))
            {
                imgIconReward.sprite = playerData.DataTexture.IconCoin;
            }
            else
            {
                imgIconReward.sprite = playerData.DataTextureSkin.GetIcon(reward.Type, reward.Id);
            }

            imgIconReward.SetNativeSize();

            for (int i = 0; i < listChest.Count; i++)
            {
                listChest[i].Init(this, _reward);
            }
        }

        public void SetLayoutKey()
        {
            for (int i = 0; i < listObjKeys.Count; i++)
            {
                listObjKeys[i].SetActive(false);
            }

            int key = GameManager.Instance.Profile.GetKey();
            for (int i = 0; i < key; i++)
            {
                listObjKeys[i].SetActive(true);
            }

            if (key <= 0)
            {
                btnVideo.gameObject.SetActive(true);
            }

            if (NumberWatchVideo >= 2)
            {
                btnDone.gameObject.SetActive(true);
                btnVideo.gameObject.SetActive(false);
            }
        }

        private void OnClickBtnVideo()
        {
            RewardAdStatus adStatus =
                AdManager.Instance.ShowAdsReward(() => { OnRewardedVideo(); }, Helper.video_reward_chest_key, true);

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

            SoundManager.Instance.PlaySoundButton();
        }

        private void OnRewardedVideo()
        {
            if (!isShow)
                return;

            GameManager.Instance.Profile.AddKey(3, Helper.video_reward_chest_key);
            for (int i = 0; i < listObjKeys.Count; i++)
            {
                listObjKeys[i].SetActive(true);
            }

            NumberWatchVideo++;


            btnVideo.gameObject.SetActive(false);
        }

        private void OnClickBtnClose()
        {

            var key = GameManager.Instance.Profile.GetKey();
            GameManager.Instance.Profile.AddKey(-key, Helper.video_reward_chest_key);

            OnBackPressed();

            SoundManager.Instance.PlaySoundButton();
        }

        private void OnClickBtnDone()
        {
            OnBackPressed();
            SoundManager.Instance.PlaySoundButton();
        }*/
    }

}