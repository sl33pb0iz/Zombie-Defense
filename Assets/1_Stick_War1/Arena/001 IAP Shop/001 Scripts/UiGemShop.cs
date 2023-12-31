using System.Collections;
using System.Collections.Generic;
using Unicorn;
using Unicorn.UI;
using UnityEngine;

namespace Arena
{
    public class UiGemShop : UICanvas
    {
        [SerializeField] private DataShopIap dataShopIap;

        [SerializeField] private ItemGemShop[] arrItemGem;
        [SerializeField] private IdPack[] iapIdPack;
        [SerializeField] private Sprite[] arrIcons;
        [SerializeField] private string[] arrDescriptions;

        public void Init()
        {
            for (int i = 0; i < iapIdPack.Length; i++)
            {
                arrItemGem[i].Init(iapIdPack[i], dataShopIap.dictInfoPackage[iapIdPack[i]].listRewardPack[0].amount,
                    GameManager.Instance.IapController.DictInfoPricePack[iapIdPack[i]].localizedPrice,
                    arrIcons[i],
                    arrDescriptions[i]);
            }
        }
    }
}
