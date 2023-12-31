using System.Collections;
using System.Collections.Generic;
using Unicorn;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class UiSurvivorShop : UICanvas
    {
        
        public UiDailyShop UiDailyShop;
        public UiGemShop UiGemShop;
        public UiGoldShop UiGoldShop;
        public UiNormalSupplies UiNormalSupplies;
        public UiSuperSupplies UiSuperSupplies;
        public UiOpenCrate UiOpenCrate;
        public UiTurretPack UiTurretPack;

        [SerializeField] private Scrollbar scrollBar;
        public bool IsGoToGem { get; set; }
        public bool IsGoToGold { get; set; }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (IsShow)
            {
                Init();
              
                if (IsGoToGem)
                {
                    IsGoToGem = false;
                    scrollBar.value = 0.2f;
                }

                else if (IsGoToGold)
                {
                    IsGoToGold = false;
                    scrollBar.value = 0f;
                }

                else
                {
                    scrollBar.value = 1f;
                }

            }

        }

        public void Init()
        {
            UiGemShop.Init();
            UiTurretPack.Init();
            UiGoldShop.Init();
            UiDailyShop.Init();
            UiNormalSupplies.Init();
            UiSuperSupplies.Init();
        }

    }
}