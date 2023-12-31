using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spicyy.System;
using Unicorn;
using Unicorn.Utilities;
using TMPro;
using UnityEngine.UI;

namespace Spicyy.UI
{
    public class UpgradePlayerStatsController : MonoBehaviour
    {
        public PlayerUpgradeStatEvent.UpgradeStatType UpgradeStatType;
        public List<Button> buttonUpgrades;
        public TextMeshProUGUI playerLevelText;
        public TextMeshProUGUI costUpgrade;
        public Image costImage;

        public Sprite[] conditionImage;
        public Sprite[] buttonImage;

        private PlayerDataManager PlayerDataManager => PlayerDataManager.Instance;

        List<PlayerStatBonus> m_Stats;

        private void Awake()
        {
            m_Stats = PlayerDataManager.DataPlayerStat.dictStatTemplates[UpgradeStatType];
        }
        private void Start()
        {
            ShowCurrentInfo();
            foreach (var button in buttonUpgrades)
            {
                button.onClick.AddListener(() => { OnUpgrade();});
            }
        }

        protected void OnUpgrade()
        {
            Events.PlayerUpgradeStatEvent.upgradeType = UpgradeStatType;
            UpgradeStat();
            EventManager.Broadcast(Events.PlayerUpgradeStatEvent);
        }


        public void ShowCurrentInfo()
        {
            playerLevelText.text = "LV." + PlayerDataManager.Instance.DataPlayerStat.GetPlayerStatLevel(UpgradeStatType).ToString();

            int curLevel = PlayerDataManager.Instance.DataPlayerStat.GetPlayerStatLevel(UpgradeStatType);


            if (m_Stats[curLevel].cost < PlayerDataManager.GetGold())
            {
                costImage.sprite = conditionImage[0];
                buttonUpgrades[0].image.sprite = buttonImage[0];
                costUpgrade.text = m_Stats[curLevel].cost.ToString();
            }
            else
            {
                costImage.sprite = conditionImage[1];
                buttonUpgrades[0].image.sprite = buttonImage[1];
                costUpgrade.text = "FREE";
            }
        }

        public void UpgradeStat()
        {
            SoundManager.Instance.PlaySoundButton();
            int currentlevel = PlayerDataManager.Instance.DataPlayerStat.GetPlayerStatLevel(UpgradeStatType);

            if (currentlevel >= m_Stats.Count - 1)
            {
                playerLevelText.text = "LV.MAX";
                costUpgrade.text = "MAX";
                return;
            }

            if (m_Stats[currentlevel].cost <= PlayerDataManager.GetGold())
            {
                GameManager.Instance.Profile.AddGold(-m_Stats[currentlevel].cost, "");
                Upgrade();
            }
            else
            {
                UnicornAdManager.ShowAdsReward(() => Upgrade(), Helper.video_upgrade_player);
            }

            void Upgrade()
            {
                currentlevel += 1;
                PlayerDataManager.Instance.DataPlayerStat.SetPlayerStatLevel(UpgradeStatType, currentlevel);
            }


            ShowCurrentInfo();
        }
    }

}
