using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unicorn;
using Unicorn.UI;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class UiLootEndgame : UICanvas
    {
        [Title("_GAME OBJECT")]
        [SerializeField] private GameObject winBtnGroup;
        [SerializeField] private GameObject lostBtnGroup;
        [Title("_Button")]
        [SerializeField] private Button btnClose;
        [SerializeField] private Button btnRematch, btnHome;
        [SerializeField] private int loseGold;
        [SerializeField] private int winBaseGold;
        [SerializeField] private int loseDesignQuantity;
        [SerializeField] private int winDesignQuantity;
        public ItemLootEndgame[] arrItemLoot;

        private void Start()
        {
            btnClose.onClick.AddListener(OnClickBtnClose);
            btnRematch.onClick.AddListener(OnClickBtnRematch);
            btnHome.onClick.AddListener(OnClickBtnHome);
        }

        public void Init(int top = 1, bool isWin = false)
        {
            /*winBtnGroup.SetActive(isWin);
            lostBtnGroup.SetActive(!isWin);

            arrItemLoot[0].Init(LootType.GOLD,
                isWin
                    ? (3 - top) * winBaseGold +
                      winBaseGold * (GameManager.Instance.GameSetting.GetCurrentLimitedTime() + 1)
                    : loseGold);
            arrItemLoot[1].Init(LootType.DESIGN,
                isWin
                    ? (3 - top) * winDesignQuantity + (GameManager.Instance.GameSetting.GetCurrentLimitedTime() + 1)
                    : loseDesignQuantity);

            Show(true);*/
        }

        private void OnClickBtnClose()
        {
            SoundManager.Instance.PlaySoundButton();
            OnBackPressed();
        }

        private void OnClickBtnRematch()
        {/*
            SoundManager.Instance.PlaySoundButton();
            UnicornAdManager.ShowInterstitial(Helper.inter_arena_lose);

            OnBackPressed();
            GameManager.Instance.UiController.UiArenaImage.Show(false);
            GameManager.Instance.UiController.UiMatchMaking.IsIngame = true;
            GameManager.Instance.UiController.UiMatchMaking.Show(true);
            
            
            if (GameManager.Instance.UiController.UiSurvivoRevive.IsShowing)
            {
                GameManager.Instance.UiController.UiSurvivoRevive.IsShowing = false;
                GameManager.Instance.UiController.UiSurvivoRevive.OnBackPressed();
            }
            else
            {
                GameManager.Instance.UiController.UiArenaLose.OnBackPressed();
            }*/
        }

        private void OnClickBtnHome()
        {
            /*SoundManager.Instance.PlaySoundButton();

            UnicornAdManager.ShowInterstitial(Helper.inter_arena_lose_home);
            OnBackPressed();
            GameManager.Instance.UiController.UiArenaImage.Show(false);
            GameManager.Instance.MainCamera.CameraFollowSetup.FollowTransform = null;
            GameManager.Instance.MainCamera.CameraFollowSetup.Init();
            GameManager.Instance.GameMode = GameMode.NORMAL;
            GameManager.Instance.LevelTransition.LoadScene(false);

            if (GameManager.Instance.UiController.UiSurvivoRevive.IsShowing)
            {
                GameManager.Instance.UiController.UiSurvivoRevive.IsShowing = false;
                GameManager.Instance.UiController.UiSurvivoRevive.OnBackPressed();
            }
            else
            {
                GameManager.Instance.UiController.UiArenaLose.OnBackPressed();
            }*/
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
        }
    }
}