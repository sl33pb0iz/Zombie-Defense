using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowyy
{
    public delegate void OnFirstTimeOpening();
    public delegate void OnRefreshDisplay();
    public delegate void OnClose();

    public enum EquipmentType
    {
        Weapon,
        Backpack,
        Helmet,
        Armor,
        Boot
    }
    public enum WeaponType
    {
        PISTOL,
        UZI,
        SHOTGUN,
        AK47,
        MACHINEGUN, 
        ROCKETGUN,
        FLAMEGUN,
        TESLAGUN,
        LAZERGUN,

    }
    public enum ItemSet
    {
        PRIVATE,
        CORPORAL,
        SERGEANT,
        MAJOR,
        COLONEL,
        CAPTAIN,
    }
    public enum Rarity
    {
        COMMON = 0,
        RARE = 1,
        EPIC = 2,
        MYTHICAL = 3,
        LEGENDARY = 4,
    }
    public enum EffectType
    {
        ATK,
        DEF,
        HP,
        SKILL,
    }
    public enum SortType
    {
        BYRARITY = 0,
        BYSLOT = 1,
        BYLEVEL = 2,
    }
    public enum ResourceType
    {
        Blueprint_Weapon,
        Blueprint_Backpack,
        Blueprint_Helmet,
        Blueprint_Armor,
        Blueprint_Boot,
    }

    public enum CrateType
    {
        NORMAL = 0,
        ADVANCE = 1,
        SUPER = 2
    }

    public enum ShopItemType
    {
        GEM,
        GOLD,
        DESIGN,
        EQUIPMENT,
    }

    public enum CurrencyType
    {
        VIDEO,
        FREE,
        GOLD,
        GEM,
    }

    public enum TowerTypeShop
    { 
        Machine = 0,
        TeslaCoil = 1,
        Rocket = 2, 
        Flamethrower = 3,
        Lazer = 4,
    }
}
