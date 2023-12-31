using Snowyy;
using Snowyy.EquipmentSystem;
using Snowyy.Ultilities;
using System;
using TMPro;
using Unicorn.UI;
using UnityEngine;

namespace Arena
{
    public class UiDailyShop : UICanvas
    {
        [SerializeField] private D_SurvivorShop dataSurvivorShop;
        [SerializeField] private TextMeshProUGUI txtRefreshDisplay;
        [SerializeField] private ItemDailyShop[] itemDailyShops;

        private void Update()
        {
            UpdateRefreshTime();
        }

        private void UpdateRefreshTime()
        {
            DateTime timeNow = DateTime.Now;
            DateTime finalTime = new DateTime(timeNow.Year, timeNow.Month, timeNow.AddDays(1).Day, 0, 0, 0);
            long tickFinalTime = finalTime.Ticks;
            long tickTimeNow = timeNow.Ticks;

            long elapsedTicks = tickFinalTime - tickTimeNow;

            txtRefreshDisplay.text =
                $"Refresh Timer: {TimeSpan.FromTicks(elapsedTicks).Hours}h {TimeSpan.FromTicks(elapsedTicks).Minutes}m {TimeSpan.FromTicks(elapsedTicks).Seconds}s";
            if (SnowyyExtensions.CheckConditionDay(SurvivorShopDataManager.Instance.GetTimeDailyShopRefresh(), 1))
            {
                Init();
            }
        }

        public void Init()
        {
            InitNewDay();
            InitGemSlot();
            for (int i = 1; i < itemDailyShops.Length; i++)
            {
                var itemToInit = SurvivorShopDataManager.Instance.GetItemDailyShop(i);
                itemDailyShops[i].InitItem(itemToInit, i);
            }

        }

        public void InitGemSlot()
        {
            var dailyGemCount = SurvivorShopDataManager.Instance.GetGemEachDayCount();
            var currencyType = dailyGemCount > 0 ? CurrencyType.VIDEO : CurrencyType.FREE;
            bool isLimit = dailyGemCount >= 3;

            if (isLimit)
            {
                SurvivorShopDataManager.Instance.SetItemDailyPurchased(0, true);
            }

            var itemToInit = new ItemToPurchase(isLimit ? true : false,
                ShopItemType.GEM,
                dataSurvivorShop.freeGemEachDay[
                    dailyGemCount >= dataSurvivorShop.freeGemEachDay.Length
                        ? dataSurvivorShop.freeGemEachDay.Length - 1
                        : dailyGemCount],
                currencyType, 0);

            itemDailyShops[0].InitItem(itemToInit, 0);
        }

        public void InitNewDay()
        {
            if (!SnowyyExtensions.CheckConditionDay(SurvivorShopDataManager.Instance.GetTimeDailyShopRefresh(), 1))
                return;
            SurvivorShopDataManager.Instance.SetGemEachDayCount(0);
            SurvivorShopDataManager.Instance.SetItemDailyPurchased(0, false);
            SurvivorShopDataManager.Instance.SetTimeDailyShopRefresh(DateTime.Now.ToString());
            for (int i = 1; i < itemDailyShops.Length; i++)
            {
                SurvivorShopDataManager.Instance.SetItemDailyPurchased(i, false);
                var itemToPurchase =
                    dataSurvivorShop.GetRandomItemShop(dataSurvivorShop.dailyShopSlotType[i]);
                SurvivorShopDataManager.Instance.SetItemDailyShop(i, itemToPurchase);
            }
        }
    }

    public class ItemToPurchase
    {
        public bool isPurchased = false;
        public ShopItemType shopItemType;
        public ResourceType resourceType;
        public Equipment equipment;
        public int quantity;
        public CurrencyType currencyType;
        public int pricePerItem;

        public ItemToPurchase()
        {
        }

        public ItemToPurchase(bool isPurchased, ShopItemType shopItemType, ResourceType resourceType,
            Equipment equipment, int quantity, CurrencyType currencyType, int pricePerItem)
        {
            this.isPurchased = isPurchased;
            this.shopItemType = shopItemType;
            this.resourceType = resourceType;
            this.equipment = equipment;
            this.quantity = quantity;
            this.currencyType = currencyType;
            this.pricePerItem = pricePerItem;
        }

        public ItemToPurchase(bool isPurchased, ShopItemType shopItemType, int quantity, CurrencyType currencyType,
            int pricePerItem)
        {
            this.isPurchased = isPurchased;
            this.shopItemType = shopItemType;
            this.quantity = quantity;
            this.currencyType = currencyType;
            this.pricePerItem = pricePerItem;
        }

        public ItemToPurchase(bool isPurchased, DataItemDailyShop dataItemDailyShop, Equipment equipment = null)
        {
            this.isPurchased = isPurchased;
            shopItemType = dataItemDailyShop.shopItemType;
            quantity = dataItemDailyShop.quantity;
            currencyType = dataItemDailyShop.currencyType;
            pricePerItem = dataItemDailyShop.pricePerItem;
            this.resourceType = dataItemDailyShop.resourceType;
            this.equipment = equipment;
        }
    }
}