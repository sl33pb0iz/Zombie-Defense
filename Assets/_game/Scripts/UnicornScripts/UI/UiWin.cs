using System.Collections;
using DG.Tweening;
using TMPro;
using Unicorn;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Unicorn.UI
{
    public class UiWin : UICanvas
    {

        [FoldoutGroup("Button"), SerializeField] private Button btnCollectGem;
        [FoldoutGroup("Button"), SerializeField] private Button btnX3Gem;

        [FoldoutGroup("Text"), SerializeField] private TextMeshProUGUI txtGemBonus;
        [FoldoutGroup("Text"), SerializeField] private TextMeshProUGUI txtGemCollect;

        [FoldoutGroup("Component"), SerializeField] private ForceBar forceBar;

        private int _gem;

        public int Gem
        {
            get => _gem;
            protected set => _gem = value;
        }

        private void Start()
        {
            btnCollectGem.onClick.AddListener(OnClickBtnNoThank);
            btnX3Gem.onClick.AddListener(OnClickBtnX3Coin);
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (!isShow)
            {
                return;
            }
        }

        public virtual void Init(int gem)
        {

            CalculateGem(gem);
            btnX3Gem.gameObject.SetActive(true);
            btnCollectGem.interactable = true;
            int lvlShowRate = RocketRemoteConfig.GetIntConfig("config_show_rate_game", 4);
            if (PlayerPrefs.GetInt("showRate", 0) == 0 && GameManager.Instance.DataLevel.DisplayLevel >= lvlShowRate)
            {
                StartCoroutine(IEShowRateGame());
                PlayerPrefs.SetInt("showRate", 1);
            }

            //StartCoroutine(IEWaitShowFx());

            btnX3Gem.transform.GetChild(0).gameObject.SetActive(true);
            btnX3Gem.transform.GetChild(1).gameObject.SetActive(true);
            btnCollectGem.gameObject.SetActive(true);

        }
        protected virtual void CalculateGem(int gem)
        {
            Gem = gem;
        }

        private void Update()
        {
            var value = forceBar.GetValue();
            txtGemBonus.text = $"+{Gem * value} ";
            txtGemCollect.text = $"COLLECT {Gem}";
        }

        private void OnClickBtnNoThank()
        {

            SoundManager.Instance.PlayFxSound(SoundManager.GameSound.RewardClick);
            btnCollectGem.interactable = false;
            GameManager.Instance.Profile.AddGem(Gem, "end_game");
            UnicornAdManager.ShowInterstitial(Helper.inter_end_game_win);
            StartCoroutine(IEGoLobby());
        }

        private void OnClickBtnX3Coin()
        {
            SoundManager.Instance.PlaySoundButton();
            UnicornAdManager.ShowAdsReward(() => OnRewardVideo(1),Helper.video_reward_end_game);
            return;

        }

        private void OnRewardVideo(int x)
        {
            if (x <= 0 && !isShow)
            {
                return;
            }

            btnX3Gem.gameObject.SetActive(false);
            btnCollectGem.interactable = false;
            forceBar.StopRunning();
            GameManager.Instance.Profile.AddGem(Gem * forceBar.GetValue(), Helper.video_reward_end_game);

            //txtCoin.text = string.Format("+{0}", _gold * 3);
            SoundManager.Instance.PlaySoundReward();

            StartCoroutine(IEGoLobby());
        }


        private IEnumerator IEShowRateGame()
        {
            yield return new WaitForSeconds(0.5f);
            PopupRateGame.Instance.Show();
        }


        private IEnumerator IEGoLobby()
        {
            yield return new WaitForSeconds(0.5f);
            OnBackPressed();
            GameManager.Instance.LoadLevel();
        }
    }
}
