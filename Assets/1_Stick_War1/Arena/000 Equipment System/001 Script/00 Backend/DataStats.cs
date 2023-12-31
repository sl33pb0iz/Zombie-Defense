using Sirenix.OdinInspector;
using Snowyy.EquipmentSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unicorn;
using UnityEngine;

namespace Snowyy.EquipmentSystem
{
    [CreateAssetMenu(fileName = "Stats Data", menuName = "Equipment System Data/Stats Data")]
    public class DataStats : SerializedScriptableObject
    {
        [Title("RARITY BASED UPGRADE RESOURCES REQUIRED", titleAlignment: TitleAlignments.Centered)]
        [PropertyOrder(0)]
        [InfoBox("This is where you modify upgrade resources required  for each level")]
        public Dictionary<ResourceType, ResourcesUpgrade[]> dictUpgradeResourcesPerLvl;

        [Title("RARITY BASED LEVEL CAP", titleAlignment: TitleAlignments.Centered)]
        [PropertyOrder(0)]
        [InfoBox("This is where you modify level cap for each rarity")]
        public Dictionary<Rarity, int> dictLevelCapRarityBased;

        [Title("RARITY BASED BASE STATS", titleAlignment: TitleAlignments.Centered)]
        [PropertyOrder(1)]
        [InfoBox("These dictionaries stores rarity based base stats of <b>DIFFERENT EQUIPMENT TYPE</b>\nEach int array length must equal to the number of Rarity", InfoMessageType.Info)]
        [LabelText("WEAPON")]
        public Dictionary<StatsType, BaseStat> dictBaseStats_Weapon;

        [PropertyOrder(1)]
        [LabelText("Backpack")]
        public Dictionary<StatsType, BaseStat> dictBaseStats_Backpack;

        [PropertyOrder(1)]
        [LabelText("HELMET")]
        public Dictionary<StatsType, BaseStat> dictBaseStats_Helmet;

        [PropertyOrder(1)]
        [LabelText("ARMOR")]
        public Dictionary<StatsType, BaseStat> dictBaseStats_Armor;

        [PropertyOrder(1)]
        [LabelText("BOOT")]
        public Dictionary<StatsType, BaseStat> dictBaseStats_Boot;

