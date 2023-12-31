using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arena;
using Castle.Core.Internal;
using Spicyy.System;

namespace Snowyy.EquipmentSystem
{
    public class EquipmentGenerator : MonoBehaviour
    {
        public static EquipmentGenerator Instance;

        public DataItemTemplate dataItemTemplate;
        public int randomSeed = 10000;
        public Rarity[] arrRarity;

        private void Awake()
        {
            Instance = this;
        }

        public Equipment GenerateEquipment(CrateType crateType)
        {
            var equipmentType = GenerateEquipmentType();
            Debug.Log("RESULT CUA CHU BE DAN NE " + equipmentType);
            var weaponType = GenerateWeaponType();
            ItemTemplate itemTemplate = equipmentType == EquipmentType.Weapon
                ? dataItemTemplate.dictItemTemplates[equipmentType].Find(item => item.weaponType == weaponType)
                : dataItemTemplate.dictItemTemplates[equipmentType][Random.Range(0, dataItemTemplate.dictItemTemplates[equipmentType].Length)];

            var rarity = GetRarity(crateType);

            var newEquipment =
                EquipmentDataManager.Instance.CreateNewEquipment(itemTemplate.Id, 0, equipmentType, rarity);
            newEquipment.SetItemTemplate(itemTemplate);
            EquipmentDataManager.Instance.AddEquipment(newEquipment);
            EquipmentDataManager.Instance.SaveAllEquipments();
            return newEquipment;
        }

        private EquipmentType GenerateEquipmentType()
        {
            var randomResult = Random.Range(0, randomSeed);
            Debug.Log((randomResult) + " AI ZO BRO " + randomSeed * 0.18f);
            if (randomResult <= 0.18f * randomSeed)
            {
                return EquipmentType.Backpack;
            }

            if (randomResult > 0.36f * randomSeed && randomResult <= 0.36f * randomSeed)
            {
                return EquipmentType.Helmet;
            }

            if (randomResult > 0.54f * randomSeed && randomResult <= 0.54f * randomSeed)
            {
                return EquipmentType.Armor;
            }

            if (randomResult > 0.72f * randomSeed && randomResult <= 0.72f * randomSeed)
            {
                return EquipmentType.Boot;
            }

            return EquipmentType.Weapon;
        }

        private WeaponType GenerateWeaponType()
        {
            var randomResult = Random.Range(0, randomSeed);
            // 6 = TOTAL WEAPON TYPE
            var min = randomSeed / 6;
            if (randomResult <= min)
            {
                return WeaponType.PISTOL;
            }
            else if (randomResult > min && randomResult <= 2 * min)
            {
                return WeaponType.SHOTGUN;
            }
            else if (randomResult > 2 * min && randomResult <= 3 * min)
            {
                return WeaponType.UZI;
            }
            else if (randomResult > 3 * min && randomResult <= 4 * min)
            {
                return WeaponType.AK47;
            }
            else if (randomResult > 4 * min & randomResult <= 5 * min)
            {
                return WeaponType.MACHINEGUN;
            }
            else
            {
                return WeaponType.FLAMEGUN;
            }
        }

        private Rarity GetRarity(CrateType crateType)
        {
            switch (crateType)
            {
                case CrateType.NORMAL:
                    return GenerateNormalCrate();
                case CrateType.ADVANCE:
                    return GenerateAdvanceCrate();
                case CrateType.SUPER:
                    return GenerateSuperCrate();
                default:
                    Debug.Log($"This CrateType {crateType} is not implemented");
                    return Rarity.COMMON;
            }
        }

        private Rarity GenerateNormalCrate()
        {
            var randomResult = Random.Range(0, randomSeed);
            if (randomResult <= 0.75 * randomSeed)
            {
                return Rarity.COMMON;
            }
            else
            {
                return Rarity.RARE;
            }
        }

        private Rarity GenerateAdvanceCrate()
        {
            var currentCounter = SurvivorShopDataManager.Instance.GetAdvanceCrateEpicCounter();
            SurvivorShopDataManager.Instance.SetAdvanceCrateEpicCounter(currentCounter + 1);
            if (currentCounter + 1 == 10)
            {
                SurvivorShopDataManager.Instance.SetAdvanceCrateEpicCounter(0);
                return Rarity.EPIC;
            }

            var randomResult = Random.Range(0, randomSeed);
            if (randomResult <= 0.6 * randomSeed)
            {
                return Rarity.COMMON;
            }
            else if (randomResult > 0.6 * randomSeed && randomResult <= 0.95 * randomSeed)
            {
                return Rarity.RARE;
            }
            else
            {
                SurvivorShopDataManager.Instance.SetAdvanceCrateEpicCounter(0);
                return Rarity.EPIC;
            }
        }

        private Rarity GenerateSuperCrate()
        {
            var currentMythical = SurvivorShopDataManager.Instance.GetSuperCrateMythicalCounter();
            SurvivorShopDataManager.Instance.SetSuperCrateMythicalCounter(currentMythical + 1);
            if (currentMythical + 1 == 30)
            {
                SurvivorShopDataManager.Instance.SetSuperCrateMythicalCounter(0);
                return Rarity.MYTHICAL;
            }

            var currentEpic = SurvivorShopDataManager.Instance.GetSuperCrateEpicCounter();
            SurvivorShopDataManager.Instance.SetSuperCrateEpicCounter(currentEpic + 1);
            if (currentEpic + 1 == 10)
            {
                SurvivorShopDataManager.Instance.SetSuperCrateEpicCounter(0);
                return Rarity.EPIC;
            }

            var randomResult = Random.Range(0, randomSeed);
            if (randomResult <= 0.55 * randomSeed)
            {
                return Rarity.COMMON;
            }
            else if (randomResult > 0.55 * randomSeed && randomResult <= 0.9 * randomSeed)
            {
                return Rarity.RARE;
            }
            else if (randomResult > 0.9 * randomSeed && randomResult <= 0.99 * randomSeed)
            {
                SurvivorShopDataManager.Instance.SetSuperCrateEpicCounter(0);
                return Rarity.EPIC;
            }
            else
            {
                SurvivorShopDataManager.Instance.SetSuperCrateMythicalCounter(0);
                return Rarity.MYTHICAL;
            }
        }

        [Button]
        public void GenerateNewEquipment(EquipmentType equipmentType)
        {
            var rarity = GenerateRarity();
            var itemTemplate = dataItemTemplate.dictItemTemplates[equipmentType][Random.Range(0, dataItemTemplate.dictItemTemplates[equipmentType].Length)];
            var newEquipment = EquipmentDataManager.Instance.CreateNewEquipment(itemTemplate.Id, 0, equipmentType, rarity);
            newEquipment.SetItemTemplate(itemTemplate);
            EquipmentDataManager.Instance.AddEquipment(newEquipment);
            Debug.Log("GenerateNewEquipment");
        }

        [Button]
        public void GenerateResources()
        {
            for (int i = 0; i < (int)ResourceType.Blueprint_Boot + 1; i++)
            {
                Resource resource = new Resource((ResourceType)i, Random.Range(1, 1000));
                EquipmentDataManager.Instance.SetResource((ResourceType)i, resource);
            }
            Debug.Log("GenerateResources");
        }

        [Button]
        public void GenerateResource(ResourceType type, int quantity)
        {
            EquipmentDataManager.Instance.SetResource(type, new Resource(type, quantity));
        }

        private Rarity GenerateRarity()
        {
            return arrRarity[Random.Range(0, arrRarity.Length)];
        }

    }
}
