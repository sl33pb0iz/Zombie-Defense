using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Unicorn.UI
{
    public class UISetting : UICanvas
    {
        [SerializeField] private Button btnMusic;
        [SerializeField] private TextMeshProUGUI txtMusic;
        [SerializeField] private Button btnSound;
        [SerializeField] private TextMeshProUGUI txtSound;
        [SerializeField] private Button btnVibrate;
        [SerializeField] private TextMeshProUGUI txtVibrate;
        [SerializeField] private Button btnRateUs;
        [SerializeField] private Button btnRestorePurchase;
        [SerializeField] private Button btnClose; 

        protected override void Awake()
        {
            btnMusic.onClick.AddListener(ToggleMusic);
            btnSound.onClick.AddListener(ToggleSound);
            btnVibrate.onClick.AddListener(ToggleVibrate);
            btnRateUs.onClick.AddListener(Rate);
            btnRestorePurchase.onClick.AddListener(RestorePurchase);
            btnClose.onClick.AddListener(OnClosePressed);

            RestoreSettings();
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (!isShow)
            {
                return; 
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            SoundManager.Instance.PlaySoundButton();
        }

        public override void OnClosePressed()
        {
            base.OnClosePressed();
            SoundManager.Instance.PlaySoundButton();
        }

        private void RestoreSettings()
        {
            txtMusic.text = PlayerDataManager.Instance.GetMusicSetting() ? "ON" : "OFF";
            txtSound.text = PlayerDataManager.Instance.GetSoundSetting() ? "ON" : "OFF";

        }

        private void ToggleMusic()
        {
            bool isOn = !PlayerDataManager.Instance.GetMusicSetting();
            txtMusic.text = isOn ? "ON" : "OFF";
            PlayerDataManager.Instance.SetMusicSetting(isOn);

            SoundManager.Instance.PlaySoundButton();

            SoundManager.Instance.SettingMusic(isOn);

            Color btnColor = isOn ? Color.white : Color.grey; 
            btnMusic.image.color = btnColor;
        }

        private void ToggleSound()
        {
            bool isOn = !PlayerDataManager.Instance.GetSoundSetting();
            txtSound.text = isOn ? "ON" : "OFF";
            PlayerDataManager.Instance.SetSoundSetting(isOn);

            SoundManager.Instance.PlaySoundButton();

            SoundManager.Instance.SettingFxSound(isOn);

            Color btnColor = isOn ? Color.white : Color.grey;
            btnSound.image.color = btnColor;
        }

        private void ToggleVibrate()
        {
            bool isOn = !PlayerDataManager.Instance.GetVibrateSetting();
            txtVibrate.text = isOn ? "ON" : "OFF";
            PlayerDataManager.Instance.SetVibrateSetting(isOn);
            VibrationManager.instance.VibratePop();

            Color btnColor = isOn ? Color.white : Color.grey;
            btnVibrate.image.color = btnColor;
        }

        private void Rate()
        {
            Application.OpenURL(RocketConfig.OPEN_LINK_RATE);
        }

        private void RestorePurchase()
        {
#if UNITY_IOS
        GameManager.Instance.IapController.RestoreButtonClick();
#endif
        }

    }

}