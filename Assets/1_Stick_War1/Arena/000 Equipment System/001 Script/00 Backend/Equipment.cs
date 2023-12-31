using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowyy.EquipmentSystem
{
    [Serializable]
    public class Equipment
    {
        public string Id;
        public bool IsEquip;
        public int CurrentLevel;
        public EquipmentType EquipmentType;
        public Rarity Rarity;

        public ItemTemplate itemTemplate;

        public Equipment(string IdItemTemplate, bool isEquip, int currentLevel, EquipmentType equipmentType, Rarity rarity)
        {
            Id = IdItemTemplate;
            IsEquip = isEquip;
            CurrentLevel = currentLevel;
            EquipmentType = equipmentType;
            Rarity = rarity;
            this.itemTemplate = itemTemplate;
        }
        public Equipment(bool isEquip, int currentLevel, EquipmentType equipmentType, Rarity rarity)
        {
            IsEquip = isEquip;
            CurrentLevel = currentLevel;
            EquipmentType = equipmentType;
            Rarity = rarity;
            this.itemTemplate = itemTemplate;
        }
        public Equipment(EquipmentData data)
        {
            Id = data.Id;
            IsEquip = data.IsEquip;
            CurrentLevel = data.CurrentLevel;
            EquipmentType = data.EquipmentType;
            Rarity = data.Rarity;
        }
        public void SetItemTemplate(ItemTemplate itemTemplate)
        {
            this.itemTemplate = itemTemplate;
        }
        public void IncreaseLevel(int numberOfLevels = 1)
        {
            CurrentLevel += numberOfLevels;
        }
        public void ResetLevel()
        {
            CurrentLevel = 0;
        }
        public void SetIsEquip(bool isEquip)
        {
            IsEquip = isEquip;
        }
    }

}
