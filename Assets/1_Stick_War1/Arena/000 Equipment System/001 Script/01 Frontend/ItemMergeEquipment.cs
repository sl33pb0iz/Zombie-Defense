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
    public class ItemMergeEquipment : MonoBehaviour
    {
        [Button("Buttons")]
        [SerializeField] private Button btnClick;
        [Title("Image")]
        [SerializeField] private Image imgBorder;
        [SerializeField] private Image imgIcon;
        [Title("Text")]
        [SerializeField] private TextMeshProUGUI txtLevel;
        [Title("GameObject")]
        [SerializeField] private GameObject panelEquipped;
        [SerializeField] private GameObject panelLock;
        [SerializeField] private GameObject panelTick;
        [Title("Data")]
        [SerializeField] private DataRarityEffect dataRarityEffect;
        [SerializeField] private DataItemTemplate dataItemTemplate;

        private Equipment bindedEquipment;
        private void Start()
        {
            btnClick.onClick.AddListener(OnClickBtnClick);
        }
        public void Init(Equipment equipment, bool isLock = false, bool isTick = false)
        {
            bindedEquipment = equipment;

            imgBorder.sprite = dataRarityEffect.dictRarityBasedSprites[equipment.Rarity].borderSprite;
            imgIcon.sprite = equipment.itemTemplate.arrUpgradeIcons[(int)equipment.Rarity];
            txtLevel.text = $"LV.{equipment.CurrentLevel + 1}";

            panelEquipped.SetActive(equipment.IsEquip);
            panelLock.SetActive(isLock);
            panelTick.SetActive(isTick);
        }
        private void OnClickBtnClick()
        {
            SoundManager.Instance.PlaySoundButton();

            if (panelLock.activeSelf)
            {
                return;
            }
            MergeTopManager topManager = UiEquipmentSystemBrain.Instance.UiMergeEquipment.TopManager;
            MergeBotManager botManager = UiEquipmentSystemBrain.Instance.UiMergeEquipment.BotManager;
            Equipment currentMergeEquipment = topManager.ItemCurrentEquipment.BindedEquipment;

            if (panelTick.activeSelf)
            {
                if (currentMergeEquipment == bindedEquipment)
                {
                    topManager.RemoveMergedEquipment();
                    botManager.OnRefresh?.Invoke();
                }
                if (topManager.CheckIncludedInMaterialSlot(bindedEquipment))
                {
                    topManager.GetMaterialSlot(bindedEquipment).Init();
                    botManager.OnMergeItemChanged?.Invoke(bindedEquipment);
                }
                return;
            }


            if (currentMergeEquipment == null)
            {
                topManager.SelectMergedEquipment(bindedEquipment);
                botManager.OnMergeItemChanged?.Invoke(bindedEquipment);
                return;
            }
            if (currentMergeEquipment == bindedEquipment)
            {
                topManager.RemoveMergedEquipment();
                botManager.OnRefresh?.Invoke();
                return;
            }
            else
            {
                if (topManager.CheckMaterialsSlot())
                {
                    topManager.GetMaterialEmptySlot().Init(bindedEquipment);
                    botManager.OnMergeItemChanged?.Invoke(bindedEquipment);
                }
            }

        }
    }
}
