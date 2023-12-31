using Castle.Core.Internal;
using Snowyy;
using Snowyy.EquipmentSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Arena
{
    public class SurvivorShopDataManager : MonoBehaviour
    {
        public static SurvivorShopDataManager Instance;

        public DataItemTemplate dataItemTemplate;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (GetTimeDailyShopRefresh().IsNullOrEmpty())
            {
                SetTimeDailyShopRefresh(DateTime.MinValue.ToString());
            }

            if (GetTimeRefreshFreeNormalCrate().IsNullOrEmpty())
            {
                SetTimeRefreshFreeNormalCrate(DateTime.MinValue.ToString());
            }

            if (GetTimeRefreshFreeAdvanceCrate().IsNullOrEmpty())
            {
                SetTimeRefreshFreeAdvanceCrate(DateTime.MinValue.ToString());
            }

            DontDestroyOnLoad(this.gameObject);
        }

        public void SetSuperCrateMythicalCounter(int counter)
        {
            PlayerPrefs.SetInt(StringHelper.SUPER_CRATE_MYTHICAL_COUNTER, counter);
        }

        public int GetSuperCrateMythicalCounter()
        {
            return PlayerPrefs.GetInt(StringHelper.SUPER_CRATE_MYTHICAL_COUNTER, 0);
        }

        public void SetSuperCrateEpicCounter(int counter)
        {
            PlayerPrefs.SetInt(StringHelper.SUPER_CRATE_EPIC_COUNTER, counter);
        }

        public int GetSuperCrateEpicCounter()
        {
            return PlayerPrefs.GetInt(StringHelper.SUPER_CRATE_EPIC_COUNTER, 0);
        }

        public void SetAdvanceCrateEpicCounter(int counter)
        {
            PlayerPrefs.SetInt(StringHelper.ADVANCE_CRATE_EPIC_COUNTER, counter);
        }

        public int GetAdvanceCrateEpicCounter()
        {
            return PlayerPrefs.GetInt(StringHelper.ADVANCE_CRATE_EPIC_COUNTER, 0);
        }

        public void SetItemDailyPurchased(int itemSlot, bool isPurchased)
        {
            PlayerPrefs.SetInt($"{StringHelper.DAILY_ITEM_PURCHASED} {itemSlot}", isPurchased ? 0 : 1);
        }

        public bool GetItemDailyPurchased(int itemSlot)
        {
            return PlayerPrefs.GetInt($"{StringHelper.DAILY_ITEM_PURCHASED} {itemSlot}", 0) == 0 ? true : false;
        }

        public void SetItemTowerUnlock(TowerTypeShop towerType, bool isUnlocked)
        {
            PlayerPrefs.SetInt($"{towerType}", isUnlocked ? 1 : 0) ;
        }

        public bool GetItemTowerUnlocked(TowerTypeShop towerType)
        {
            return PlayerPrefs.GetInt($"{towerType}", 0) == 0 ? false : true;
        }

        public void SetReceivedFreeGoldDaily(bool isReceived)
        {
            PlayerPrefs.SetInt(StringHelper.GOLD_FREE_DAILY, isReceived ? 0 : 1);
        }

        public bool GetReceivedFreeGoldDaily()
        {
            return PlayerPrefs.GetInt(StringHelper.GOLD_FREE_DAILY, 1) == 0 ? true : false;
        }

        public void SetTimeDailyShopRefresh(string time)
        {
            PlayerPrefs.SetString(StringHelper.TIME_REFRESH_DAILY_SHOP, time);
        }

        public string GetTimeDailyShopRefresh()
        {
            return PlayerPrefs.GetString(StringHelper.TIME_REFRESH_DAILY_SHOP, "");
        }

        public void SetTimeRefreshFreeAdvanceCrate(string time)
        {
            PlayerPrefs.SetString(StringHelper.TIME_REFRESH_ADVANCE_CRATE, time);
        }

        public string GetTimeRefreshFreeAdvanceCrate()
        {
            return PlayerPrefs.GetString(StringHelper.TIME_REFRESH_ADVANCE_CRATE, "");
        }

        public void SetTimeRefreshFreeNormalCrate(string time)
        {
            PlayerPrefs.SetString(StringHelper.TIME_REFRESH_NORMAL_CRATE, time);
        }

        public string GetTimeRefreshFreeNormalCrate()
        {
            return PlayerPrefs.GetString(StringHelper.TIME_REFRESH_NORMAL_CRATE, "");
        }

        public void SetGemEachDayCount(int count)
        {
            PlayerPrefs.SetInt(StringHelper.GEM_EACH_DAY_COUNTER, count);
        }

        public int GetGemEachDayCount()
        {
            return PlayerPrefs.GetInt(StringHelper.GEM_EACH_DAY_COUNTER, 0);
        }

        public ItemToPurchase GetItemDailyShop(int id)
        {
            try
            {
                var item = ES3.Load($"{StringHelper.TIME_REFRESH_DAILY_SHOP} {id}", (ItemToPurchase)null);
                if (item == null)
                {
                    Debug.Log("null");
                    return null;
                }
                else
                {
                    if (item.shopItemType == ShopItemType.EQUIPMENT)
                    {
                        var itemTemplate = dataItemTemplate.dictItemTemplates[item.equipment.EquipmentType]
                            .Find(i => i.Id == item.equipment.Id);
                        item.equipment.SetItemTemplate(itemTemplate);
                    }

                    return item;
                }
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("The file doesn't existed!! Return null");
                return null;
            }
        }

        public void SetItemDailyShop(int id, ItemToPurchase itemToSave)
        {
            ES3.Save($"{StringHelper.TIME_REFRESH_DAILY_SHOP} {id}", itemToSave);
        }
    }
}