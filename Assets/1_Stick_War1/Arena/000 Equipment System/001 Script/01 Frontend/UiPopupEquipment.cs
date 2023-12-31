using Sirenix.OdinInspector;
using Snowyy;
using Snowyy.Ultilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unicorn;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.UI;
using Spicyy.System;

namespace Snowyy.EquipmentSystem
{
    public class UiPopupEquipment : UICanvas
    {
        [FoldoutGroup("Button"), SerializeField] private Button btnUn_Equip;
        [FoldoutGroup("Button"), SerializeField] private Button btnLevelUp;
        [FoldoutGroup("Button"), SerializeField] private Button btnSell;
        [FoldoutGroup("Button"), SerializeField] private Button btnClose;
        [FoldoutGroup("Button"), SerializeField] private Button btnCloseBg;
        [FoldoutGroup("Button"), SerializeField] private Button btnRefund;

        [FoldoutGroup("Image"), SerializeField] private Image imgTitleBorder;
        [FoldoutGroup("Image"), SerializeField] private Image imgIcon;
        [FoldoutGroup("Image"), SerializeField] private Image imgIconBorder;
        [FoldoutGroup("Image"), SerializeField] private Image imgStatDisplay;
        [FoldoutGroup("Image"), SerializeField] private Image imgLvlDisplayFill;
        [FoldoutGroup("Image"), SerializeField] private Image imgIconResourceToUpgrade;

        [FoldoutGroup("Text"), SerializeField] private TextMeshProUGUI txtBtnUn_Equip;
        [FoldoutGroup("Text"), SerializeField] private TextMeshProUGUI txtTitle;
        [FoldoutGroup("Text"), SerializeField] private TextMeshProUGUI txtStatDisplay;
        [FoldoutGroup("Text"), SerializeField] private TextMeshProUGUI txtLvlDisplay;
        [FoldoutGroup("Text"), SerializeField] private TextMeshProUGUI txtGoldToUpgrade;
        [FoldoutGroup("Text"), SerializeField] private TextMeshProUGUI txtResourceToUpgrade;

        [FoldoutGroup("Game Object"), SerializeField] private GameObject areaUpgradeInfo;
        [FoldoutGroup("Game Object"), SerializeField] private GameObject areaTxtMaxUpgrade;

        [FoldoutGroup("Data"), SerializeField] private DataRarityEffect dataRarityEffect;
        [FoldoutGroup("Data"), SerializeField] private DataStats dataStats;
        [FoldoutGroup("Data"), SerializeField] private DataResources dataResources;

        [FoldoutGroup("Others"), SerializeField] private string stringColorAlert;
        [FoldoutGroup("Others"), SerializeField] private Sprite[] arrStatsBackground;
        [FoldoutGroup("Others"), SerializeField] private Dictionary<Rarity, ItemSkillDisplay> dictEffectDisplays;

        public Equipment equipmentToShow { get; private set; }
        private int equipmentToShowIndex;

        private EquipmentDataManager equipmentDataManager;
        private InventoryManager inventoryManager;
        private EquipmentChangerPreviewCharacter previewCharacter; 
        private EquipSlotManager equipSlotManager;

        private bool isManagersInitialized = false;
        private bool isClicked = false;


        private void Start()
        {
            InitManagers();

            btnClose.onClick.AddListener(OnClickBtnClose);
            btnCloseBg.onClick.AddListener(() =>
            {
                OnClickBtnCloseBg();
            });
            btnUn_Equip.onClick.AddListener(OnClickBtnUnOrEquip);
            btnLevelUp.onClick.AddListener(OnClickBtnLevelUp);
            btnSell.onClick.AddListener(OnClickBtnMaxLevel);
            btnRefund.onClick.AddListener(OnClickBtnRefund);

        }

        private void InitManagers()
        {
            if (isManagersInitialized)
            {
                return;
            }
            isManagersInitialized = true;

            equipmentDataManager = EquipmentDataManager.Instance;
            inventoryManager = UiEquipmentSystemBrain.Instance.UiEquipmentSystem.InventoryManager;
            equipSlotManager = UiEquipmentSystemBrain.Instance.UiEquipmentSystem.EquipSlotManager;
            previewCharacter = UiEquipmentSystemBrain.Instance.UiEquipmentSystem.PreviewCharacter;

        }

