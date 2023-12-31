using Sirenix.OdinInspector;
using Snowyy.EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using UnityEngine;
using UnityEngine.UI;

namespace Snowyy.MergeSystem
{
    public class ItemMergedMaterial : MonoBehaviour
    {
        [Title("Button")]
        [SerializeField] private Button btnClick;
        [Title("Image")]
        [SerializeField] private Image imgBgType;
        [SerializeField] private Image imgBorder;
        [SerializeField] private Image imgIcon;
        [Title("Text")]
        [SerializeField] private TextMeshProUGUI txtLevel;
        [Title("Others")]
        [SerializeField] private GameObject goItem;
        [Title("Data")]
        [SerializeField] private DataRarityEffect dataRarityEffect;
        [SerializeField] private DataItemTemplate dataItemTemplate;

        public Equipment BindedEquipment { get; private set; }
        private void Start()
        {
            btnClick.onClick.AddListener(OnClickBtnClick);
        }
        public void Init(Equipment equipment = null)
        {
            if (equipment == null)
            {
                BindedEquipment = null;
                goItem.SetActive(false);
                Equipment currentMergingEquipment = UiEquipmentSystemBrain.Instance.UiMergeEquipment.TopManager.ItemCurrentEquipment.BindedEquipment;
                if (currentMergingEquipment != null)
                {
                    imgBgType.sprite = dataItemTemplate.dictEquipmentTypeIcons[currentMergingEquipment.EquipmentType];
                    imgBgType.color = dataRarityEffect.dictRarityBasedSprites[currentMergingEquipment.Rarity].rarityColor;
                }
                return;
            }
            BindedEquipment = equipment;
            goItem.SetActive(true);
            imgBorder.sprite = dataRarityEffect.dictRarityBasedSprites[equipment.Rarity].borderSprite;
            imgIcon.sprite = equipment.itemTemplate.arrUpgradeIcons[(int)equipment.Rarity];
            txtLevel.text = $"Lv.{equipment.CurrentLevel + 1}";

        }
        private void OnClickBtnClick()
        {
            SoundManager.Instance.PlaySoundButton();
            if (BindedEquipment == null)
            {
                return;
            }
            Init();
            UiEquipmentSystemBrain.Instance.UiMergeEquipment.BotManager.OnMergeItemChanged?.Invoke(UiEquipmentSystemBrain.Instance.UiMergeEquipment.TopManager.ItemCurrentEquipment.BindedEquipment);
        }
    }
}
