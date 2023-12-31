using Newtonsoft.Json.Schema;
using Sirenix.OdinInspector;
using Snowyy;
using Snowyy.EquipmentSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arena
{
    [CreateAssetMenu(fileName = "Survivor Shop", menuName = "Survivor Shop Data", order = 0)]
    public class D_SurvivorShop : SerializedScriptableObject
    {
        [Title("SPRITE", titleAlignment: TitleAlignments.Centered)]
        [PropertyOrder(1)]
        public Sprite goldSprite;
        [PropertyOrder(1)]
        public Sprite gemSprite;

        [Title("OTHER SCRIPTABLE", titleAlignment: TitleAlignments.Centered)]
        [PropertyOrder(2)]
        public DataItemTemplate dataItemTemplate;
        
        [Title("DATA", titleAlignment: TitleAlignments.Centered)]
        [PropertyOrder(3)]
        public GoldShopItem[] goldShopItems;
        [PropertyOrder(3)]
        public int[] freeGemEachDay;
        [PropertyOrder(3)]
        public Dictionary<int, ShopItemType> dailyShopSlotType;
        [PropertyOrder(3)]
        public Dictionary<ShopItemType, DataItemDailyShop[]> dataDailyShopItems;
        [PropertyOrder(3)]
        public List<TowerShopItem> dataTowerShopItems;

        [Title("CRATES", titleAlignment: TitleAlignments.Centered)]
        [PropertyOrder(4)]
        public int normalCratePrice = 80;
        [PropertyOrder(4)]
        public int advanceCratePrice = 160;

        public ItemToPurchase GetRandomItemShop(ShopItemType shopItemType)
        {
            var randomIndex = Random.Range(0, dataDailyShopItems[shopItemType].Length);
            bool isEquipment = shopItemType == ShopItemType.EQUIPMENT;
            if (isEquipment)
            {
                var equipmentType = dataDailyShopItems[shopItemType][randomIndex].equipmentType;
                var itemTemplate = dataItemTemplate.GetRandomItemTemplate(equipmentType);


                Equipment equipment = EquipmentDataManager.Instance.CreateNewEquipment(itemTemplate.Id, 0, equipmentType, GetEquipmentRarityDailyShop());

                return new ItemToPurchase(false, dataDailyShopItems[shopItemType][randomIndex], equipment);
            }
            return new ItemToPurchase(false, dataDailyShopItems[shopItemType][randomIndex]);
        }

        public Rarity GetEquipmentRarityDailyShop()
        {
            var randomChances = Random.Range(0, 1000);
            if (randomChances <= 600)
            {
                return Rarity.COMMON;
            }
            else if (randomChances > 600 & randomChances <= 900)
            {
                return Rarity.RARE;
            }
            else
            {
                return Rarity.EPIC;
            }
        }

        [Title("BUTTONS", titleAlignment: TitleAlignments.Centered)]
        [Button]
        [PropertyOrder(0)]
        public void AutoSetShopItemType()
        {
            var keys = dataDailyShopItems.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                for (int j = 0; j < dataDailyShopItems[keys[i]].Length; j++)
                {
                    dataDailyShopItems[keys[i]][j].shopItemType = keys[i];
                }
            }
        }
        [Button]
        [PropertyOrder(0)]
        public void AutoSetEquipmentQuantity()
        {
            for (int i = 0; i < dataDailyShopItems[ShopItemType.EQUIPMENT].Length; i++)
            {
                dataDailyShopItems[ShopItemType.EQUIPMENT][i].quantity = 1;
            }

        }
        
    }
    [Serializable]
    public class DataItemDailyShop
    {
        [ReadOnly]
        public ShopItemType shopItemType;
        [ShowIf("@this.shopItemType == ShopItemType.DESIGN")]
        public ResourceType resourceType;
        [ShowIf("@this.shopItemType == ShopItemType.EQUIPMENT")]
        public EquipmentType equipmentType;
        //[ShowIf("@this.shopItemType == ShopItemType.GEM ||this.shopItemType == ShopItemType.GOLD || this.shopItemType == ShopItemType.DESIGN")]
        public int quantity;
        public CurrencyType currencyType;
        [HideIf("@this.currencyType == CurrencyType.FREE")]
        public int pricePerItem;

    }
    [Serializable]
    public struct GoldShopItem
    {
        public int priceGem;
        public int goldAmount;
    }

    [Serializable]
    public class TowerShopItem
    {
        public TowerTypeShop towerType;
        public Sprite towerIcon;
        public int priceTower; 
    }
}
