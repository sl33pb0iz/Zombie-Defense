using DG.Tweening;
using RocketTeam.Sdk.Services.Ads;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.UI
{
    public class UiPause : UICanvas
    {
        [SerializeField] private Button btnContinue;
        [SerializeField] public Button btnPause; 

        private void Start()
        {
            btnContinue.onClick.AddListener(OnClickBtnContinue);
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (!isShow)
            {
                Time.timeScale = 1f;
                return;
            }
            Time.timeScale = 0f;
            

        }

        private void OnClickBtnContinue()
        {
            GameManager.Instance.Resume();
            
            SoundManager.Instance.PlaySoundButton();
            if(GameManager.Instance.GameStateController.CurrentGameState == GameState.IN_GAME)
            {
                Show(false, false);
            }
            if (GameManager.Instance.GameStateController.CurrentGameState == GameState.LOBBY)
            {
                Show(false, true);
                GameManager.Instance.UiController.OpenUiMainLobby();
            }
        }
    }
}
