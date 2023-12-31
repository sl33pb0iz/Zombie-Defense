using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unicorn;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Snowyy.EquipmentSystem
{
    public class UiPopupResources : UICanvas
    {
        [Title("Buttons")]
        [SerializeField] private Button btnClose;
        [SerializeField] private Button btnCloseBg;

        [Title("Others")]
        [SerializeField] private ItemResource itemResourcePrefab;
        [SerializeField] private Transform layoutTransform;
        [SerializeField] private List<ResourceType> listResourceTypes;

        private List<ItemResource> listItemResources = new List<ItemResource>();

        private void Start()
        {
            btnClose.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySoundButton();
                OnClosePopupPressed();
            });
            btnCloseBg.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySoundButton();
                OnClosePopupPressed();
            });
        }
        public void Init()
        {
            var listTemp = listResourceTypes.OrderBy(item => item).ToList();
            if (listTemp.Count <= 0)
            {
                if (listItemResources.Count <= 0)
                {
                    return;
                }
                for (int i = 0; i < listItemResources.Count; i++)
                {
                    SimplePool.Despawn(listItemResources[i].gameObject);
                }
                listItemResources.Clear();
                return;
            }
            if (listItemResources.Count < listTemp.Count)
            {
                for (int i = listItemResources.Count; i < listTemp.Count; i++)
                {
                    var itemResource = SimplePool.Spawn(itemResourcePrefab);
                    itemResource.transform.SetParent(layoutTransform);
                    itemResource.transform.localScale = Vector3.one; 
                    listItemResources.Add(itemResource);
                }
            }
            else
            {
                for (int i = listItemResources.Count - 1; i > listTemp.Count - 1; i--)
                {
                    var itemResource = listItemResources[i];
                    listItemResources.Remove(itemResource);
                    SimplePool.Despawn(itemResource.gameObject);
                }
            }

            DisplayInfo(listTemp);
        }

        private void DisplayInfo(List<ResourceType> listTemp)
        {
            for (int i = 0; i < listTemp.Count; i++)
            {
                listItemResources[i].Init(EquipmentDataManager.Instance.GetResource(listTemp[i]));
            }
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (IsShow)
            {
                Init();
            }
        }
        [PropertyTooltip("Remember to config code when add new ResourceType")]
        [PropertyOrder(0)]
        [Button]
        public void AutoAddResourceType()
        {
            listResourceTypes.Clear();
            for (int i = 0; i < (int)ResourceType.Blueprint_Boot + 1; i++)
            {
                listResourceTypes.Add((ResourceType)i);
            }
        }
    }
}
