using Sirenix.OdinInspector;
using Snowyy.EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Snowyy.MergeSystem
{
    public class UiMergeEquipment : UICanvas
    {
        [Title("Image")]
        [SerializeField] private Image imgTopBackground;
        [Title("Others")]
        [SerializeField] private MergeTopManager topManager;
        [SerializeField] private MergeBotManager botManager;
        [SerializeField] private UiMergeBottomBar mergeBottomBar;

        [Title("Others")]
        [SerializeField] private Button btnClose;


        public MergeTopManager TopManager => topManager;
        public MergeBotManager BotManager => botManager;

        public OnClose OnClose { get; private set; }

        private void Start()
        {
            OnClose += Close;
            btnClose.onClick.AddListener(OnClickBtnClose);
        }
        public void Init()
        {
            topManager.Init();
            botManager.Init();
        }
        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (IsShow)
            {
                Init();
            }
            mergeBottomBar.Show(IsShow);
            ToggleImgBackground(IsShow);
        }

        private void Close()
        {
            OnClosePopupPressed();
            UiEquipmentSystemBrain.Instance.UiEquipmentSystem.OnRefresh?.Invoke();
        }
        private void OnClickBtnClose()
        {
            SoundManager.Instance.PlaySoundButton();
            UiEquipmentSystemBrain.Instance.UiMergeEquipment.OnClose?.Invoke();
        }


        private void ToggleImgBackground(bool isToggle)
        {
            imgTopBackground.gameObject.SetActive(isToggle);
        }


    }
}
