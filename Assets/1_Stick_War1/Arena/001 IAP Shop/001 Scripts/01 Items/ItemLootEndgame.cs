using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Snowyy;
using Snowyy.EquipmentSystem;
using TMPro;
using Unicorn;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class ItemLootEndgame : MonoBehaviour
    {
        [Title("_TEXT")]
        [SerializeField] private TextMeshProUGUI txtQuantity;

        [Title("_IMAGES")]
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgBorder;
        [Title("_DATA")]
        [SerializeField] private DataResources data;

        [SerializeField] private Sprite iconGold;
        [SerializeField] private ResourceType[] arrResource;

        public void Init(LootType lootType, int quantity)
        {
            arrResource.Shuffle();
            txtQuantity.text = $"X{quantity}";
            
            switch (lootType)
            {
                case LootType.GOLD:
                    GameManager.Instance.Profile.AddGold(quantity, "add_gold_loot_endgame");
                    imgBorder.gameObject.SetActive(false);
                    imgIcon.sprite = iconGold;
                    break;
                case LootType.DESIGN:
                    var currentResource = EquipmentDataManager.Instance.GetResource(arrResource[0]);
                    EquipmentDataManager.Instance.SetResource(arrResource[0],
                        new Resource(arrResource[0], currentResource.quantity + quantity));
                    imgIcon.sprite = data.dictResourceInfos[arrResource[0]].iconResource;
                    imgBorder.gameObject.SetActive(false);
                    break;
            }
        }
    }

    public enum LootType
    {
        GOLD,
        DESIGN,
    }
}