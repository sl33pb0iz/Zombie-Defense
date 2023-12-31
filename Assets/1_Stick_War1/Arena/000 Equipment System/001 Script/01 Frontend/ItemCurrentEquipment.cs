using Sirenix.OdinInspector;
using Snowyy.EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Snowyy.MergeSystem
{
    public class ItemCurrentEquipment : MonoBehaviour
    {
        [Button("Button")]
        [SerializeField] private Button btnOnClick;
        [Title("Image")]
        [SerializeField] private Image imgBorder;
        [SerializeField] private Image imgIcon;
        [Title("Text")]
        [SerializeField] private TextMeshProUGUI txtLevel;
        [Title("Data")]
        [SerializeField] private DataRarityEffect dataRarityEffect;
        [Title("Others")]
        [SerializeField] private GameObject goItem;

        public Equipment BindedEquipment { get; private set; }
        private void Start()
        {
            btnOnClick.onClick.AddListener(OnClickBtnClick);
        }
        public void Init(Equipment equipment = null)
        {
            if (equipment == null)
            {
                BindedEquipment = null;
                goItem.SetActive(false);
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
            UiEquipmentSystemBrain.Instance.UiMergeEquipment.TopManager.RemoveMergedEquipment();
            UiEquipmentSystemBrain.Instance.UiMergeEquipment.BotManager.OnRefresh?.Invoke();
        }
    }
}
