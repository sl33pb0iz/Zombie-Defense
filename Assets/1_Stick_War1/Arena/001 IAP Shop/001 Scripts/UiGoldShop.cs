using RocketTeam.Sdk.Services.Ads;
using Snowyy.Ultilities;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using Unicorn.UI;
using UnityEngine;

namespace Arena
{
    public class UiGoldShop : UICanvas
    {
        [SerializeField] private ItemGoldShop[] itemGoldShop;
        [SerializeField] private Sprite[] itemIcon;
        [SerializeField] private D_SurvivorShop data;

        private bool isInit = false;
        private bool isLoaded = false;

        public void Init()
        {
            isInit = false;
            InitFreeGold();
            for (int i = 1; i < itemGoldShop.Length; i++)
            {
                itemGoldShop[i].Init(data.goldShopItems[i].goldAmount, data.goldShopItems[i].priceGem, itemIcon[i]);
            }
        }

//         private void Update()
//         {
//             if (!isInit && SurvivorShopDataManager.Instance.GetReceivedFreeGoldDaily() == false)
//             {
//                 isInit = true;
//                 itemGoldShop[0].Init(data.goldShopItems[0].goldAmount, data.goldShopItems[0].priceGem, true);
//                 return;
//             }
//             if (SurvivorShopDataManager.Instance.GetReceivedFreeGoldDaily() == false)
//                 return;
// #if !UNITY_EDITOR
//             if (isLoaded == false && AdManager.Instance.IsInterstitialLoaded((int)AdEnums.ShowType.VIDEO_REWARD))
//             {
//                 isLoaded = !isLoaded;
//                 itemGoldShop[0].Init(data.goldShopItems[0].goldAmount, data.goldShopItems[0].priceGem, false, true);
//             }
//             else if (isLoaded == true && !AdManager.Instance.IsInterstitialLoaded((int)AdEnums.ShowType.VIDEO_REWARD))
//             {
//                 isLoaded = !isLoaded;
//                 itemGoldShop[0].Init(data.goldShopItems[0].goldAmount, data.goldShopItems[0].priceGem);
//             }
// #endif
//         }
        private void InitFreeGold()
        {
            if (SnowyyExtensions.CheckConditionDay(SurvivorShopDataManager.Instance.GetTimeDailyShopRefresh(), 1))
            {
                SurvivorShopDataManager.Instance.SetReceivedFreeGoldDaily(false);
                return;
            }

            if (SurvivorShopDataManager.Instance.GetReceivedFreeGoldDaily() == false)
            {
                itemGoldShop[0].Init(data.goldShopItems[0].goldAmount, data.goldShopItems[0].priceGem, itemIcon[0],true);
                return;
            }

            //if (SurvivorShopDataManager.Instance.GetReceivedFreeGoldDaily() == false)
            //{
            //    itemGoldShop[0].Init(data.goldShopItems[0].goldAmount, data.goldShopItems[0].priceGem, true);
            //    return;
            //}

            //if (AdManager.Instance.IsInterstitialLoaded((int)AdEnums.ShowType.VIDEO_REWARD))
            //{
            //    itemGoldShop[0].Init(data.goldShopItems[0].goldAmount, data.goldShopItems[0].priceGem, false, true);
            //    return;
            //}
            itemGoldShop[0].Init(data.goldShopItems[0].goldAmount, data.goldShopItems[0].priceGem, itemIcon[0]);
        }
    }
}