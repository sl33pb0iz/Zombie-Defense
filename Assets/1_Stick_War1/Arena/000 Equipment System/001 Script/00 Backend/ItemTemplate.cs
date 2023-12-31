using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Snowyy.EquipmentSystem
{
    [Serializable]
    public class ItemTemplate
    {
        [FoldoutGroup("Auto Bool Attributes")]
        [Tooltip("Use the Auto Set Bool Button above")]
        [Sirenix.OdinInspector.ReadOnly]
        public bool isWeapon;
        [FoldoutGroup("Auto Bool Attributes")]
        [Tooltip("Use 2 Toggle Item Set Effect Buttons above")]
        [Sirenix.OdinInspector.ReadOnly]
        public bool isItemSetEffect;

        [Tooltip("This id is used for saved equipment to find its item template")]
        public string Id;
        public int ordinalNumber;
        public string name;
        public string description; 
        public ResourceType resourceToUpgrade;

        //*** this array length must equal to the number of Rarity
        [Tooltip("This array length must equal to the number of Rarity")]
        public EffectType[] arrEffects;

        //*** this array length must equal to the number of Rarity
        //[PreviewField(120)]
        [Tooltip("This array length must equal to the number of Rarity")]
        public Sprite[] arrUpgradeIcons;

        [Tooltip("This attribute is to distinguish weapon type.\nIf this item isn't weapon, ignore this shit")]
        [ShowIf("isWeapon")]
        public WeaponType weaponType;

        [Tooltip("(OPTIONNAL) This attribute is to detect item set effect." +
            "\nIf you aren't implement item set features, ignore this shit" +
            "\nIn this template, the creator dont apply item set effect for weapon")]
        [ShowIf("isItemSetEffect")]
        public ItemSet itemSet;
        public ItemTemplate()
        {

        }
    }
}
