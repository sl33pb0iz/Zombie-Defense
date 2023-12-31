using Sirenix.OdinInspector;
using Snowyy;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class ItemGoldShop : MonoBehaviour
    {
        [FoldoutGroup("_BUTTON"), SerializeField] private Button btnPurchase;

        [FoldoutGroup("_IMAGE"), SerializeField] private Image iconPack;

        [FoldoutGroup("_GAMEOBJECT"), SerializeField] private GameObject freeGo;
        [FoldoutGroup("_GAMEOBJECT"), SerializeField] private GameObject videoGo;
        [FoldoutGroup("_GAMEOBJECT"), SerializeField] private GameObject priceGo;

        [FoldoutGroup("_TEXT"), SerializeField] private TextMeshProUGUI txtAmount;
        [FoldoutGroup("_TEXT"), SerializeField] private TextMeshProUGUI txtPrice;

        private CurrencyType currencyType;
        private int goldToReward;
        private int gemToPay;

        private void Start()
        {
            btnPurchase.onClick.AddListener(OnClickBtnPurchase);
        }

        private void OnClickBtnPurchase()
        {
            SoundManager.Instance.PlaySoundButton();
            switch (currencyType)
            {
                case CurrencyType.FREE:
                    GameManager.Instance.Profile.AddGold(goldToReward, "gold_reward_daily_free");
                    SoundManager.Instance.PlaySoundReward();
                    SurvivorShopDataManager.Instance.SetReceivedFreeGoldDaily(true);
                    break;
                case CurrencyType.GEM:
                    if (!(GameManager.Instance.Profile.GetGem() >= gemToPay))
                    {
                        GameManager.Instance.UiController.PopupNotEnough("NOT ENOUGH GEM");
                        return;
                    }

                    GameManager.Instance.Profile.AddGem(-gemToPay, "gem_pay_gold_shop_" + goldToReward);
                    GameManager.Instance.Profile.AddGold(goldToReward, "gold_reward_daily_gem_" + goldToReward);
                    SoundManager.Instance.PlaySoundReward();
                    break;
                case CurrencyType.VIDEO:
                    UnicornAdManager.ShowAdsReward(() => GrantRewardVideo(0), StringHelper.REWARD_VIDEO_GOLD_DAILY);
                    break;
            }

            GameManager.Instance.UiController.UiSurvivorShop.UiGoldShop.Init();
        }

        private void GrantRewardVideo(int x)
        {
            GameManager.Instance.Profile.AddGold(goldToReward, "gold_reward_daily_video");
            SoundManager.Instance.PlaySoundReward();
        }

        public void Init(int goldAmount, int price, Sprite icon,bool isFree = false, bool isVideo = false)
        {
            gemToPay = price;
            goldToReward = goldAmount;

            iconPack.sprite = icon;
            txtAmount.text = $"{goldAmount}";
            txtPrice.text = $"{price}";

            SetUpButton(isFree, isVideo);
        }

        void SetUpButton(bool isFree, bool isVideo)
        {
            freeGo.SetActive(false);
            videoGo.SetActive(false);
            priceGo.SetActive(false);

            if (isFree)
            {
                currencyType = CurrencyType.FREE;
                freeGo.SetActive(true);
                return;
            }

            if (isVideo)
            {
                currencyType = CurrencyType.VIDEO;
                videoGo.SetActive(true);
                return;
            }

            {
                currencyType = CurrencyType.GEM;
                priceGo.SetActive(true);
            }
            
        }
    }
}