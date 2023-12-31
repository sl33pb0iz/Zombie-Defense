using Sirenix.OdinInspector;
using Snowyy;
using Snowyy.EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class UiSuperSupplies : UICanvas
    {
        [Title("_RICH TEXT")]
        [SerializeField] private string epicColor;
        [SerializeField] private string mythicalColor;
        [Title("_PRICE")]
        [SerializeField] private int singleRoll = 300;
        [SerializeField] private int multiRoll = 2700;
        [Title("_BUTTONS")]
        [SerializeField] private Button btnOpenSingle;
        [SerializeField] private Button btnOpenMultiple;
        [SerializeField] private Button btnRateInfo;
        [Title("_TEXT")]
        [SerializeField] private TextMeshProUGUI txtProgress;

        private void Start()
        {
            btnOpenSingle.onClick.AddListener(OnClickBtnOpenSingle);
            btnOpenMultiple.onClick.AddListener(OnClickBtnOpenMultiple);
            btnRateInfo.onClick.AddListener(OnClickBtnRateInfo);
        }

        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            var currentEpic = SurvivorShopDataManager.Instance.GetSuperCrateEpicCounter();
            var currentMythical = SurvivorShopDataManager.Instance.GetSuperCrateMythicalCounter();
            txtProgress.text =
                $"Get <color={epicColor}>Epic</color> in {10 - currentEpic} open\nGet <color={mythicalColor}>Mythical</color> in {30 - currentMythical} open";
        }

        private void OnClickBtnOpenSingle()
        {
            SoundManager.Instance.PlaySoundButton();
            var currentGem = GameManager.Instance.Profile.GetGem();
            if (currentGem < singleRoll)
            {
                //TODO: POP UP NOT ENOUGH GEM
                GameManager.Instance.UiController.PopupNotEnough("NOT ENOUGH GEM");
                return;
            }

            GameManager.Instance.Profile.AddGem(-singleRoll, "gem_open_single_super_crate");
            GameManager.Instance.UiController.UiSurvivorShop.UiOpenCrate.InitSingle(CrateType.SUPER);
        }

        private void OnClickBtnOpenMultiple()
        {
            SoundManager.Instance.PlaySoundButton();
            var currentGem = GameManager.Instance.Profile.GetGem();
            if (currentGem < multiRoll)
            {
                //TODO: POP UP NOT ENOUGH GEM
                GameManager.Instance.UiController.PopupNotEnough("NOT ENOUGH GEM");
                return;
            }

            GameManager.Instance.Profile.AddGem(-multiRoll, "gem_open_multi_super_crate");
            GameManager.Instance.UiController.UiSurvivorShop.UiOpenCrate.SetMultiCounter(0);
            GameManager.Instance.UiController.UiSurvivorShop.UiOpenCrate.InitMulti(CrateType.SUPER);
        }

        private void OnClickBtnRateInfo()
        {
        }
    }
}