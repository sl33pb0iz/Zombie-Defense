using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class ItemGemShop : MonoBehaviour
    {
        [Title("_BUTTON")]
        [SerializeField] private Button btnPurchase;
        [Title("_IMAGE")]
        [SerializeField] private Image imgIcon;
        [Title("_TEXT")]
        [SerializeField] private TextMeshProUGUI txtAmount;
        [SerializeField] private TextMeshProUGUI txtPrice;
        [SerializeField] private TextMeshProUGUI txtDescription;


        private IdPack currentIdPack;

        private void Start()
        {
            btnPurchase.onClick.AddListener(OnClickBtnPurchase);
        }

        public void Init(IdPack iapPack, int gemAmount, string price, Sprite icon, string description)
        {
            currentIdPack = iapPack;
            txtAmount.text = $"{gemAmount}";
            txtPrice.text = price;
            imgIcon.sprite = icon;
            txtDescription.text = description;
        }

        private void OnClickBtnPurchase()
        {
            SoundManager.Instance.PlaySoundButton();
//TODO: bnack to IAP
            GameManager.Instance.Profile.AddGem(
                GameManager.Instance.IapController.Data.dictInfoPackage[currentIdPack].listRewardPack[0].amount,
                "gem_iap_test");
            return;
            GameManager.Instance.IapController.PurchaseProduct((int)currentIdPack);
        }
    }
}