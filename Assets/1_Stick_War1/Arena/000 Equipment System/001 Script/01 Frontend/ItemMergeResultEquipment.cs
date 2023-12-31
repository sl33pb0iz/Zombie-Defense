using Sirenix.OdinInspector;
using Snowyy.EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Snowyy.MergeSystem
{
    public class ItemMergeResultEquipment : MonoBehaviour
    {
        [Title("Button")]
        [SerializeField] private Button btnClick;
        [Title("Image")]
        [SerializeField] private Image imgBorder;
        [SerializeField] private Image imgIcon;
        [Title("Text")]
        [SerializeField] private TextMeshProUGUI txtName;
        [SerializeField] private TextMeshProUGUI txtLevel;
        [SerializeField] private TextMeshProUGUI txtCurrentLevel;
        [SerializeField] private TextMeshProUGUI txtCurrentStat;
        [SerializeField] private TextMeshProUGUI txtResultLevel;
        [SerializeField] private TextMeshProUGUI txtResultStat;
        [SerializeField] private TextMeshProUGUI txtStatType;
        [SerializeField] private TextMeshProUGUI textDescription;
        [Title("Object")]
        [SerializeField] private GameObject goItem;
        [SerializeField] private GameObject textHolder;
        [Title("Data")]
        [SerializeField] private DataRarityEffect dataRarityEffect;
        [SerializeField] private DataItemTemplate dataItemTemplate;
        [SerializeField] private DataStats dataStats;

        public void Init(Equipment equipment = null)
        {
            if (equipment == null)
            {
                goItem.SetActive(false);
                textHolder.SetActive(false);
                textDescription.gameObject.SetActive(true);
                return;
            }
            goItem.SetActive(true);
            textHolder.SetActive(true);
            textDescription.gameObject.SetActive(false);

            imgBorder.sprite = dataRarityEffect.dictRarityBasedSprites[(Rarity)((int)equipment.Rarity + 1)].borderSprite;
            imgIcon.sprite = equipment.itemTemplate.arrUpgradeIcons[(int)equipment.Rarity + 1];
            DisplayResultInformation(equipment);
        }
        private void DisplayResultInformation(Equipment equipment)
        {
            txtName.text = $"{equipment.itemTemplate.name}";
            txtLevel.text = $"Lv.{equipment.CurrentLevel + 1}";
            txtCurrentLevel.text = $"{dataStats.GetLevelCap(equipment.Rarity) + 1}";
            txtResultLevel.text = $"{dataStats.GetLevelCap((Rarity)((int)equipment.Rarity + 1)) + 1}";
            switch (equipment.EquipmentType)
            {
                case EquipmentType.Weapon:
                    txtStatType.text = $"ATK";
                    txtCurrentStat.text = $"{dataStats.GetStatAmount(equipment, StatsType.ATTACK)}";
                    txtResultStat.text = $"{dataStats.GetStatAmount(equipment.EquipmentType, StatsType.ATTACK, (Rarity)((int)equipment.Rarity + 1), equipment.CurrentLevel)}";
                    break;
                case EquipmentType.Backpack:
                    txtStatType.text = $"ATK";
                    txtCurrentStat.text = $"{dataStats.GetStatAmount(equipment, StatsType.ATTACK)}";
                    txtResultStat.text = $"{dataStats.GetStatAmount(equipment.EquipmentType, StatsType.ATTACK, (Rarity)((int)equipment.Rarity + 1), equipment.CurrentLevel)}";
                    break;
                case EquipmentType.Helmet:
                    txtStatType.text = $"HP";
                    txtCurrentStat.text = $"{dataStats.GetStatAmount(equipment, StatsType.HEALTH)}";
                    txtResultStat.text = $"{dataStats.GetStatAmount(equipment.EquipmentType, StatsType.HEALTH, (Rarity)((int)equipment.Rarity + 1), equipment.CurrentLevel)}";
                    break;
                case EquipmentType.Armor:
                    txtStatType.text = $"HP";
                    txtCurrentStat.text = $"{dataStats.GetStatAmount(equipment, StatsType.HEALTH)}";
                    txtResultStat.text = $"{dataStats.GetStatAmount(equipment.EquipmentType, StatsType.HEALTH, (Rarity)((int)equipment.Rarity + 1), equipment.CurrentLevel)}";
                    break;
                case EquipmentType.Boot:
                    txtStatType.text = $"HP";
                    txtCurrentStat.text = $"{dataStats.GetStatAmount(equipment, StatsType.HEALTH)}";
                    txtResultStat.text = $"{dataStats.GetStatAmount(equipment.EquipmentType, StatsType.HEALTH, (Rarity)((int)equipment.Rarity + 1), equipment.CurrentLevel)}";
                    break;
            }
        }
    }
}
