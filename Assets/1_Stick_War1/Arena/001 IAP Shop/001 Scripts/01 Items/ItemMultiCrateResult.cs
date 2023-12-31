using Snowyy.EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class ItemMultiCrateResult : MonoBehaviour
    {
        [SerializeField] private Image imgBorder;
        [SerializeField] private Image imgIcon;

        [SerializeField] private DataRarityEffect dataRarityEffect;
        public void Init(Equipment equipment)
        {
            imgBorder.sprite = dataRarityEffect.dictRarityBasedSprites[equipment.Rarity].borderSprite;
            imgIcon.sprite = equipment.itemTemplate.arrUpgradeIcons[(int)equipment.Rarity];
        }
    }
}
