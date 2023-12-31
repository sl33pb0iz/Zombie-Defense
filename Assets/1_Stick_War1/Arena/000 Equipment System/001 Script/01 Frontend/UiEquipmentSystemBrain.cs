using Sirenix.OdinInspector;
using Snowyy.MergeSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Snowyy.EquipmentSystem
{
    public class UiEquipmentSystemBrain : SerializedMonoBehaviour
    {
        public static UiEquipmentSystemBrain Instance;

        public UiEquipmentSystem UiEquipmentSystem;
        public UiPopupEquipment UiPopupEquipment;
        public UiPopupResources UiPopupResources;
        public UiPopupRefund UiPopupRefund;
        public UiMergeEquipment UiMergeEquipment;


        private void Awake()
        {
            Instance = this;
        }

        public void ShowUiEquipmentSystem(bool value)
        {
            UiEquipmentSystem.Show(value);
            if (value == false)
            {
                ShowUiMergeEquipment(value);
            }
        }

        public void ShowUiMergeEquipment(bool value)
        {
            UiMergeEquipment.Show(value);
        }

        

    }
}
