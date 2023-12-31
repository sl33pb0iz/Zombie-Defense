using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowyy.EquipmentSystem
{
    [Serializable]
    public class RarityBasedSprite
    {
        [PreviewField]
        public Sprite borderSprite;
        [PreviewField]
        public Sprite titleBarSprite;
        public Color rarityColor;
    }
}
