using System;
using Castle.Components.DictionaryAdapter.Xml;
using RocketTeam.Sdk.Services.Ads;
using Sirenix.OdinInspector;
using Snowyy;
using System.Collections;
using System.Collections.Generic;
using Castle.Core.Internal;
using Snowyy.Ultilities;
using TMPro;
using Unicorn;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class UiNormalSupplies : UICanvas
    {
        [Title("_RICH TEXT")]
        [SerializeField] private string epicColor;
        [Title("_BUTTONS")]
        [SerializeField] private Button btnVideoNormal;
        [SerializeField] private Button btnVideoAdvance;
        [SerializeField] private Button btnOpenNormal;
        [SerializeField] private Button btnOpenAdvance;
        [Title("_GAMEOBJECT")]
        [SerializeField] private GameObject notiVideoNormal;
        [SerializeField] private GameObject notiVideoAdvance;
        [SerializeField] private GameObject greyVideoNormal;
        [SerializeField] private GameObject greyVideoAdvance;
        [Title("_Text")]
        [SerializeField] private TextMeshProUGUI txtPriceNormal;
        [SerializeField] private TextMeshProUGUI txtPriceAdvance;
        [SerializeField] private TextMeshProUGUI txtOpenProgress;
        [SerializeField] private TextMeshProUGUI txtCooldownNormal;
        [SerializeField] private TextMeshProUGUI txtCooldownAdvance;
        [Title("_DATA")]
        [SerializeField] private D_SurvivorShop data;

        private bool isLoaded = false;

        public void Init()
        {
            var currentCounter = SurvivorShopDataManager.Instance.GetAdvanceCrateEpicCounter();
            txtOpenProgress.text = $"Get <color={epicColor}>Epic</color> equipments in {10 - currentCounter} open";
            if (SnowyyExtensions.CheckConditionDay(SurvivorShopDataManager.Instance.GetTimeRefreshFreeNormalCrate(), 1))
            {
                notiVideoNormal.SetActive(true);
                greyVideoNormal.SetActive(false);
                btnVideoNormal.gameObject.SetActive(true);
            }
            else
            {
                notiVideoNormal.SetActive(false);
                greyVideoNormal.SetActive(true);
                btnVideoNormal.gameObject.SetActive(false);
            }

            if (SnowyyExtensions.CheckConditionDay(SurvivorShopDataManager.Instance.GetTimeRefreshFreeAdvanceCrate(),
                    2))
            {
                notiVideoAdvance.SetActive(true);
                greyVideoAdvance.SetActive(false);
                btnVideoAdvance.gameObject.SetActive(true);
            }
            else
            {
                notiVideoAdvance.SetActive(false);
                greyVideoAdvance.SetActive(true);
                btnVideoAdvance.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            isLoaded = false;
            btnVideoNormal.onClick.AddListener(OnClickBtnVideoNormal);
            btnVideoAdvance.onClick.AddListener(OnClickBtnVideoAdvance);
            btnOpenNormal.onClick.AddListener(OnClickBtnOpenNormal);
            btnOpenAdvance.onClick.AddListener(OnClickBtnOpenAdvance);
            txtPriceNormal.text = $"X{data.normalCratePrice}";
            txtPriceAdvance.text = $"X{data.advanceCratePrice}";
        }

        private void Update()
        {
            var currentNormalDate = SurvivorShopDataManager.Instance.GetTimeRefreshFreeNormalCrate();
            var currentAdvanceDate = SurvivorShopDataManager.Instance.GetTimeRefreshFreeAdvanceCrate();
            if (currentNormalDate.Equals("") || currentAdvanceDate.Equals(""))
            {
                return;
            }

            var currentNormalDateTime = DateTime.Parse(currentNormalDate);
            var currentAdvanceDateTime = DateTime.Parse(currentAdvanceDate);

            DateTime timeNow = DateTime.Now;
            DateTime finalTimeNormal = new DateTime(currentNormalDateTime.Year, currentNormalDateTime.Month,
                currentNormalDateTime.AddDays(1).Day, 0, 0, 0);
            DateTime finalTimeAdvance = new DateTime(currentAdvanceDateTime.Year, currentAdvanceDateTime.Month,
                currentAdvanceDateTime.AddDays(2).Day, 0, 0, 0);
            long tickFinalTimeNormal = finalTimeNormal.Ticks;
            long tickFinalTimeAdvance = finalTimeAdvance.Ticks;
            long tickTimeNow = timeNow.Ticks;

            long elapsedTimeNormal = tickFinalTimeNormal - tickTimeNow;
            long elaspedTimeAdvance = tickFinalTimeAdvance - tickTimeNow;
            TimeSpan cdNormal = TimeSpan.FromTicks(elapsedTimeNormal);
            TimeSpan cdAdvance = TimeSpan.FromTicks(elaspedTimeAdvance);

            txtCooldownNormal.text = $"{cdNormal.Hours}h {cdNormal.Minutes}m {cdNormal.Seconds}s";
            txtCooldownAdvance.text = $"{cdAdvance.Hours + cdAdvance.Days * 24}h {cdAdvance.Minutes}m {cdAdvance.Seconds}s";
            
            if (SnowyyExtensions.CheckConditionDay(currentNormalDate, 1) ||
                SnowyyExtensions.CheckConditionDay(currentAdvanceDate, 2))
            {
                Init();
            }
        }

        private void RollNormal()
        {
            GameManager.Instance.UiController.UiSurvivorShop.UiOpenCrate.InitSingle(CrateType
                .NORMAL);
        }

        private void RollAdvance()
        {
            GameManager.Instance.UiController.UiSurvivorShop.UiOpenCrate.InitSingle(CrateType
                .ADVANCE);
        }

        private void OnRewardVideoNormal(int x = 0)
        {
            SurvivorShopDataManager.Instance.SetTimeRefreshFreeNormalCrate(DateTime.Now.ToString());
            RollNormal();
        }

        private void OnRewardVideoAdvance(int x = 0)
        {
            SurvivorShopDataManager.Instance.SetTimeRefreshFreeAdvanceCrate(DateTime.Now.ToString());
            RollAdvance();
        }

        private void OnClickBtnVideoNormal()
        {
            UnicornAdManager.ShowAdsReward(() => OnRewardVideoNormal(), StringHelper.REWARD_VIDEO_CRATE_NORMAL);
        }

        private void OnClickBtnVideoAdvance()
        {
            UnicornAdManager.ShowAdsReward(()=> OnRewardVideoAdvance(), StringHelper.REWARD_VIDEO_CRATE_ADVANCE);
        }

        private void OnClickBtnOpenNormal()
        {
            SoundManager.Instance.PlaySoundButton();
            var currentGem = GameManager.Instance.Profile.GetGem();
            if (currentGem < data.normalCratePrice)
            {
                //TODO: POP UP NOT ENOUGH GEM
                GameManager.Instance.UiController.PopupNotEnough("NOT ENOUGH GEM");
                return;
            }

            GameManager.Instance.Profile.AddGem(-data.normalCratePrice, "gem_open_normal_crate");
            RollNormal();
        }

        private void OnClickBtnOpenAdvance()
        {
            SoundManager.Instance.PlaySoundButton();
            var currentGem = GameManager.Instance.Profile.GetGem();
            if (currentGem < data.advanceCratePrice)
            {
                //TODO: POP UP NOT ENOUGH GEM
                GameManager.Instance.UiController.PopupNotEnough("NOT ENOUGH GEM");
                return;
            }

            GameManager.Instance.Profile.AddGem(-data.advanceCratePrice, "gem_open_advance_crate");
            RollAdvance();
        }
    }
}