        public int GetStatAmount(Equipment equipment, StatsType statsType)
        {
            return GetStatAmount(equipment.EquipmentType, statsType, equipment.Rarity, equipment.CurrentLevel);
        }
        public int GetStatAmount(EquipmentType equipmentType, StatsType statsType, Rarity rarity, int CurrentLevel)
        {
            var dictTemp = GetBaseStatDictionary(equipmentType);
            switch (statsType)
            {
                case StatsType.ATTACK:
                    return dictTemp[statsType].baseStat[(int)rarity]
                        + CurrentLevel * dictTemp[statsType].bonusAmountPerLevel[(int)rarity];
                case StatsType.HEALTH:
                    return dictTemp[statsType].baseStat[(int)rarity]
                        + CurrentLevel * dictTemp[statsType].bonusAmountPerLevel[(int)rarity];
                case StatsType.DEFENSE:
                    return dictTemp[statsType].baseStat[(int)rarity]
                        + CurrentLevel * dictTemp[statsType].bonusAmountPerLevel[(int)rarity];
                default:
                    Debug.Log($"This stats type {statsType} is not implemented!!");
                    return 0;
            }
        }
        public Dictionary<StatsType, BaseStat> GetBaseStatDictionary(EquipmentType equipmentType)
        {
            switch (equipmentType)
            {
                case EquipmentType.Weapon:
                    return dictBaseStats_Weapon;
                case EquipmentType.Backpack:
                    return dictBaseStats_Backpack;
                case EquipmentType.Helmet:
                    return dictBaseStats_Helmet;
                case EquipmentType.Armor:
                    return dictBaseStats_Armor;
                case EquipmentType.Boot:
                    return dictBaseStats_Boot;
                default:
                    Debug.Log($"This equipment type {equipmentType} doesn't have a dictionary!!" +
                        $"\nIf u want, it's your job to implement it");
                    return null;
            }
        }
        public bool IsCanLevelUp(Equipment equipment)
        {
            bool enoughGold = GameManager.Instance.Profile.GetGold() >= GetGoldRequiredForLevelUp(equipment);
            bool enoughResources = EquipmentDataManager.Instance.GetResource(equipment.itemTemplate.resourceToUpgrade).quantity >= GetNumberOfResourceTypeRequiredForLevelUp(equipment);
            if (enoughGold && enoughResources) return true;
            return false;
        }
        public int GetLevelCap(Rarity rarity)
        {
            return dictLevelCapRarityBased[rarity] - 1;
        }
        public long GetGoldRequiredForLevelUp(Equipment equipment)
        {
            return dictUpgradeResourcesPerLvl[equipment.itemTemplate.resourceToUpgrade][equipment.CurrentLevel].goldRequired;
        }
        public long GetGoldRequiredForLevelUp(Equipment equipment, int level)
        {
            return dictUpgradeResourcesPerLvl[equipment.itemTemplate.resourceToUpgrade][level].goldRequired;
        }
        public int GetNumberOfResourceTypeRequiredForLevelUp(Equipment equipment)
        {
            return dictUpgradeResourcesPerLvl[equipment.itemTemplate.resourceToUpgrade][equipment.CurrentLevel].numberOfResourcesTypeRequired;
        }
        public int GetNumberOfResourceTypeRequiredForLevelUp(Equipment equipment, int level)
        {
            return dictUpgradeResourcesPerLvl[equipment.itemTemplate.resourceToUpgrade][level].numberOfResourcesTypeRequired;
        }
        public void GetRefundGoldAndResources(Equipment equipment, out long refundGold, out int refundResource)
        {
            refundGold = 0;
            refundResource = 0;
            for (int i = equipment.CurrentLevel - 1; i >= 0; i--)
            {
                refundGold += GetGoldRequiredForLevelUp(equipment, i);
                refundResource += GetNumberOfResourceTypeRequiredForLevelUp(equipment, i);
            }

        }
        public int GetGoldAndResourcesRequiredForMaxLevel(Equipment equipment, long currentGold, int currentResourceQuantity, out long requiredGold, out int requiredResources)
        {
            long totalGold = 0;
            int counterGold = 0;
            for (int i = equipment.CurrentLevel; i < dictLevelCapRarityBased[equipment.Rarity]; i++)
            {
                totalGold += GetGoldRequiredForLevelUp(equipment, i);
                counterGold++;
                if (totalGold > currentGold)
                {
                    totalGold -= GetGoldRequiredForLevelUp(equipment, i);
                    counterGold--;
                    break;
                }
            }
            int totalResource = 0;
            int counterResource = 0;
            for (int i = equipment.CurrentLevel; i < dictLevelCapRarityBased[equipment.Rarity]; i++)
            {
                totalResource += GetNumberOfResourceTypeRequiredForLevelUp(equipment, i);
                counterResource++;
                if (totalResource > currentResourceQuantity)
                {
                    totalResource -= GetNumberOfResourceTypeRequiredForLevelUp(equipment, i);
                    counterResource--;
                    break;
                }
            }

            if (counterGold >= counterResource)
            {
                for (int i = counterGold; i > counterResource; i--)
                {
                    totalGold -= GetGoldRequiredForLevelUp(equipment, equipment.CurrentLevel + i - 1);
                }
                requiredGold = totalGold;
                requiredResources = totalResource;
                return counterResource;
            }
            else
            {
                for (int i = counterResource; i > counterGold; i--)
                {
                    totalResource -= GetNumberOfResourceTypeRequiredForLevelUp(equipment, equipment.CurrentLevel + i - 1);

                }
                requiredGold = totalGold;
                requiredResources = totalResource;
                return counterGold;
            }

        }
        [Button]
        [PropertyOrder(0)]
        public void AutoSetUpgradeResourcesPerLvl()
        {
            long baseGold = 100;
            int baseResource = 1;
            for (int i = 0; i < (int)ResourceType.Blueprint_Boot + 1; i++)
            {
                if (!dictUpgradeResourcesPerLvl.ContainsKey((ResourceType)i))
                {
                    dictUpgradeResourcesPerLvl.Add((ResourceType)i, new ResourcesUpgrade[dictLevelCapRarityBased[Rarity.LEGENDARY] - 1]);
                }
            }
            var keys = dictUpgradeResourcesPerLvl.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                int counter = 1;
                for (int j = 0; j < dictUpgradeResourcesPerLvl[keys[i]].Length; j++)
                {
                    if (counter < 5)
                    {
                        dictUpgradeResourcesPerLvl[keys[i]][j]
                            = new ResourcesUpgrade(baseGold * (counter), baseResource);
                    }
                    else if (counter >= 5 && counter < 7)
                    {
                        dictUpgradeResourcesPerLvl[keys[i]][j]
                            = new ResourcesUpgrade(2 * baseGold * (counter), baseResource * 2);
                    }
                    else if (counter >= 7 && counter < 10)
                    {
                        dictUpgradeResourcesPerLvl[keys[i]][j]
                            = new ResourcesUpgrade(3 * baseGold * (counter), baseResource * 3);
                    }
                    else if (counter >= 10 && counter < 15)
                    {
                        dictUpgradeResourcesPerLvl[keys[i]][j]
                            = new ResourcesUpgrade(5 * baseGold * (counter), baseResource * 5);
                    }
                    else if (counter >= 15 && counter < 30)
                    {
                        dictUpgradeResourcesPerLvl[keys[i]][j]
                            = new ResourcesUpgrade(6 * baseGold * (counter), baseResource * 10);
                    }
                    else if (counter >= 30 && counter < 50)
                    {
                        dictUpgradeResourcesPerLvl[keys[i]][j]
                            = new ResourcesUpgrade(7 * baseGold * (counter), baseResource * 15);
                    }
                    else if (counter >= 50 && counter < 70)
                    {
                        dictUpgradeResourcesPerLvl[keys[i]][j]
                            = new ResourcesUpgrade(8 * baseGold * (counter), baseResource * 20);
                    }
                    else if (counter >= 70)
                    {
                        dictUpgradeResourcesPerLvl[keys[i]][j]
                            = new ResourcesUpgrade(10 * baseGold * (counter), baseResource * 25);
                    }
                    counter++;
                }
            }
        }
    }
    [Serializable]

    public class BaseStat
    {
        public int[] bonusAmountPerLevel;
        public int[] baseStat;
    }

    [Serializable]
    public class ResourcesUpgrade
    {
        public long goldRequired;
        public int numberOfResourcesTypeRequired;

        public ResourcesUpgrade()
        {
        }

        public ResourcesUpgrade(long goldRequired, int numberOfResourcesTypeRequired)
        {
            this.goldRequired = goldRequired;
            this.numberOfResourcesTypeRequired = numberOfResourcesTypeRequired;
        }
    }
}
