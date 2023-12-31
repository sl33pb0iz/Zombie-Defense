using TMPro;
using Unicorn;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.UI
{
    public class PopupRewardEndGame : UICanvas
    {
        [SerializeField] private Image imgIcon;
        [SerializeField] private Button btnVideo;
        [SerializeField] private Button btnClose;
        [SerializeField] private Button btnClaim;

        [SerializeField] private TextMeshProUGUI txtCoinReplace;
        private RewardEndGame _reward;
        [SerializeField] private GameObject objParentRewards;

        private void Start()
        {
            btnClose.onClick.AddListener(OnClickBtnClose);
            btnVideo.onClick.AddListener(OnClickBtnVideo);
            btnClaim.onClick.AddListener(OnClickBtnClose);
        }

        public void Init(RewardEndGame reward, TypeDialogReward type)
        {

            _reward = reward;
            objParentRewards.SetActive(false);
            txtCoinReplace.gameObject.SetActive(false);
            var playerData = GameManager.Instance.PlayerDataManager;
            if (PlayerDataManager.Instance.GetUnlockSkin(reward.Type, reward.Id))
            {
                imgIcon.sprite = playerData.DataTexture.IconCoin;
                txtCoinReplace.text = $"+{reward.NumberCoinReplace}";
                txtCoinReplace.gameObject.SetActive(true);
            }
            else
            {
                imgIcon.sprite = playerData.DataTextureSkin.GetIcon(reward.Type, reward.Id);
            }

            imgIcon.SetNativeSize();

            switch (type)
            {
                case TypeDialogReward.LUCKY_WHEEL:
                    {
                        btnClaim.gameObject.SetActive(true);
                        btnVideo.gameObject.SetActive(false);
                        btnClose.gameObject.SetActive(false);

                        txtCoinReplace.gameObject.SetActive(false);
                    }
                    break;
                case TypeDialogReward.END_GAME:
                    {
                        btnClaim.gameObject.SetActive(false);
                        btnVideo.gameObject.SetActive(true);
                        btnClose.gameObject.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnClickBtnVideo()
        {
            UnicornAdManager.ShowAdsReward(OnRewardedVideo, Helper.video_reward_end_game);
            SoundManager.Instance.PlaySoundButton();
        }

        private void OnClickBtnClose()
        {
            bool isNextReward = RocketRemoteConfig.GetBoolConfig("next_reward_end_game_user_lose_it", true);
            if (isNextReward)
            {
                SetupNextReward();
            }

            OnClosePressed();

            SoundManager.Instance.PlaySoundButton();
        }

        private void OnRewardedVideo()
        {
            if (!isShow)
                return;

            if (PlayerDataManager.Instance.GetUnlockSkin(_reward.Type, _reward.Id))
            {
                GameManager.Instance.Profile.AddGold(_reward.NumberCoinReplace, Helper.video_reward_end_game);
            }
            else
            {
                PlayerDataManager.Instance.SetUnlockSkin(_reward.Type, _reward.Id);
                PlayerDataManager.Instance.SetIdEquipSkin(_reward.Type, _reward.Id);
            }

            SetupNextReward();
            OnBackPressed();
        }

        public void ActiveReward()
        {
            objParentRewards.SetActive(true);
            SoundManager.Instance.PlaySoundReward();

        }

        private void SetupNextReward()
        {
            var indexReward = PlayerDataManager.Instance.GetCurrentIndexRewardEndGame();
            PlayerDataManager.Instance.SetProcessReceiveRewardEndGame(0);
            indexReward++;
            PlayerDataManager.Instance.SetCurrentIndexRewardEndGame(indexReward);
        }
    }

}