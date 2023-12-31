using System;
using System.Collections;
using System.Collections.Generic;
using RocketTeam.Sdk.Services.Ads;
using Sirenix.OdinInspector;
using TMPro;
using Unicorn.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Arena;
using Random = UnityEngine.Random;

namespace Unicorn.UI
{
    public class UiMainLobby : UICanvas
    {
        [Title("Button")] [SerializeField] private Button btnHide;
        [SerializeField] private Button btnLuckySpin;
        [SerializeField] private Button btnSkin;
        [SerializeField] private Button btnNoAds;
        [SerializeField] private Button btnEquipment;

        public TextMeshProUGUI txtCurentLevel;
        public TextMeshProUGUI txtLevelInfo; 

        public Button BtnNoAds
        {
            get => btnNoAds;
        }

        public Button BtnPlay => btnHide;

        // Start is called before the first frame update
        void Start()
        {
            btnHide.onClick.AddListener(OnClickBtnPlay);
            btnLuckySpin.onClick.AddListener(OnClickBtnSpin);
            btnSkin.onClick.AddListener(OnClickShopSkin);
            btnEquipment.onClick.AddListener(OnClickBtnInventory);
            btnNoAds.onClick.AddListener(OnClickBtnNoAds);
            Init();
        }

        private void Init()
        {
            /*SetLayoutKey();*/
            var dataLevel = GameManager.Instance.DataLevel;
            BtnPlay.gameObject.SetActive(true);
            if (PlayerDataManager.Instance.IsNoAds())
            {
                btnNoAds.interactable = false;
            }
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (IsShow)
            {
                Init();
            }
            else
            {
                BtnPlay.gameObject.SetActive(false);
            }
        }


        private void OnClickBtnPlay()
        {
            GameManager.Instance.StartLevel();
            
            ShowAniHide();

            SoundManager.Instance.PlaySoundButton();
        }

        private void OnClickBtnSpin()
        {
            GameManager.Instance.UiController.OpenLuckyWheel();

            SoundManager.Instance.PlaySoundButton();
        }

        private void OnClickShopSkin()
        {
            GameManager.Instance.UiController.OpenShopIAP();
            SoundManager.Instance.PlaySoundButton();
        }

        private void OnClickBtnNoAds()
        {
            GameManager.Instance.IapController.PurchaseProduct((int) IdPack.NO_ADS_BASIC);
            SoundManager.Instance.PlaySoundButton();
        }


        private void OnClickBtnStage()
        {
            GameManager.Instance.UiController.OpenUiStage();
            SoundManager.Instance.PlaySoundButton();
        }

        private void OnClickBtnInventory()
        {
            GameManager.Instance.UiController.OpenUIEquipment();

            SoundManager.Instance.PlaySoundButton();
        }

        public void ShowAniHide()
        {
            Show(false);
        }

        public void ActiveMainLobby()
        {
            Show(true);
        }
        
    }
}
