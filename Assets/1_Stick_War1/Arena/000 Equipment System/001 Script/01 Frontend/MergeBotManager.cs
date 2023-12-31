using Sirenix.OdinInspector;
using Snowyy.EquipmentSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unicorn;
using UnityEngine;
using UnityEngine.UI;

namespace Snowyy.MergeSystem
{
    public class MergeBotManager : MonoBehaviour
    {
        [Title("Buttons")]
        [SerializeField] private Button btnFilterClose;
        [SerializeField] private Button btnFilter;
        [SerializeField] private Button btnMergeAll;
        [SerializeField] private ButtonFilter[] btnFilterTypes;
        [Title("Data")]
        [SerializeField] private DataRarityEffect dataRarityEffect;
        [SerializeField] private DataItemTemplate dataItemTemplate;
        [Title("Others")]
        [SerializeField] private ItemMergeEquipment itemEquipmentPrefab;
        [SerializeField] private Transform layoutTransform;
        [SerializeField] private GameObject filterChoices;
        [SerializeField] private GameObject[] arrFilterIcons;

        private FilterType currentFilterType;

        private List<Equipment> listAllEquipmentsIncludeEquipped = new List<Equipment>();
        private List<ItemMergeEquipment> listItemUiEquipment = new List<ItemMergeEquipment>();

        public OnRefreshDisplay OnRefresh { get; private set; }

        public delegate void OnMergeItemChangedEvent(Equipment equipment);
        public OnMergeItemChangedEvent OnMergeItemChanged { get; private set; }
        private void Start()
        {
            btnMergeAll.onClick.AddListener(OnClickBtnMergeAll);
            btnFilter.onClick.AddListener(OnClickBtnFilter);
            btnFilterClose.onClick.AddListener(() =>
            {
                ToggleFilterChoices(false);
            });
            for (int i = 0; i < btnFilterTypes.Length; i++)
            {
                int id = i;
                btnFilterTypes[i].btnChoose.onClick.AddListener(() => OnClickBtnBehaviour(id));
            }
            OnRefresh += Refresh;
            OnMergeItemChanged += Refresh;
        }
        public void Init()
        {
            SetFilterType(FilterType.ALL);

            ToggleFilterChoices(false);

            EquipmentDataManager.Instance.LoadAllEqquipments();
            listAllEquipmentsIncludeEquipped = new List<Equipment>();
            listAllEquipmentsIncludeEquipped = EquipmentDataManager.Instance.ListAllEquipments.ToList();
            for (int i = 0; i < Enum.GetValues(typeof(EquipmentType)).Length; i++)
            {
                var equipment =
                    EquipmentDataManager.Instance.GetCurrentEquippedEquipment((EquipmentType)Enum.GetValues(typeof(EquipmentType)).GetValue(i));
                if (equipment != null)
                {
                    listAllEquipmentsIncludeEquipped.Add(equipment);
                }
            }
            Refresh();
        }
        private void Refresh(Equipment equipment)
        {
            var tempList = FilterByType(equipment);
            if (tempList.Count <= 0)
            {
                if (listItemUiEquipment.Count <= 0)
                {
                    return;
                }
                for (int i = 0; i < listItemUiEquipment.Count; i++)
                {
                    SimplePool.Despawn(listItemUiEquipment[i].gameObject);
                }
                listItemUiEquipment.Clear();
                return;
            }


            if (listItemUiEquipment.Count < tempList.Count)
            {
                for (int i = listItemUiEquipment.Count; i < tempList.Count; i++)
                {
                    var itemEquipment = SimplePool.Spawn(itemEquipmentPrefab);
                    itemEquipment.transform.SetParent(layoutTransform);
                    itemEquipment.transform.localScale = Vector3.one;
                    listItemUiEquipment.Add(itemEquipment);
                }
            }
            else
            {
                for (int i = listItemUiEquipment.Count - 1; i > tempList.Count - 1; i--)
                {
                    var itemEquipment = listItemUiEquipment[i];
                    listItemUiEquipment.Remove(itemEquipment);
                    SimplePool.Despawn(itemEquipment.gameObject);
                }
            }

            tempList = Sort(tempList);
            DisplayWhileMerging(tempList);
        }
        private void Refresh()
        {
            var tempList = FilterByType(currentFilterType);

            if (tempList.Count <= 0)
            {
                if (listItemUiEquipment.Count <= 0)
                {
                    return;
                }
                for (int i = 0; i < listItemUiEquipment.Count; i++)
                {
                    SimplePool.Despawn(listItemUiEquipment[i].gameObject);
                }
                listItemUiEquipment.Clear();
                return;
            }


            if (listItemUiEquipment.Count < tempList.Count)
            {
                for (int i = listItemUiEquipment.Count; i < tempList.Count; i++)
                {
                    var itemEquipment = SimplePool.Spawn(itemEquipmentPrefab);
                    itemEquipment.transform.SetParent(layoutTransform);
                    itemEquipment.transform.localScale = Vector3.one;
                    listItemUiEquipment.Add(itemEquipment);
                }
            }
            else
            {
                for (int i = listItemUiEquipment.Count - 1; i > tempList.Count - 1; i--)
                {
                    var itemEquipment = listItemUiEquipment[i];
                    listItemUiEquipment.Remove(itemEquipment);
                    SimplePool.Despawn(itemEquipment.gameObject);
                }
            }

            tempList = Sort(tempList);
            Display(tempList);
        }

