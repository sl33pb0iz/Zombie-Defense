using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using UnityEngine;
using UnityEngine.UI;

namespace Snowyy.EquipmentSystem
{
    public class ItemEquipment : MonoBehaviour
    {
        [Title("Buttons")]
        [SerializeField] private Button btnClick;
        [Title("Images")]
        [SerializeField] private Image imgBackground;
        [SerializeField] private Image iconEquipment;
        [SerializeField] private Image iconEquipmentType;
        [Title("Text")]
        [SerializeField] private TextMeshProUGUI txtLevel;

        [Title("Others")]
        [SerializeField] private DataRarityEffect data;
        [SerializeField] private DataItemTemplate dataItemTemplate;

        public Equipment BindedEquipment { get; private set; }
        
        private void Start()
        {
            btnClick.onClick.AddListener(OnClickBtnThisItem);
        }
        
        public void Init(Equipment equipment)
        {
            BindedEquipment = equipment;

            txtLevel.text = $"LV.{equipment.CurrentLevel + 1}";
            iconEquipment.sprite = equipment.itemTemplate.arrUpgradeIcons[(int)equipment.Rarity];
            imgBackground.sprite = data.dictRarityBasedSprites[equipment.Rarity].borderSprite;

            iconEquipmentType.sprite = dataItemTemplate.dictEquipmentTypeIcons[equipment.EquipmentType];
            iconEquipmentType.color = data.dictRarityBasedSprites[equipment.Rarity].rarityColor;
            //iconEquipmentType.gameObject.SetActive(false);
        }
        
        public void OnClickBtnThisItem()
        {
            UiEquipmentSystemBrain.Instance.UiPopupEquipment.SetEquipmentToShow(BindedEquipment, EquipmentDataManager.Instance.FindEquipmentIndex(BindedEquipment));
            UiEquipmentSystemBrain.Instance.UiPopupEquipment.Show(true);
        }
    }
}
