using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spicyy.System;
using Unicorn.Utilities;

[CreateAssetMenu(fileName = "Data Player Stat", menuName = "ScriptableObjects/Data Player Stat")]

public class PlayerStatData : SerializedScriptableObject
{
    [Title("DATA", titleAlignment: TitleAlignments.Centered, bold: true)]
    [PropertyOrder(1)]
    [InfoBox("This dictionary stores list of player stat ", InfoMessageType.Info)]
    public Dictionary<PlayerUpgradeStatEvent.UpgradeStatType, List<PlayerStatBonus>> dictStatTemplates;

    [PropertyOrder(2)]
    [InfoBox("This dictionary stores list of player default stat ", InfoMessageType.Info)]
    public List<DefaultStatSettings> defaultStatSettings = new List<DefaultStatSettings>();

    [Title("EDITOR BUTTONS", titleAlignment: TitleAlignments.Centered, bold: true)]

    [PropertyOrder(0)]
    [Button]
    [PropertyTooltip("Clear all stats for each stat type")]
    public void AutoClearAllStatsForKey()
    {
        foreach (var key in dictStatTemplates.Keys)
        {
            if (dictStatTemplates.TryGetValue(key, out var playerStatBonusList))
            {
                playerStatBonusList.Clear();
            }
        }
    }

    [PropertyOrder(0)]
    [Button]
    [PropertyTooltip("Set level of PlayerStat")]
    public void AutoSetLevelOfStat()
    {
        foreach (var key in dictStatTemplates.Keys)
        {
            List<PlayerStatBonus> playerStatBonus = dictStatTemplates[key];
            for (int index = 0; index < playerStatBonus.Count; index++)
            {
                playerStatBonus[index].level = index + 1;
            }
        }
    }

    [PropertyOrder(0)]
    [Button]
    [PropertyTooltip("Set type of PlayerStat")]
    public void AutoSetCostOfStat()
    {
        foreach (var key in dictStatTemplates.Keys)
        {
            List<PlayerStatBonus> playerStatBonus = dictStatTemplates[key];
            for (int index = 0; index < playerStatBonus.Count; index++)
            {
                playerStatBonus[index].cost = 50 + 50 * index;
            }
        }
    }

    [PropertyOrder(0)]
    [Button]
    [PropertyTooltip("Set type of PlayerStat")]
    public void AutoAddMorePlayerStat()
    {
        foreach (var key in dictStatTemplates.Keys)
        {
            // Kiểm tra xem đã có danh sách PlayerStatBonus cho loại PlayerUpgradeStatEvent.UpgradeStatType này chưa
            if (!dictStatTemplates.TryGetValue(key, out var playerStatBonusList))
            {
                // Nếu chưa có, tạo mới một danh sách rỗng cho loại này
                playerStatBonusList = new List<PlayerStatBonus>();
                dictStatTemplates[key] = playerStatBonusList;
            }

            // Thêm 5 PlayerStatBonus mới với level tăng dần vào danh sách
            for (int i = 0; i < 5; i++)
            {
                PlayerStatBonus newStatBonus = new PlayerStatBonus();
                newStatBonus.level = playerStatBonusList.Count + 1; // Gán level mới
                playerStatBonusList.Add(newStatBonus);
            }
        }
    }

    [PropertyOrder(0)]
    [Button]
    [PropertyTooltip("Set Stat of PlayerStat")]
    public void AutoSetIndexOfStat()
    {
        foreach (var key in dictStatTemplates.Keys)
        {
            switch (key)
            {
                case PlayerUpgradeStatEvent.UpgradeStatType.Attack:
                    SetStatForType(key, GetStatDefault(PlayerUpgradeStatEvent.UpgradeStatType.Attack), 1);
                    break;
                case PlayerUpgradeStatEvent.UpgradeStatType.Health:
                    SetStatForType(key, GetStatDefault(PlayerUpgradeStatEvent.UpgradeStatType.Health), 20);
                    break;
                case PlayerUpgradeStatEvent.UpgradeStatType.Defense:
                    SetStatForType(key, GetStatDefault(PlayerUpgradeStatEvent.UpgradeStatType.Defense), 1);
                    break;
            }
        }
    }

