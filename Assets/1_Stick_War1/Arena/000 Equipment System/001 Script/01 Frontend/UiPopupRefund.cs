using Sirenix.OdinInspector;
using Snowyy.Ultilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Snowyy.EquipmentSystem
{
    public class UiPopupRefund : UICanvas
    {
        [Title("Buttons")]
        [SerializeField] private Button btnClose;
        [SerializeField] private Button btnCloseBg;
        [SerializeField] private Button btnLevelDown;
        [Title("Image")]
        [SerializeField] private Image imgIconEquipmentCurrent;
        [SerializeField] private Image imgIconEquipmentRefund;
        [SerializeField] private Image imgBgEquipmentCurrent;
        [SerializeField] private Image imgBgEquipmentRefund;
        [Title("Text")]
        [SerializeField] private TextMeshProUGUI txtGoldRefund;
        [SerializeField] private TextMeshProUGUI txtResourceRefund;
        [SerializeField] private TextMeshProUGUI txtCurrentLevel;
        [Title("Data")]
        [SerializeField] private DataRarityEffect dataRarityEffect;
        [SerializeField] private DataStats dataStats;

        private Equipment bindedEquipment;
        private int bindedEquipmentIndex;

        private long refundGold;
        private int refundResource;
        private void Start()
        {
            btnClose.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySoundButton();
                OnClosePopupPressed();
            });
            btnCloseBg.onClick.AddListener(() =>
            {
                OnClosePopupPressed();
            });
            btnLevelDown.onClick.AddListener(OnClickBtnLevelDown);
        }
        private void OnClickBtnLevelDown()
        {
            SoundManager.Instance.PlaySoundButton();
            OnClosePopupPressed();

            GameManager.Instance.Profile.AddGold((int)refundGold, StringHelper.GOLD_REFUND);
            var currentResourceQuantity = EquipmentDataManager.Instance.GetResource(bindedEquipment.itemTemplate.resourceToUpgrade).quantity;
            EquipmentDataManager.Instance.
                SetResource(bindedEquipment.itemTemplate.resourceToUpgrade, new Resource(bindedEquipment.itemTemplate.resourceToUpgrade, currentResourceQuantity + refundResource));
            bindedEquipment.ResetLevel();

            if (UiEquipmentSystemBrain.Instance.UiEquipmentSystem.InventoryManager.IsItemInInventory(bindedEquipment))
            {
                EquipmentDataManager.Instance.SetEquipmentByIndex(bindedEquipmentIndex, bindedEquipment);
            }
            else
            {
                EquipmentDataManager.Instance.SetCurrentEquippedEquipment(bindedEquipment.EquipmentType, bindedEquipment);
            }

            EquipmentDataManager.Instance.SaveAllEquipments();
            UiEquipmentSystemBrain.Instance.UiEquipmentSystem.OnRefresh?.Invoke();
        }
        private void Init(Equipment equipment)
        {
            imgIconEquipmentCurrent.sprite = equipment.itemTemplate.arrUpgradeIcons[(int)equipment.Rarity];
            imgIconEquipmentRefund.sprite = equipment.itemTemplate.arrUpgradeIcons[(int)equipment.Rarity];

            imgBgEquipmentCurrent.sprite = dataRarityEffect.dictRarityBasedSprites[equipment.Rarity].borderSprite;
            imgBgEquipmentRefund.sprite = dataRarityEffect.dictRarityBasedSprites[equipment.Rarity].borderSprite;

            dataStats.GetRefundGoldAndResources(equipment, out refundGold, out refundResource);

            txtCurrentLevel.text = $"Lv.{equipment.CurrentLevel + 1}";
            txtGoldRefund.text = $"x{SnowyyExtensions.FormatLargeNumber(refundGold)}";
            txtResourceRefund.text = $"x{refundResource}";


        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (IsShow)
            {
                Init(bindedEquipment);
            }
        }
        public void SetBindedEquipment(Equipment equipment, int index)
        {
            bindedEquipment = equipment;
            bindedEquipmentIndex = index;
        }
    }
}
