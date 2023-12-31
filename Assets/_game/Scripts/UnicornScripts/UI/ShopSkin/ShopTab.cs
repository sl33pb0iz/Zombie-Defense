using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.UI.Shop
{
    public class ShopTab : MonoBehaviour
    {
        [SerializeField] private Button btnTab;
        [SerializeField] private List<Sprite> listSprStateTabs;
        [SerializeField] private Image imgButton;

        private ShopCharacter shopController;

        public bool IsActive { get; private set; }

        private void Start()
        {
            btnTab.onClick.AddListener(OnClickBtnTab);
        }

        public void Init(ShopCharacter _shopController)
        {
            shopController = _shopController;
        }

        public void ActiveTab()
        {
            imgButton.sprite = listSprStateTabs[0];
            IsActive = true;
        }

        public void DisableTab()
        {
            imgButton.sprite = listSprStateTabs[1];
            IsActive = false;
        }

        private void OnClickBtnTab()
        {
            shopController.ActiveContentTab(transform.GetSiblingIndex());

            SoundManager.Instance.PlaySoundButton();
        }
    }

}