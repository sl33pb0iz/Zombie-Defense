using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.UI
{
    public class ItemChest : MonoBehaviour
    {
       /* [SerializeField] private Button btnChest;
        [SerializeField] private Image imgIconReward;
        [SerializeField] private GameObject objRewardCoin;
        [SerializeField] private TextMeshProUGUI txtAmountCoin;
        private PopupChestKey popupChestKey;
        private RewardEndGame _reward;

        // Start is called before the first frame update
        void Start()
        {
            btnChest.onClick.AddListener(OnClickBtnChest);
        }

        [Sirenix.OdinInspector.Button]
        void lit()
        {
            txtAmountCoin = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        public void Init(PopupChestKey _popupChestKey, RewardEndGame reward)
        {
            _reward = reward;
            popupChestKey = _popupChestKey;
            btnChest.gameObject.SetActive(true);
            objRewardCoin.SetActive(false);
            popupChestKey.IsOpenPrize = false;
            txtAmountCoin.gameObject.SetActive(false);
            imgIconReward.gameObject.SetActive(false);
        }

        private void OnClickBtnChest()
        {
            if (GameManager.Instance.Profile.GetKey() <= 0)
            {
                PopupDialogCanvas.Instance.Show("Not Enough Key");
                return;
            }


            SoundManager.Instance.PlaySoundReward();


            btnChest.gameObject.SetActive(false);
            if (popupChestKey.NumberWatchVideo >= 1 && !popupChestKey.IsOpenPrize)
            {

                objRewardCoin.SetActive(false);
                popupChestKey.IsOpenPrize = true;
                if (PlayerDataManager.Instance.GetUnlockSkin(_reward.Type, _reward.Id))
                {
                    imgIconReward.sprite = GameManager.Instance.PlayerDataManager.DataTexture.IconCoin;
                    txtAmountCoin.text = string.Format("+{0}", _reward.NumberCoinReplace);
                    txtAmountCoin.gameObject.SetActive(true);
                    GameManager.Instance.Profile.AddGold(_reward.NumberCoinReplace, Helper.video_reward_chest_key);
                }
                else
                {
                    txtAmountCoin.gameObject.SetActive(false);
                    imgIconReward.sprite = PlayerDataManager.Instance.DataTextureSkin.GetIcon(_reward.Type, _reward.Id);
                    PlayerDataManager.Instance.SetUnlockSkin(_reward.Type, _reward.Id);
                }

                imgIconReward.gameObject.SetActive(true);
                imgIconReward.SetNativeSize();

                var indexReward = PlayerDataManager.Instance.GetCurrentIndexRewardEndGame();
                //GameManager.Instance.PlayerDataManager.SetProcessReceiveRewardEndGame(0);
                indexReward++;
                PlayerDataManager.Instance.SetCurrentIndexRewardEndGame(indexReward);
            }
            else
            {
                int gold = Helper.GetRandomGoldReward();
                objRewardCoin.SetActive(true);
                txtAmountCoin.text = string.Format("+{0}", gold);
                txtAmountCoin.gameObject.SetActive(true);
                imgIconReward.gameObject.SetActive(false);

                GameManager.Instance.Profile.AddGold(gold, Helper.video_reward_chest_key);
            }

            GameManager.Instance.Profile.AddKey(-1, Helper.video_reward_chest_key);

            popupChestKey.SetLayoutKey();
        }*/
    }

}