    [PropertyOrder(0)]
    [Button]
    [PropertyTooltip("Set default values for each stat type")]
    public void SetDefaultValuesForStatTypes()
    {
        foreach (var key in dictStatTemplates.Keys)
        {
            List<PlayerStatBonus> m_Stats = dictStatTemplates[key];
            PlayerUpgradeStatEvent.UpgradeStatType statType = key;

            // Tìm giá trị mặc định cho statType trong danh sách settings
            DefaultStatSettings defaultSettings = defaultStatSettings.Find(setting => setting.statType == statType);

            if (defaultSettings != null)
            {
                m_Stats[0].stat = defaultSettings.defaultStatValue;
                m_Stats[0].cost = defaultSettings.defaultCostValue;
            }
            else
            {
                Debug.LogWarning($"No default value found for {statType}. Set default value manually.");
            }
        }
    }

    private void SetStatForType(PlayerUpgradeStatEvent.UpgradeStatType type, int startingValue, int increasePerLevel)
    {
        if (dictStatTemplates.TryGetValue(type, out var playerStatBonusList))
        {
            int currentStatValue = startingValue;
            foreach (var statBonus in playerStatBonusList)
            {
                statBonus.stat = currentStatValue;
                currentStatValue += increasePerLevel;
            }
        }
    }

    public float GetPlayerStatAtLevel(PlayerUpgradeStatEvent.UpgradeStatType upgradeType, int level)
    {
        List<PlayerStatBonus> m_Stats = dictStatTemplates[upgradeType];
        return m_Stats[level].stat;
    }

    private int GetStatDefault(PlayerUpgradeStatEvent.UpgradeStatType statType)
    {
        DefaultStatSettings defaultSetting = defaultStatSettings.Find(setting => setting.statType == statType);

        if (defaultSetting != null)
        {
            return defaultSetting.defaultStatValue;
        }
        else
        {
            Debug.LogWarning($"No default value found for {statType}. Set default value manually.");
            return 0; // Giá trị mặc định nếu không tìm thấy thiết lập
        }
    }

    public int GetPlayerStatLevel(PlayerUpgradeStatEvent.UpgradeStatType upgradeStatType)
    {
        switch (upgradeStatType)
        {
            case PlayerUpgradeStatEvent.UpgradeStatType.Attack:
                return PlayerPrefs.GetInt(Helper.PlayerAttackLevel, 0);
            case PlayerUpgradeStatEvent.UpgradeStatType.Defense:
                return PlayerPrefs.GetInt(Helper.PlayerDefenseLevel, 0);
            case PlayerUpgradeStatEvent.UpgradeStatType.Health:
                return PlayerPrefs.GetInt(Helper.PlayerHealthLevel, 0);
            default:
                break;
        }
        return 0;
    }

    public void SetPlayerStatLevel(PlayerUpgradeStatEvent.UpgradeStatType upgradeStatType, int level)
    {
        switch (upgradeStatType)
        {
            case PlayerUpgradeStatEvent.UpgradeStatType.Attack:
                PlayerPrefs.SetInt(Helper.PlayerAttackLevel, level);
                break;
            case PlayerUpgradeStatEvent.UpgradeStatType.Defense:
                PlayerPrefs.SetInt(Helper.PlayerDefenseLevel, level);
                break;
            case PlayerUpgradeStatEvent.UpgradeStatType.Health:
                PlayerPrefs.SetInt(Helper.PlayerHealthLevel, level);
                break;
        }
    }

    public float GetTotalATK()
    {
        return PlayerPrefs.GetFloat(Helper.PlayerTotalATK, GetStatDefault(PlayerUpgradeStatEvent.UpgradeStatType.Attack));
    }

    public void SetTotalATK(float value)
    {
        PlayerPrefs.SetFloat(Helper.PlayerTotalATK, value);
    }

    public float GetTotalHealth()
    {
        return PlayerPrefs.GetFloat(Helper.PlayerTotalHealth, GetStatDefault(PlayerUpgradeStatEvent.UpgradeStatType.Health));

    }

    public void SetTotalHealth(float value)
    {
        PlayerPrefs.SetFloat(Helper.PlayerTotalHealth, value);
    }

    public float GetTotalDefense()
    {
        return PlayerPrefs.GetFloat(Helper.PlayerTotalDefense, GetStatDefault(PlayerUpgradeStatEvent.UpgradeStatType.Defense));

    }

    public void SetTotalDefense(float value)
    {
        PlayerPrefs.SetFloat(Helper.PlayerTotalDefense, value);
    }

}

public class PlayerStatBonus
{
    public int level;
    public int cost;
    public int stat;
}

[System.Serializable]
public class DefaultStatSettings
{
    public PlayerUpgradeStatEvent.UpgradeStatType statType;
    public int defaultCostValue;
    public int defaultStatValue;
}

