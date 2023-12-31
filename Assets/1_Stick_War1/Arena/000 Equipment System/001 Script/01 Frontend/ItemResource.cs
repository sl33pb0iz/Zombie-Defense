using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Snowyy.EquipmentSystem
{
    public class ItemResource : MonoBehaviour
    {
        [Title("Image")]
        [SerializeField] private Image imgIcon;
        [Title("Text")]
        [SerializeField] private TextMeshProUGUI txtQuantity;
        [Title("Others")]
        [SerializeField] private DataResources dataResources;

        public void Init(Resource resource)
        {
            imgIcon.sprite = dataResources.dictResourceInfos[resource.resourceType].iconResource;
            txtQuantity.text = $"x{resource.quantity}";
        }
    }
}