        private void Init(Equipment equipment)
        {
            isClicked = false;

            InitManagers();
            var keys = dictEffectDisplays.Keys.ToArray();

            txtTitle.text = equipment.itemTemplate.name;
            txtLvlDisplay.text = $"Level: {equipment.CurrentLevel + 1}/{dataStats.GetLevelCap(equipment.Rarity) + 1}";
            imgIcon.sprite = equipment.itemTemplate.arrUpgradeIcons[(int)equipment.Rarity];

            InitButtonEquip();
            InitButtonRefund(equipment);
            SetUpgradeInfo(equipment);
            SetStatDisplay(equipment);
            SetRarityBasedImages(equipment.Rarity);
            SetEffectUnlockOrLock(equipment.Rarity, keys);
            SetEffectDescriptions(equipment, keys);
        }
        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (IsShow)
            {
                Init(equipmentToShow);
            }
        }
        //**
        //*** Init Rarity-based border and title
        private void SetRarityBasedImages(Rarity rarity)
        {
            imgIconBorder.sprite = dataRarityEffect.dictRarityBasedSprites[rarity].borderSprite;
            imgTitleBorder.sprite = dataRarityEffect.dictRarityBasedSprites[rarity].titleBarSprite;
        }
        //**
        //*** Init unlock or lock state of rarity effects
        private void SetEffectUnlockOrLock(Rarity currentRarity, Rarity[] arrRarity)
        {

            for (int i = 0; i < arrRarity.Length; i++)
            {
                if ((int)arrRarity[i] <= (int)currentRarity && arrRarity[i] != Rarity.COMMON)
                {
                    dictEffectDisplays[arrRarity[i]].ToggleImgLock(false);
                    dictEffectDisplays[arrRarity[i]].SetTextEffectColor(Color.white);

                }
                else
                {
                    dictEffectDisplays[arrRarity[i]].ToggleImgLock(true);
                    dictEffectDisplays[arrRarity[i]].SetTextEffectColor(Color.gray);
                }
            }
        }
        //**
        //*** Display upgrade materials
        private void SetUpgradeInfo(Equipment equipment)
        {
            if (equipment.CurrentLevel == dataStats.GetLevelCap(equipment.Rarity))
            {
                areaUpgradeInfo.SetActive(false);
                btnLevelUp.gameObject.SetActive(false);
                btnSell.gameObject.SetActive(false);
                areaTxtMaxUpgrade.SetActive(true);
                return;
            }
            areaUpgradeInfo.SetActive(true);
            btnLevelUp.gameObject.SetActive(true);
            btnSell.gameObject.SetActive(true);
            areaTxtMaxUpgrade.SetActive(false);

            long currentGold = GameManager.Instance.Profile.GetGold();
            long goldRequired = dataStats.GetGoldRequiredForLevelUp(equipment);
            int currentResourceQuantity = equipmentDataManager.GetResource(equipment.itemTemplate.resourceToUpgrade).quantity;
            int numberOfResourcesTypeRequired = dataStats.GetNumberOfResourceTypeRequiredForLevelUp(equipment);
            if (currentGold < goldRequired)
            {
                txtGoldToUpgrade.text = $"<color={stringColorAlert}>{SnowyyExtensions.FormatLargeNumber(currentGold)}</color>/{SnowyyExtensions.FormatLargeNumber(goldRequired)}";
            }
            else
            {
                txtGoldToUpgrade.text = $"{SnowyyExtensions.FormatLargeNumber(currentGold)}/{SnowyyExtensions.FormatLargeNumber(goldRequired)}";
            }
            if (currentResourceQuantity < numberOfResourcesTypeRequired)
            {
                txtResourceToUpgrade.text = $"<color={stringColorAlert}>{currentResourceQuantity}</color>/{numberOfResourcesTypeRequired}";
            }
            else
            {
                txtResourceToUpgrade.text = $"{currentResourceQuantity}/{numberOfResourcesTypeRequired}";
            }

            imgIconResourceToUpgrade.sprite = dataResources.dictResourceInfos[equipment.itemTemplate.resourceToUpgrade].iconResource;
        }
        //**
        //*** Display effects' description
        private void SetEffectDescriptions(Equipment equipment, Rarity[] arrRarity)
        {
            for (int i = 0; i < arrRarity.Length; i++)
            {
                dictEffectDisplays[arrRarity[i]].SetTextEffecTDes(dataRarityEffect.GetEffectDes(equipment.itemTemplate.arrEffects[(int)arrRarity[i]], arrRarity[i]));
            }
        }
        //**
        //*** Display Stats
        private void SetStatDisplay(Equipment equipment)
        {
            switch (equipment.EquipmentType)
            {
                case EquipmentType.Weapon:
                    imgStatDisplay.sprite = arrStatsBackground[0];
                    imgStatDisplay.SetNativeSize();
                    txtStatDisplay.text = $"+{dataStats.GetStatAmount(equipment, StatsType.ATTACK)}";
                    break;
                case EquipmentType.Backpack:
                    imgStatDisplay.sprite = arrStatsBackground[0];
                    imgStatDisplay.SetNativeSize();
                    txtStatDisplay.text = $"+{dataStats.GetStatAmount(equipment, StatsType.ATTACK)}";

                    break;
                case EquipmentType.Helmet:
                    imgStatDisplay.sprite = arrStatsBackground[1];
                    imgStatDisplay.SetNativeSize();
                    txtStatDisplay.text = $"+{dataStats.GetStatAmount(equipment, StatsType.HEALTH)}";

                    break;
                case EquipmentType.Armor:
                    imgStatDisplay.sprite = arrStatsBackground[1];
                    imgStatDisplay.SetNativeSize();
                    txtStatDisplay.text = $"+{dataStats.GetStatAmount(equipment, StatsType.HEALTH)}";

                    break;
                case EquipmentType.Boot:
                    imgStatDisplay.sprite = arrStatsBackground[1];
                    imgStatDisplay.SetNativeSize();
                    txtStatDisplay.text = $"+{dataStats.GetStatAmount(equipment, StatsType.HEALTH)}";
                    break;
                default:
                    Debug.Log($"This equipment type {equipment.EquipmentType} is not implemented to its display stats!!!");
                    break;
            }
        }
        public void SetEquipmentToShow(Equipment equipment, int index)
        {
            equipmentToShow = equipment;
            equipmentToShowIndex = index;
        }
        private void InitButtonRefund(Equipment equipment)
        {
            if (equipment.CurrentLevel > 0)
            {
                btnRefund.gameObject.SetActive(true);
                return;
            }
            btnRefund.gameObject.SetActive(false);
        }
        private void InitButtonEquip()
        {
            if (inventoryManager.IsItemInInventory(equipmentToShow))
            {
                txtBtnUn_Equip.text = "EQUIP";
                return;
            }
            txtBtnUn_Equip.text = "UNEQUIP";
        }
        private void OnClickBtnRefund()
        {
            //TODO: Implement refund feature
            SoundManager.Instance.PlaySoundButton();

            UiEquipmentSystemBrain.Instance.UiPopupRefund.SetBindedEquipment(equipmentToShow, equipmentToShowIndex);
            UiEquipmentSystemBrain.Instance.UiPopupRefund.Show(true);
        }
        private void OnClickBtnClose()
        {
            SoundManager.Instance.PlaySoundButton();
            OnClickBtnCloseBg();

        }
        private void OnClickBtnCloseBg()
        {
            OnClosePopupPressed();
            UiEquipmentSystemBrain.Instance.UiEquipmentSystem.OnRefresh?.Invoke();
        }
        private void OnClickBtnLevelUp()
        {
            SoundManager.Instance.PlaySoundButton();
            //*** CHECK CAN UPGRADE?
            long currentGold = GameManager.Instance.Profile.GetGold();
            long goldRequired = dataStats.GetGoldRequiredForLevelUp(equipmentToShow);
            if (currentGold < goldRequired)
            {
                //TODO: POPUP NOT ENOUGH GOLD
                Debug.LogError("NOT ENOUGH GOLD RUI BAN TRE OI");
                return;
            }
            int currentResourceQuantity = equipmentDataManager.GetResource(equipmentToShow.itemTemplate.resourceToUpgrade).quantity;
            int numberOfResourcesTypeRequired = dataStats.GetNumberOfResourceTypeRequiredForLevelUp(equipmentToShow);
            if (currentResourceQuantity < numberOfResourcesTypeRequired)
            {
                //TODO: POPUP NOT ENOUGH MATERIAL
                Debug.LogError("NOT ENOUGH MATERIAL RUI BAN TRE OI");
                return;
            }
            //TODO: ANIMATION
            //*** PAY GOLD AND RESOURCES
            GameManager.Instance.Profile.AddGold(-(int)goldRequired, StringHelper.PAY_GOLD_MAX_LEVEL);
            equipmentDataManager.SetResource(equipmentToShow.itemTemplate.resourceToUpgrade, new Resource(equipmentToShow.itemTemplate.resourceToUpgrade, currentResourceQuantity - numberOfResourcesTypeRequired));
            //*** APPLY
            equipmentToShow.IncreaseLevel();
            if (inventoryManager.IsItemInInventory(equipmentToShow))
            {
                equipmentDataManager.SetEquipmentByIndex(equipmentToShowIndex, equipmentToShow);
            }
            else
            {
                equipmentDataManager.SetCurrentEquippedEquipment(equipmentToShow.EquipmentType, equipmentToShow);
            }
            equipmentDataManager.SaveAllEquipments();
            Init(equipmentToShow);
        }
        private void OnClickBtnMaxLevel()
        {
            SoundManager.Instance.PlaySoundButton();
            //*** CHECK CAN UPGRADE?
            long currentGold = GameManager.Instance.Profile.GetGold();
            long goldRequired = dataStats.GetGoldRequiredForLevelUp(equipmentToShow);
            if (currentGold < goldRequired)
            {
                //TODO: POPUP NOT ENOUGH GOLD
                Debug.LogError("NOT ENOUGH GOLD RUI BAN TRE OI");
                return;
            }
            int currentResourceQuantity = equipmentDataManager.GetResource(equipmentToShow.itemTemplate.resourceToUpgrade).quantity;
            int numberOfResourcesTypeRequired = dataStats.GetNumberOfResourceTypeRequiredForLevelUp(equipmentToShow);
            if (currentResourceQuantity < numberOfResourcesTypeRequired)
            {
                //TODO: POPUP NOT ENOUGH MATERIAL
                Debug.LogError("NOT ENOUGH MATERIAL RUI BAN TRE OI");
                return;
            }

            //TODO: ANIMATION
            //var numberOfLevelsToUpgrade = dataStats.GetLevelCap(equipmentToShow.Rarity) - equipmentToShow.CurrentLevel;
            var numberOfLevelsToUpgrade = dataStats.GetGoldAndResourcesRequiredForMaxLevel(equipmentToShow, currentGold, currentResourceQuantity, out var requiredGold, out var requiredResource);

            //*** PAY GOLD AND RESOURCES
            GameManager.Instance.Profile.AddGold(-(int)requiredGold, StringHelper.PAY_GOLD_MAX_LEVEL);
            Debug.LogError("REQUIRED GOLD NE CHU BE DAN " + requiredGold);
            equipmentDataManager.SetResource(equipmentToShow.itemTemplate.resourceToUpgrade, new Resource(equipmentToShow.itemTemplate.resourceToUpgrade, currentResourceQuantity - requiredResource));
            //*** APPLY
            equipmentToShow.IncreaseLevel(numberOfLevelsToUpgrade);
            if (inventoryManager.IsItemInInventory(equipmentToShow))
            {
                equipmentDataManager.SetEquipmentByIndex(equipmentToShowIndex, equipmentToShow);
            }
            else
            {
                equipmentDataManager.SetCurrentEquippedEquipment(equipmentToShow.EquipmentType, equipmentToShow);
            }
            equipmentDataManager.SaveAllEquipments();
            Init(equipmentToShow);
        }
        private void OnClickBtnUnOrEquip()
        {
            if (isClicked)
            {
                return;
            }
            isClicked = true;

            SoundManager.Instance.PlaySoundButton();
            OnClosePopupPressed();

            if (inventoryManager.IsItemInInventory(equipmentToShow))
            {
                Equip();
                return;
            }
            UnEquip();
        }
        private void Equip()
        {
            equipmentDataManager.RemoveEquipment(equipmentToShow);
            Equipment currentEquipment = equipmentDataManager.GetCurrentEquippedEquipment(equipmentToShow.EquipmentType);
            if (currentEquipment != null)
            {
                equipmentDataManager.AddEquipment(currentEquipment);
            }

            equipmentDataManager.SaveAllEquipments();
            equipmentDataManager.SetCurrentEquippedEquipment(equipmentToShow.EquipmentType, equipmentToShow);

            equipSlotManager.InitEquip(equipmentToShow.EquipmentType);
            inventoryManager.Init();
            previewCharacter.Init();
            EventManager.Broadcast(Events.PlayerChangeEquipmentEvent);
        }
        private void UnEquip()
        {
            Equipment currentEquipment = equipmentDataManager.GetCurrentEquippedEquipment(equipmentToShow.EquipmentType);

            equipmentDataManager.AddEquipment(currentEquipment);

            equipmentDataManager.SaveAllEquipments();
            equipmentDataManager.SetCurrentEquippedEquipment(equipmentToShow.EquipmentType, null);

            equipSlotManager.InitEquip(equipmentToShow.EquipmentType);
            inventoryManager.Init();
            previewCharacter.Init();
            EventManager.Broadcast(Events.PlayerChangeEquipmentEvent);
        }
    }
    [Serializable]
    public class ItemSkillDisplay
    {
        public Image imgLock;
        public TextMeshProUGUI txtEffectDes;

        public void SetTextEffecTDes(string des)
        {
            txtEffectDes.text = des;
        }
        public void SetTextEffectColor(Color color)
        {
            txtEffectDes.color = color;
        }
        public void ToggleImgLock(bool isToggle)
        {
            imgLock.gameObject.SetActive(isToggle);
        }
    }
}
