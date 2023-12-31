using Unicorn.UI.Shop;
using UnityEngine;
using UnityEngine.UI;
using Snowyy.EquipmentSystem;
using Arena;

namespace Unicorn.UI
{
    public class UiController : MonoBehaviour
    {
        public enum ScreenState
        {
            MainLobby,
            Pause,
            ShopIAP,
            LuckyWheel,
            Setting,
            Stage,
            QuitLevel,
            Equipment,
        }

        public ScreenState screenState;

        public UiMainLobby UiMainLobby;
        public UiLose UiLose;
        public UiWin UiWin;
        public UiPause UiPause;
        public UISetting UiSetting;
        public UiStage UiStage; 
        public UiQuitLevel UiQuitLevel;
        public UiTop UiTop;
        public UiEquipmentSystemBrain UiEquipmentSystemBrain;
        public UiPopupUsername UiPopupUsername; 

        public PopupRewardEndGame PopupRewardEndGame;
        public LuckyWheel LuckyWheel;
        public UiLoading Loading;

        public UiSurvivorShop UiSurvivorShop;
        public UiNotEnough UiNotEnough;

        private PlayerDataManager PlayerDataManager => PlayerDataManager.Instance;


        private void OnEnable()
        {
            UiTop.OnClickBtnAddGemEvent += OpenShopIAP;
            UiTop.OnClickBtnAddGoldEvent += OpenShopIAP; 
        }

        private void OnDisable()
        {
            UiTop.OnClickBtnAddGemEvent -= OpenShopIAP;
            UiTop.OnClickBtnAddGoldEvent -= OpenShopIAP;
        }

        public void Init()
        {
            GameManager.Instance.GamePaused += OpenUiPause;
            GameManager.Instance.GameResumed += OpenUiMainLobby;

            OpenUiUsername(); 
/*
            ShopCharacter.Configure(
                PlayerDataManager.Instance,
                PlayerDataManager.Instance.DataTextureSkin);*/
        }
        public void OpenUiMainLobby()
        {
             UiTop.ActiveButtonFunction(true);
             screenState = ScreenState.MainLobby;
             Invoke(nameof(UpdateButtonState), 0f);
        }
        public void OpenUiLose()
        {
            UiLose.Show(true);
        }
        public void OpenUiWin(int gold)
        {
            var UiWin = GameManager.Instance.UiController.UiWin;

            UiWin.Show(true);
            UiWin.Init(gold);
        }

        public void OpenUiUsername()
        {
            if (PlayerDataManager.CanChangeUsername())
            {
                // Hiển thị popup khi lần đầu tiên mở game
                UiPopupUsername.Show(true);
                
            }

            else UiPopupUsername.Show(false);
        }
        
        public void OpenUiSetting()
        {
            screenState = ScreenState.Setting;
            Invoke(nameof(UpdateButtonState), 0f);
        }
        public void OpenUiQuitLevel()
        {
            if(GameManager.Instance.GameStateController.CurrentGameState == GameState.IN_GAME || GameManager.Instance.GameStateController.CurrentGameState == GameState.END_GAME)
            {
                screenState = ScreenState.QuitLevel;
                Invoke(nameof(UpdateButtonState), 0f);
            }
            else if(GameManager.Instance.GameStateController.CurrentGameState == GameState.LOBBY)
            {
                OpenUiMainLobby();
            }

        }
        public void OpenShopIAP(bool IsGoToGold = false, bool IsGoToGem = false)
        {
            UiSurvivorShop.IsGoToGold = IsGoToGold;
            UiSurvivorShop.IsGoToGem = IsGoToGem;
            UiTop.ActiveButtonFunction(false);
            screenState = ScreenState.ShopIAP;
            Invoke(nameof(UpdateButtonState), 0f);
        }

        public void OpenUIEquipment()
        {
            UiTop.ActiveButtonFunction(false);
            screenState = ScreenState.Equipment;
            Invoke(nameof(UpdateButtonState), 0f);
        }

        public void OpenUiPause()
        {         
            screenState = ScreenState.Pause;
            Invoke(nameof(UpdateButtonState), 0f);

        }
        public void OpenUiStage()
        {
            UiTop.ActiveButtonFunction(false);
            screenState = ScreenState.Stage;
            Invoke(nameof(UpdateButtonState), 0f);
        }
        public void OpenPopupReward(RewardEndGame reward, TypeDialogReward type)
        {
            if (PopupRewardEndGame.IsShow)
                return;

            PopupRewardEndGame.Show(true);
            PopupRewardEndGame.Init(reward, type);
        }
        
        public void OpenLuckyWheel()
        {
            UiTop.ActiveButtonFunction(false);
            screenState = ScreenState.LuckyWheel;
            Invoke(nameof(UpdateButtonState), 0f);
        }
        public void OpenLoading(bool isLoading)
        {
            Loading.Show(isLoading);
        }

        public void PopupNotEnough(string txtToDisplay)
        {
            UiNotEnough.Init(txtToDisplay);
        }

        void TurnOffAllUi()
        {
            UiMainLobby.Show(false);
            UiSurvivorShop.Show(false);
            LuckyWheel.Show(false);
            UiPause.Show(false);
            UiSetting.Show(false);
            UiStage.Show(false);
            UiQuitLevel.Show(false);
            UiEquipmentSystemBrain.ShowUiEquipmentSystem(false);
        }

        void UpdateButtonState()
        {
            TurnOffAllUi();
            switch (screenState)
            {
                case ScreenState.MainLobby:
                    UiMainLobby.Show(true);
                    break;
                case ScreenState.ShopIAP:
                    UiSurvivorShop.Show(true);
                    break;
                case ScreenState.LuckyWheel:
                    LuckyWheel.Show(true);
                    break;
                case ScreenState.Pause:
                    UiPause.Show(true);
                    break;
                case ScreenState.Setting:
                    UiSetting.Show(true);
                    break;
                case ScreenState.Stage:
                    UiStage.Show(true);
                    break;
                case ScreenState.QuitLevel:
                    UiQuitLevel.Show(true);
                    break;
                case ScreenState.Equipment:
                    UiEquipmentSystemBrain.ShowUiEquipmentSystem(true);
                    break; 
            }
        }
    }
}

