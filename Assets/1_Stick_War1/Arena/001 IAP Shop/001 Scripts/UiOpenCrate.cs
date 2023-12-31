using Snowyy.EquipmentSystem;
using Snowyy;
using System.Collections;
using System.Collections.Generic;
using Unicorn.UI;
using UnityEngine;

namespace Arena
{
    public class UiOpenCrate : UICanvas
    {
        public OpenOneCrate OpenOneCrate;
        public OpenMultiCrates OpenMultiCrates;

        public List<Equipment> ListEquipmentMultiOpen { get; private set; }
        private int multiCounter = 0;

        public void InitSingle(CrateType crateType, bool isSuperNormal = false)
        {
            var equipmentNew = EquipmentGenerator.Instance.GenerateEquipment(crateType);

            Show(true);
            OpenOneCrate.Init(equipmentNew, true, crateType == CrateType.NORMAL);

        }

        public void InitMulti(CrateType crateType)
        {
            ListEquipmentMultiOpen = new List<Equipment>();
            for (int i = 0; i < 10; i++)
            {
                var equipmentNew = EquipmentGenerator.Instance.GenerateEquipment(crateType);
                ListEquipmentMultiOpen.Add(equipmentNew);
            }

            Show(true);
            ShowEachOne();
        }

        public void ShowEachOne(bool isWhiteChest = false)
        {
            if (multiCounter >= 10)
            {
                //TODO: POP UP FULL
                OpenMultiCrates.Init(ListEquipmentMultiOpen);
                return;
            }

            OpenOneCrate.Init(ListEquipmentMultiOpen[multiCounter], false);
            multiCounter++;
        }

        public void SetMultiCounter(int counter)
        {
            multiCounter = counter;
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (IsShow)
            {
                OpenOneCrate.Show(false);
            }
        }
    }
}