using Castle.Core.Internal;
using NSubstitute.Routing.Handlers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Snowyy.EquipmentSystem
{
    [Serializable]
    public class EquipmentData
    {
        public string Id;
        public bool IsEquip;
        public int CurrentLevel;
        public EquipmentType EquipmentType;
        public Rarity Rarity;

        public EquipmentData()
        {

        }

        public EquipmentData(string id, bool isEquip, int currentLevel, EquipmentType equipmentType, Rarity rarity)
        {
            Id = id;
            IsEquip = isEquip;
            CurrentLevel = currentLevel;
            EquipmentType = equipmentType;
            Rarity = rarity;
        }
        public EquipmentData(Equipment equipment)
        {
            Id = equipment.Id;
            IsEquip = equipment.IsEquip;
            CurrentLevel = equipment.CurrentLevel;
            EquipmentType = equipment.EquipmentType;
            Rarity = equipment.Rarity;
        }
    }
    public class EquipmentDataManager : MonoBehaviour
    {
        public static EquipmentDataManager Instance;

        public DataItemTemplate dataItemTemplate;
        public List<Equipment> ListAllEquipments;

        //MONO METHODS
        //***
        //**
        private void Awake()
        {
            Instance = this;

        }

        //PUBLIC METHODS
        //***
        //**
        [Button]
        [PropertyOrder(0)]
        public void SaveAllEquipments()
        {
            List<EquipmentData> listEquipmentData = new List<EquipmentData>();
            for (int i = 0; i < ListAllEquipments.Count; i++)
            {
                listEquipmentData.Add(new EquipmentData(ListAllEquipments[i]));
            }
            ES3.Save(StringHelper.LIST_ALL_EQUIPMENT, listEquipmentData);
            Debug.Log("EASY SAVE 3 ALL EQUIPMENTS SAVED");
        }
        [Button]
        [PropertyOrder(0)]
        public void LoadAllEqquipments()
        {
            ListAllEquipments = new List<Equipment>();
            try
            {
                var listEquipmentData = ES3.Load<List<EquipmentData>>(StringHelper.LIST_ALL_EQUIPMENT);

                for (int i = 0; i < listEquipmentData.Count; i++)
                {
                    ListAllEquipments.Add(new Equipment(listEquipmentData[i]));
                }

                foreach (Equipment equipment in ListAllEquipments)
                {
                    Debug.Log(equipment.Id);
                    var itemTemplate = dataItemTemplate.dictItemTemplates[equipment.EquipmentType].Find(item => item.Id == equipment.Id);
                    if (itemTemplate.Id == "")
                    {
                        ListAllEquipments.Remove(equipment);
                    }
                    else
                    {
                        equipment.SetItemTemplate(itemTemplate);
                    }

                }
                Debug.Log("EASY SAVE 3 ALL EQUIPMENTS LOADED");
            }
            catch (FileNotFoundException)
            {
                ListAllEquipments = new List<Equipment>();
                Debug.LogError("The file doesn't existed!! Return empty list");
            }
        }

        public Equipment CreateNewEquipment(string id, int currentLevel, EquipmentType equipmentType, Rarity rarity, bool isEquip = false)
        {
            return new Equipment(id, isEquip, currentLevel, equipmentType, rarity);
        }
        public Equipment CreateNewEquipment(int currentLevel, EquipmentType equipmentType, Rarity rarity, bool isEquip = false)
        {
            return new Equipment(isEquip, currentLevel, equipmentType, rarity);
        }
        public bool IsInventory(Equipment equipment)
        {
            return ListAllEquipments.Contains(equipment);
        }
        public void AddEquipment(Equipment equipment)
        {
            if (equipment != null)
            {
                equipment.SetIsEquip(false);
            }
            ListAllEquipments.Add(equipment);
        }
        public void RemoveEquipment(Equipment equipment)
        {
            ListAllEquipments.Remove(equipment);
        }
        public Equipment GetEquipmentByIndex(int index)
        {
            return ListAllEquipments[index];
        }
        public void SetEquipmentByIndex(int index, Equipment equipment)
        {
            ListAllEquipments[index] = equipment;
        }
        public int FindEquipmentIndex(Equipment equipment)
        {
            return ListAllEquipments.IndexOf(equipment);
        }
        public int GetCurrentSortTypeIndex()
        {
            return ES3.Load<int>(StringHelper.CURRENT_INVENTORY_SORT_TYPE, 0);
        }
        public void SetCurrentSortTypeIndex(int index)
        {
            ES3.Save(StringHelper.CURRENT_INVENTORY_SORT_TYPE, index);
        }
        public Equipment GetCurrentEquippedEquipment(EquipmentType equipmentType)
        {
            try
            {
                var equipment = ES3.Load($"{StringHelper.CURRENT_EQUIPPED} {equipmentType}", (Equipment)null); ;
                if (equipment == null)
                {
                    return (Equipment)null;
                }
                else
                {
                    var itemTemplate = dataItemTemplate.dictItemTemplates[equipment.EquipmentType].Find(item => item.Id == equipment.Id);
                    if (itemTemplate.Id == "")
                    {
                        return (Equipment)null;
                    }
                    else
                    {
                        equipment.SetItemTemplate(itemTemplate);
                        equipment.SetIsEquip(true);
                        return equipment;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("The file doesn't existed!! Return null");
                return (Equipment)null;
            }
        }
        public void SetCurrentEquippedEquipment(EquipmentType equipmentType, Equipment equipment)
        {
            if (equipment != null)
            {
                equipment.SetIsEquip(true);
            }
            ES3.Save($"{StringHelper.CURRENT_EQUIPPED} {equipmentType}", equipment);
        }
        public Resource GetResource(ResourceType resourceType)
        {
            try
            {
                var resource = ES3.Load($"{StringHelper.INFO_RESOURCE} {resourceType}", (Resource)null);
                if (resource == null)
                {
                    return new Resource(resourceType, 0);
                }
                else
                {
                    return resource;
                }
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("The file doesn't existed!! Return null");
                return new Resource(resourceType, 0);
            }
        }
        public void SetResource(ResourceType resourceType, Resource resource)
        {
            ES3.Save($"{StringHelper.INFO_RESOURCE} {resourceType}", resource);
        }

        public void AddResource(ResourceType resourceType, int quantity)
        {
            string resourceKey = $"{StringHelper.INFO_RESOURCE} {resourceType}";

            // Kiểm tra xem đã tồn tại giá trị tài nguyên hay chưa
            if (ES3.KeyExists(resourceKey))
            {
                // Nếu đã tồn tại, lấy giá trị tài nguyên hiện tại
                Resource currentResource = ES3.Load<Resource>(resourceKey);

                // Cộng thêm số lượng tài nguyên mới vào tài nguyên hiện tại
                currentResource.quantity += quantity;

                // Lưu tài nguyên đã cập nhật
                ES3.Save(resourceKey, currentResource);
            }
            else
            {
                // Nếu chưa tồn tại, lưu tài nguyên mới
                SetResource(resourceType, new Resource(resourceType, quantity));
            }
        }

        //PRIVATE METHODS
        //***
        //**

    }
}