using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Unicorn.UI
{
    public class UiQuitLevel : UICanvas
    {

        [SerializeField] private Button btnYes;
        [SerializeField] private Button btnNo;
        [SerializeField] private Button btnExit;

        protected override void Awake()
        {
            btnYes.onClick.AddListener(OnYesPressed);
            btnNo.onClick.AddListener(OnNoPressed);
        }
        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (!isShow)
            {
                return;
            }
            Time.timeScale = 0f;
            
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            SoundManager.Instance.PlaySoundButton();
        }

        public void OnYesPressed()
        {
            SoundManager.Instance.PlaySoundButton();
            Time.timeScale = 1f; 
            if (GameManager.Instance.GameStateController.CurrentGameState == GameState.IN_GAME || GameManager.Instance.GameStateController.CurrentGameState == GameState.END_GAME)
            {
                GameManager.Instance.UiController.OpenUiMainLobby();
                GameManager.Instance.LoadLevel();
            }
        }

        public void OnNoPressed()
        {
            SoundManager.Instance.PlaySoundButton();
            Time.timeScale = 1f;
            Show(false, false);
        }

    }
}

