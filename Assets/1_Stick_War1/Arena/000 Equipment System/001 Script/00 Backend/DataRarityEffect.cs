using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowyy.EquipmentSystem
{
    [CreateAssetMenu(fileName = "Rarity Effects Data", menuName = "Equipment System Data/Rarity Effects Data", order = 1)]
    public class DataRarityEffect : SerializedScriptableObject
    {
        [SerializeField] private string richTxtColorString;

        [Title("RARITY BASED")]
        [PropertyOrder(1)]
        [InfoBox("This dictionary stores rarity based images", InfoMessageType.Info)]
        public Dictionary<Rarity, RarityBasedSprite> dictRarityBasedSprites;
        [PropertyOrder(0)]
        [InfoBox("This dictionary stores rarity based effect amount, int array length must equal the number of Rarity", InfoMessageType.Info)]
        [SerializeField] private Dictionary<EffectType, int[]> dictRarityBasedBuffAmount;

        public string GetEffectDes(EffectType effectType, Rarity rarity)
        {
            return effectType switch
            {
                EffectType.ATK => $"ATK +<color={richTxtColorString}>{dictRarityBasedBuffAmount[effectType][(int)rarity]}%</color>",
                EffectType.HP => $"HP +<color={richTxtColorString}>{dictRarityBasedBuffAmount[effectType][(int)rarity]}%</color>",
                EffectType.DEF => $"DEF +<color={richTxtColorString}>{dictRarityBasedBuffAmount[effectType][(int)rarity]}%</color>",
                EffectType.SKILL => "not implemented",
                _ => "not implemented",
            };
        }

        public string GetRarityDisplay(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.COMMON => "Common",
                Rarity.RARE => "Rare",
                Rarity.EPIC => "Epic",
                Rarity.MYTHICAL => "Mythical",
                Rarity.LEGENDARY => "Legendary",
                _ => "",
            };
        }
    }
}
