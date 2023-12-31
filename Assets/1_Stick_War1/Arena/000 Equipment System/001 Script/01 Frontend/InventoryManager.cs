using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unicorn;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Snowyy.Ultilities;
using System;

namespace Snowyy.EquipmentSystem
{
    public class InventoryManager : MonoBehaviour
    {
        [Title("BUTTONS", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private Button btnSort;
        [ SerializeField] private Button btnMerge;

        [Title("TEXT", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private TextMeshProUGUI txtSort;

        [Title("HOLDERS", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private ItemEquipment itemEquipmentPrefab;
        [SerializeField] private Transform layoutTransform;

        private List<Equipment> listAllEquipments;
        private List<Equipment> listAllEquipmentsSorted;
        private List<ItemEquipment> listItemEquipments = new List<ItemEquipment>();

        private OnRefreshDisplay onRefreshDisplay;

        private void Awake()
        {
            onRefreshDisplay += RefreshDisplay;
        }

        private void Start()
        {
            btnSort.onClick.AddListener(OnClickBtnSort);
            btnMerge.onClick.AddListener(OnClickBtnMerge);
        }

        public void Init()
        {
            EquipmentDataManager.Instance.LoadAllEqquipments();
            listAllEquipments = new List<Equipment>();
            listAllEquipments = EquipmentDataManager.Instance.ListAllEquipments.ToList();
            if (listAllEquipments.Count <= 0)
            {
                if (listItemEquipments.Count <= 0)
                {
                    return;
                }
                for (int i = 0; i < listItemEquipments.Count; i++)
                {
                    listItemEquipments[i].transform.parent = null;
                    SimplePool.Despawn(listItemEquipments[i].gameObject);

                    //Destroy(listItemEquipments[i].gameObject);
                }
                listItemEquipments.Clear();
                return;
            }
            if (listItemEquipments.Count < listAllEquipments.Count)
            {

                for (int i = listItemEquipments.Count; i < listAllEquipments.Count; i++)
                {
                    var itemEquipment = SimplePool.Spawn(itemEquipmentPrefab);
                    itemEquipment.transform.SetParent(layoutTransform);
                    itemEquipment.transform.localPosition = Vector3.zero;
                    itemEquipment.transform.localRotation = Quaternion.identity;
                    itemEquipment.transform.localScale = Vector3.one;
                    listItemEquipments.Add(itemEquipment);
                }
            }
            else
            {
                for (int i = listItemEquipments.Count - 1; i > listAllEquipments.Count - 1; i--)
                {
                    var itemEquipment = listItemEquipments[i];
                    listItemEquipments.Remove(itemEquipment);
                    SimplePool.Despawn(itemEquipment.gameObject);
                }
            }
            onRefreshDisplay?.Invoke();
        }

        private void RefreshDisplay()
        {
            int currentIndex = EquipmentDataManager.Instance.GetCurrentSortTypeIndex();
            DisplaySortText(currentIndex);
            Sort(currentIndex);
            DisplayInfo(listAllEquipmentsSorted);
        }

        public bool IsItemInInventory(Equipment equipment)
        {
            return listAllEquipments.Contains(equipment);
        }

        private void Sort(int sortType)
        {
            listAllEquipmentsSorted = sortType switch
            {
                (int)SortType.BYRARITY => listAllEquipments.OrderByDescending(equipment => equipment.Rarity).ToList(),
                (int)SortType.BYSLOT => listAllEquipments.OrderBy(equipment => equipment.EquipmentType).ToList(),
                (int)SortType.BYLEVEL => listAllEquipments.OrderByDescending(equipment => equipment.CurrentLevel).ToList(),
                _ => listAllEquipments,
            };
        }

        private void DisplayInfo(List<Equipment> listAllEquipments)
        {
            for (int i = 0; i < listAllEquipments.Count; i++)
            {
                listItemEquipments[i].Init(listAllEquipments[i]);
            }
        }
        private void OnClickBtnSort()
        {
            var currentIndex = EquipmentDataManager.Instance.GetCurrentSortTypeIndex();
            currentIndex++;
            if (currentIndex >= Enum.GetNames(typeof(SortType)).Length)
            {
                currentIndex = 0;
            }
            EquipmentDataManager.Instance.SetCurrentSortTypeIndex(currentIndex);
            onRefreshDisplay?.Invoke();
        }

        private void OnClickBtnMerge()
        {
            SoundManager.Instance.PlaySoundButton();
            UiEquipmentSystemBrain.Instance.UiMergeEquipment.Show(true);
        }

        private void DisplaySortText(int index)
        {
            txtSort.text = SnowyyExtensions.DisplayStringSortType(index);
        }

    }
}
