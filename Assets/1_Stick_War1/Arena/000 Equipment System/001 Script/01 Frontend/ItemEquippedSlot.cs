using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Snowyy.EquipmentSystem
{
    public class ItemEquippedSlot : MonoBehaviour
    {
        [Title("BUTTONS")]
        [SerializeField] private Button btnClick;

        [Title("IMAGE")]
        [SerializeField] private Image imgEmptySlot;
        [SerializeField] private Image imgBorder;
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image arrowCanUpgrade;

        [Title("TEXTS")]
        [SerializeField] private TextMeshProUGUI txtLevel;

        [Title("HOLDERS")]
        [SerializeField] private DataStats dataStats;
        [SerializeField] private DataRarityEffect dataRarityEffect;

        private Equipment BindedEquipment;

        private void Start()
        {
            btnClick.onClick.AddListener(OnClickBtnClick);
        }
        public void Init(Equipment equipment = null)
        {
            if (equipment == null)
            {
                BindedEquipment = null;
                ToggleImgBorder(false);
                imgEmptySlot.DOFade(1, 0f);
                return;
            }
            BindedEquipment = equipment;
            ToggleImgBorder(true);
            imgEmptySlot.DOFade(0f, 0f);

            DisplayInfo(BindedEquipment);
        }

        public int GetStat()
        {
            if (BindedEquipment == null)
            {
                Debug.Log("This slot is empty. No equipment have been equipped!!");
                return 0;
            }
            switch (BindedEquipment.EquipmentType)
            {
                case EquipmentType.Weapon:
                    return dataStats.GetStatAmount(BindedEquipment, StatsType.ATTACK);
                case EquipmentType.Backpack:
                    return dataStats.GetStatAmount(BindedEquipment, StatsType.ATTACK);
                case EquipmentType.Helmet:
                    return dataStats.GetStatAmount(BindedEquipment, StatsType.HEALTH);
                case EquipmentType.Armor:
                    return dataStats.GetStatAmount(BindedEquipment, StatsType.HEALTH);
                case EquipmentType.Boot:
                    return dataStats.GetStatAmount(BindedEquipment, StatsType.HEALTH);
                default:
                    Debug.Log($"This equipment type {BindedEquipment.EquipmentType} haven't been implemented!!");
                    return 0;
            }
        }

        private void DisplayInfo(Equipment equipment)
        {
            imgBorder.sprite = dataRarityEffect.dictRarityBasedSprites[equipment.Rarity].borderSprite;
            imgIcon.sprite = equipment.itemTemplate.arrUpgradeIcons[(int)equipment.Rarity];
            txtLevel.text = $"Lv.{equipment.CurrentLevel + 1}";

            InitArrowCanUpgrade(equipment);
        }

        private void InitArrowCanUpgrade(Equipment equipment)
        {
            if (equipment.CurrentLevel < dataStats.GetLevelCap(equipment.Rarity) && dataStats.IsCanLevelUp(equipment))
            {
                ToggleArrowCanUpgrade(true);
            }
            else
            {
                ToggleArrowCanUpgrade(false);
            }
        }

        private void OnClickBtnClick()
        {
            if (BindedEquipment == null)
            {
                Debug.Log("Either this is an empty slot or this binded equipment is null so that pop up can't show up");
                return;
            }
            UiEquipmentSystemBrain.Instance.UiPopupEquipment.SetEquipmentToShow(BindedEquipment, EquipmentDataManager.Instance.FindEquipmentIndex(BindedEquipment));
            UiEquipmentSystemBrain.Instance.UiPopupEquipment.Show(true);
        }

        private void ToggleImgBorder(bool isToggle)
        {
            Debug.Log(isToggle);
            imgBorder.gameObject.SetActive(isToggle);
        }
        private void ToggleArrowCanUpgrade(bool isToggle)
        {
            arrowCanUpgrade.gameObject.SetActive(isToggle);
        }
    }
}
