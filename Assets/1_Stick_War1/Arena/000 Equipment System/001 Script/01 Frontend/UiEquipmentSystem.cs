using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using Unicorn.UI;
using UnityEngine;
using Unicorn.Examples;

namespace Snowyy.EquipmentSystem
{
    public class UiEquipmentSystem : UICanvas
    {
        [Title("LAYOUT", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] private Canvas parentCanvas;
        [SerializeField] private RectTransform contentRectTransform;

        [Title("MANAGERS", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] private EquipSlotManager equipSlotManager;
        [SerializeField] private InventoryManager inventoryManager;

        [Title("PREVIEW", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] private Canvas previewCanvas;
        [SerializeField] private EquipmentChangerPreviewCharacter previewCharacter;

        public EquipSlotManager EquipSlotManager => equipSlotManager;
        public InventoryManager InventoryManager => inventoryManager;
        public EquipmentChangerPreviewCharacter PreviewCharacter => previewCharacter;

        private float originContentHeight;

        public OnRefreshDisplay OnRefresh { get; private set; }

        protected override void Awake()
        {
            originContentHeight = contentRectTransform.sizeDelta.y;
            
        }


        private void Start()
        {
            OnRefresh += Refresh;

        }
        
        private void LateUpdate()
        {
            UpdateContentPos();
        }

        private void UpdateContentPos()
        {
            contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, originContentHeight / parentCanvas.transform.localScale.y);
        }

        private void Refresh()
        {
            inventoryManager.Init();
            equipSlotManager.Init();
            previewCharacter.Init();
        }

        private void Init()
        {
            inventoryManager.Init();
            equipSlotManager.Init();
            previewCharacter.Init();
        }

        private void OnFirstTime()
        {

        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (IsShow)
            {
                Init();
            }
        }

    }
}
