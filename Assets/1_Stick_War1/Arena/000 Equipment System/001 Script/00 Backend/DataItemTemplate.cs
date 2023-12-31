using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Snowyy.EquipmentSystem
{
    [CreateAssetMenu(fileName = "Item Template Data", menuName = "Equipment System Data/Item Template Data", order = 0)]
    public class DataItemTemplate : SerializedScriptableObject
    {

        [Title("ITEM TEMPLATES", titleAlignment: TitleAlignments.Centered)]
        [PropertyOrder(1)]
        [InfoBox("This dictionary stores list of icons distinguished by Equipment Type", InfoMessageType.Info)]
        public Dictionary<EquipmentType, Sprite> dictEquipmentTypeIcons;

        [PropertyOrder(1)]
        [InfoBox("This dictionary stores list of item templates distinguished by Equipment Type", InfoMessageType.Info)]
        public Dictionary<EquipmentType, ItemTemplate[]> dictItemTemplates;

        public ItemTemplate GetRandomItemTemplate(EquipmentType type)
        {
            var randomFactor = Random.Range(0, dictItemTemplates[type].Length);
            return dictItemTemplates[type][randomFactor];
        }

        [Title("EDITOR BUTTONS", titleAlignment: TitleAlignments.Centered)]
        [PropertyOrder(0)]
        [Button]
        [PropertyTooltip("Set isWeapon of WEAPON true, Sets isWeapon of OTHERS false")]
        public void AutoSetBoolWeapon()
        {
            for (int i = 0; i < dictItemTemplates[EquipmentType.Weapon].Length; i++)
            {
                dictItemTemplates[EquipmentType.Weapon][i].isWeapon = true;

            }
            var keys = dictItemTemplates.Keys.ToList();
            keys.Remove(EquipmentType.Weapon);
            for (int i = 0; i < keys.Count; i++)
            {
                for (int j = 0; j < dictItemTemplates[keys[i]].Length; j++)
                {
                    dictItemTemplates[keys[i]][j].isWeapon = false;
                }
            }
        }
        [PropertyOrder(0)]
        [Button]
        [PropertyTooltip("Toggle isItemSetEffect of all types")]
        public void ToggleItemSetEffectAll()
        {
            var keys = dictItemTemplates.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                for (int j = 0; j < dictItemTemplates[keys[i]].Length; j++)
                {
                    dictItemTemplates[keys[i]][j].isItemSetEffect = !dictItemTemplates[keys[i]][j].isItemSetEffect;
                }
            }
        }
        [PropertyOrder(0)]
        [Button]
        [PropertyTooltip("Toggle isItemSetEffect of WEAPON")]
        public void ToggleItemSetEffectWeapon()
        {
            for (int i = 0; i < dictItemTemplates[EquipmentType.Weapon].Length; i++)
            {
                dictItemTemplates[EquipmentType.Weapon][i].isItemSetEffect = !dictItemTemplates[EquipmentType.Weapon][i].isItemSetEffect;
            }
        }
        [PropertyOrder(0)]
        [Button]
        public void AutoSetId()
        {
            var keys = dictItemTemplates.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                for (int j = 0; j < dictItemTemplates[keys[i]].Length; j++)
                {
                    dictItemTemplates[keys[i]][j].Id = dictItemTemplates[keys[i]][j].name;
                }
            }
        }
        [PropertyOrder(0)]
        [Button]
        public void AutoSetResourceToUpgrade()
        {
            var keys = dictItemTemplates.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                for (int j = 0; j < dictItemTemplates[keys[i]].Length; j++)
                {
                    dictItemTemplates[keys[i]][j].resourceToUpgrade = (ResourceType)keys[i];
                }
            }
        }
        [PropertyOrder(0)]
        [Button]
        public void AutoSetOrdinalNumber()
        {
            var keys = dictItemTemplates.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                if(keys[i] != EquipmentType.Weapon)
                {
                    var itemTemplates = dictItemTemplates[keys[i]];
                    for (int j = 0; j < itemTemplates.Length; j++)
                    {
                        itemTemplates[j].ordinalNumber = j + 1;
                    }
                }
                else
                {
                    var itemTemplates = dictItemTemplates[keys[i]];
                    for (int j = 0; j < itemTemplates.Length; j++)
                    {
                        itemTemplates[j].ordinalNumber = j;
                    }
                }
            }
        }
    }
}
