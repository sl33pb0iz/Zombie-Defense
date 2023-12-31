using Sirenix.OdinInspector;
using Snowyy.EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowyy.MergeSystem
{
    public class MergeTopManager : MonoBehaviour
    {
        [Title("Others")]
        [SerializeField] private ItemMergeResultEquipment itemResultEquipment;
        [SerializeField] private ItemCurrentEquipment itemCurrentEquipment;
        [SerializeField] private ItemMergedMaterial[] itemMergedMaterials;
        [SerializeField] private GameObject mergeMaterialsHolder;

        public ItemMergedMaterial[] ItemMergedMaterials => itemMergedMaterials;
        public ItemCurrentEquipment ItemCurrentEquipment => itemCurrentEquipment;
        public void Init()
        {
            itemCurrentEquipment.Init();
            itemResultEquipment.Init();
            mergeMaterialsHolder.SetActive(false);
        }
        public void SelectMergedEquipment(Equipment equipment)
        {
            itemCurrentEquipment.Init(equipment);
            itemResultEquipment.Init(equipment);
            mergeMaterialsHolder.SetActive(true);

            for (int i = 0; i < itemMergedMaterials.Length; i++)
            {
                itemMergedMaterials[i].Init();
            }
        }
        public void RemoveMergedEquipment()
        {
            itemCurrentEquipment.Init();
            itemResultEquipment.Init();
            mergeMaterialsHolder.SetActive(false);

            for (int i = 0; i < itemMergedMaterials.Length; i++)
            {
                itemMergedMaterials[i].Init();
            }
        }
        public bool CheckCanMerge()
        {
            if (itemCurrentEquipment.BindedEquipment == null) return false;
            for (int i = 0; i < itemMergedMaterials.Length; i++)
            {
                if (itemMergedMaterials[i].BindedEquipment == null) return false;
            }
            return true;
        }
        public bool CheckIncludedInTopItems(Equipment equipment)
        {
            if (equipment == itemCurrentEquipment.BindedEquipment) return true;
            for (int i = 0; i < itemMergedMaterials.Length; i++)
            {
                if (equipment == itemMergedMaterials[i].BindedEquipment) return true;
            }
            return false;
        }
        public bool CheckIncludedInMaterialSlot(Equipment equipment)
        {
            for (int i = 0; i < itemMergedMaterials.Length; i++)
            {
                if (equipment == itemMergedMaterials[i].BindedEquipment) return true;
            }
            return false;
        }
        public bool CheckRarityWithCurrentItem(Equipment equipment)
        {
            if (equipment.Rarity == itemCurrentEquipment.BindedEquipment.Rarity) return true;
            return false;
        }
        public bool CheckMaterialsSlot()
        {
            for (int i = 0; i < itemMergedMaterials.Length; i++)
            {
                if (itemMergedMaterials[i].BindedEquipment == null)
                {
                    return true;
                }
            }
            return false;
        }
        public ItemMergedMaterial GetMaterialEmptySlot()
        {
            for (int i = 0; i < itemMergedMaterials.Length; i++)
            {
                if (itemMergedMaterials[i].BindedEquipment == null)
                {
                    return itemMergedMaterials[i];
                }
            }
            return null;
        }
        public ItemMergedMaterial GetMaterialSlot(Equipment equipment)
        {
            for (int i = 0; i < itemMergedMaterials.Length; i++)
            {
                if (itemMergedMaterials[i].BindedEquipment == equipment)
                {
                    return itemMergedMaterials[i];
                }
            }
            return null;
        }
    }
}
