using Sirenix.OdinInspector;
using Snowyy.EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Snowyy.MergeSystem
{
    public class UiMergeBottomBar : UICanvas
    {
        [Title("Buttons")]
        [SerializeField] private Button btnMerge;
        [Title("Layout")]
        [SerializeField] private RectTransform bottomBarRectTransform;
        [SerializeField] private Canvas parentCanvas;

        private void Start()
        {
            btnMerge.onClick.AddListener(OnClickBtnMerge);
        }
        private void LateUpdate()
        {
            UpdatePosition();
        }
        private void UpdatePosition()
        {
            if (!BlackBorderBanner.Instance) return;

            bottomBarRectTransform.anchoredPosition = new Vector2(bottomBarRectTransform.anchoredPosition.x,
                BlackBorderBanner.Instance.GetBorderBannerHeight() / parentCanvas.transform.localScale.y);
        }
       
        private void OnClickBtnMerge()
        {
            SoundManager.Instance.PlaySoundButton();
            var topManager = UiEquipmentSystemBrain.Instance.UiMergeEquipment.TopManager;
            if (!topManager.CheckCanMerge())
            {
                Debug.LogError("KO MERGE DUOC BAN GI OIIII!! IMPLEMENT POP UP DEEE");
                return;
            }

            var botManager = UiEquipmentSystemBrain.Instance.UiMergeEquipment.BotManager;

            var newEquipment = topManager.ItemCurrentEquipment.BindedEquipment;
            newEquipment.Rarity = (Rarity)((int)newEquipment.Rarity + 1);

            if (EquipmentDataManager.Instance.IsInventory(topManager.ItemCurrentEquipment.BindedEquipment))
            {
                var index = EquipmentDataManager.Instance.FindEquipmentIndex(topManager.ItemCurrentEquipment.BindedEquipment);
                EquipmentDataManager.Instance.SetEquipmentByIndex(index, newEquipment);
            }
            else
            {
                EquipmentDataManager.Instance.SetCurrentEquippedEquipment(newEquipment.EquipmentType, newEquipment);
            }

            for (int i = 0; i < topManager.ItemMergedMaterials.Length; i++)
            {
                if (EquipmentDataManager.Instance.IsInventory(topManager.ItemMergedMaterials[i].BindedEquipment))
                {
                    EquipmentDataManager.Instance.RemoveEquipment(topManager.ItemMergedMaterials[i].BindedEquipment);
                }
                else
                {
                    EquipmentDataManager.Instance.SetCurrentEquippedEquipment(topManager.ItemMergedMaterials[i].BindedEquipment.EquipmentType, null);
                }
            }

            EquipmentDataManager.Instance.SaveAllEquipments();
            botManager.Init();
            topManager.RemoveMergedEquipment();
        }
    }
}