        private List<Equipment> FilterByType(FilterType filterType)
        {
            var resultList = new List<Equipment>();
            switch (filterType)
            {
                case FilterType.ALL:
                    resultList = listAllEquipmentsIncludeEquipped.FindAll(item => item.Rarity != Rarity.LEGENDARY).ToList();
                    break;
                case FilterType.WEAPON:
                    resultList = listAllEquipmentsIncludeEquipped.FindAll(item => CheckTypeAndRarity(item, EquipmentType.Weapon)
                    ).ToList();
                    break;
                case FilterType.BACKPACK:
                    resultList = listAllEquipmentsIncludeEquipped.FindAll(item => CheckTypeAndRarity(item, EquipmentType.Backpack)
                    ).ToList();
                    break;
                case FilterType.HELMET:
                    resultList = listAllEquipmentsIncludeEquipped.FindAll(item => CheckTypeAndRarity(item, EquipmentType.Helmet)
                    ).ToList();
                    break;
                case FilterType.ARMOR:
                    resultList = listAllEquipmentsIncludeEquipped.FindAll(item => CheckTypeAndRarity(item, EquipmentType.Armor)
                    ).ToList();
                    break;
                case FilterType.BOOT:
                    resultList = listAllEquipmentsIncludeEquipped.FindAll(item => CheckTypeAndRarity(item, EquipmentType.Boot)
                    ).ToList();
                    break;
            }

            return resultList;
        }
        private List<Equipment> FilterByType(Equipment equipment)
        {
            switch (equipment.EquipmentType)
            {
                case EquipmentType.Weapon:
                    return listAllEquipmentsIncludeEquipped.FindAll(item => CheckTypeAndRarity(item, EquipmentType.Weapon, equipment.Rarity)).ToList();
                case EquipmentType.Backpack:
                    return listAllEquipmentsIncludeEquipped.FindAll(item => CheckTypeAndRarity(item, EquipmentType.Backpack, equipment.Rarity)).ToList();
                case EquipmentType.Helmet:
                    return listAllEquipmentsIncludeEquipped.FindAll(item => CheckTypeAndRarity(item, EquipmentType.Helmet, equipment.Rarity)).ToList();
                case EquipmentType.Armor:
                    return listAllEquipmentsIncludeEquipped.FindAll(item => CheckTypeAndRarity(item, EquipmentType.Armor, equipment.Rarity)).ToList();
                case EquipmentType.Boot:
                    return listAllEquipmentsIncludeEquipped.FindAll(item => CheckTypeAndRarity(item, EquipmentType.Boot, equipment.Rarity)).ToList();
                default:
                    Debug.LogError($"This equipment type {equipment.EquipmentType} hasn't been implemented!!!");
                    return new List<Equipment>();
            }
        }
        private bool CheckTypeAndRarity(Equipment item, EquipmentType equipmentType)
        {
            if (item.EquipmentType == equipmentType && item.Rarity != Rarity.LEGENDARY) return true;
            else return false;
        }
        private bool CheckTypeAndRarity(Equipment item, EquipmentType equipmentType, Rarity maxRarity)
        {
            if (item.EquipmentType == equipmentType && (int)item.Rarity <= (int)maxRarity) return true;
            else return false;
        }
        private List<Equipment> Sort(List<Equipment> listEquipments)
        {
            return listEquipments.OrderByDescending(item => item.Rarity).ToList();
        }
        private void Display(List<Equipment> listAllEquipments)
        {
            for (int i = 0; i < listAllEquipments.Count; i++)
            {
                listItemUiEquipment[i].Init(listAllEquipments[i]);
            }
        }
        private void DisplayWhileMerging(List<Equipment> listAllEquipments)
        {
            var topManager = UiEquipmentSystemBrain.Instance.UiMergeEquipment.TopManager;
            for (int i = 0; i < listAllEquipments.Count; i++)
            {
                if (topManager.CheckIncludedInTopItems(listAllEquipments[i]))
                {
                    listItemUiEquipment[i].Init(listAllEquipments[i], false, true);
                }
                else if (!topManager.CheckRarityWithCurrentItem(listAllEquipments[i]))
                {
                    listItemUiEquipment[i].Init(listAllEquipments[i], true, false);
                }
                else
                {
                    listItemUiEquipment[i].Init(listAllEquipments[i]);
                }
            }
        }
        private void ToggleFilterChoices(bool isToggle)
        {
            filterChoices.SetActive(isToggle);

            for (int i = 0; i < arrFilterIcons.Length; i++)
            {
                if (i == (int)currentFilterType)
                {
                    arrFilterIcons[i].SetActive(true);
                }
                else
                {
                    arrFilterIcons[i].SetActive(false);
                }
            }

            for (int i = 0; i < btnFilterTypes.Length; i++)
            {
                if ((FilterType)i == currentFilterType)
                {
                    btnFilterTypes[i].ToggleImageBackground(true);
                }
                else
                {
                    btnFilterTypes[i].ToggleImageBackground(false);

                }
            }

        }
        private void SetFilterType(FilterType filterType)
        {
            currentFilterType = filterType;
        }
        private void OnClickBtnMergeAll()
        {
            //TODO: IMPLEMENT MERGE ALL
        }
        private void OnClickBtnFilter()
        {
            SoundManager.Instance.PlaySoundButton();
            if (filterChoices.activeSelf)
            {
                ToggleFilterChoices(false);
            }
            else
            {
                ToggleFilterChoices(true);
            }
        }
        private void OnClickBtnBehaviour(int id)
        {
            SoundManager.Instance.PlaySoundButton();
            currentFilterType = (FilterType)id;
            switch ((FilterType)id)
            {
                case FilterType.ALL:
                    break;
                case FilterType.WEAPON:
                    break;
                case FilterType.BACKPACK:
                    break;
                case FilterType.HELMET:
                    break;
                case FilterType.ARMOR:
                    break;
                case FilterType.BOOT:
                    break;
            }
            ToggleFilterChoices(false);
            OnRefresh?.Invoke();
        }

    }
    [Serializable]
    public class ButtonFilter
    {
        public Button btnChoose;
        public Image imgBackground;
        public Sprite[] spriteBackground;

        public void ToggleImageBackground(bool isToggle)
        {
            if (isToggle)
            {
                imgBackground.sprite = spriteBackground[1];
                return;
            }
            imgBackground.sprite = spriteBackground[0];
        }
    }
    public enum FilterType
    {
        ALL,
        WEAPON,
        BACKPACK,
        HELMET,
        ARMOR,
        BOOT,
    }
}
