using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Unicorn.UI.Shop
{
    public class ShopContent : MonoBehaviour
    {
        [Space(10)] [SerializeField] private int pageSize = 6;
        [SerializeField] private RectTransform pagePrefab;
        [SerializeField] private ItemSkinShop itemSkinShopPrefab;
        
        [FoldoutGroup("Persistence")] [SerializeField]
        private RectTransform rectTransParent;

        [FoldoutGroup("Persistence")] [SerializeField]
        private HorizontalScrollSnap_Lobby scrollViewController;

        private readonly List<RectTransform> pages = new List<RectTransform>();
        private readonly List<ItemSkinShop> itemSkins = new List<ItemSkinShop>();

        
        private int currentIndex;
        private int maxIndex;

        public TypeEquipment TypeEquipment => itemSkinShopPrefab.TypeEquipment;
        public ShopCharacter ShopCharacter { get; set; }

        protected virtual void Start()
        {
            if (scrollViewController)
                scrollViewController.OnSelectionPageChangedEvent.AddListener(ChangeTab);
        }

        public virtual void InitContent(List<DataShop> listData, bool isRestPosTab = true)
        {
            if (isRestPosTab)
            {
                rectTransParent.anchoredPosition = Vector3.zero;
                currentIndex = 0;
            }

            maxIndex = GetPageIndex(listData.Count);
            InitializeItems(listData);
        }

        private void InitializeItems(List<DataShop> shopData)
        {
            CreateItems(shopData.Count);
            for (int i = 0; i < shopData.Count; i++)
            {
                itemSkins[i].Init(shopData[i], ShopCharacter);
            }
        }

        private void CreateItems(int itemCount)
        {
            if (!Application.isPlaying)
            {
                ClearAllPages();
            }

            if (itemSkins.Count >= itemCount)
                return;

            CreatePages(itemCount);

            for (var index = itemSkins.Count; index < itemCount; index++)
            {
                int currentPageIndex = GetPageIndex(index + 1);
                var itemSkin = Instantiate(itemSkinShopPrefab, pages[currentPageIndex].GetChild(0));
                itemSkins.Add(itemSkin);

                itemSkin.name = $"Item_{index}";
                if (Application.isEditor)
                {
                    itemSkin.hideFlags = HideFlags.DontSave;
                }
            }
        }

        private void CreatePages(int itemCount)
        {
            if (!Application.isPlaying)
            {
                ClearAllPages();
            }

            int totalPage = GetTotalPage(itemCount);
            if (pages.Count > totalPage) return;

            for (int i = pages.Count + 1; i <= totalPage; i++)
            {
                var newPage = Instantiate(pagePrefab, scrollViewController.transform.GetChild(0));
                pages.Add(newPage);
                newPage.name = $"Page_{i}";
                if (!Application.isPlaying)
                {
                    newPage.gameObject.hideFlags = HideFlags.DontSave;
                }
            }
        }

        private void ClearAllPages()
        {
            foreach (RectTransform rectTransform in pages)
            {
                if (Application.isPlaying)
                {
                    Destroy(rectTransform.gameObject);
                }
                else
                {
                    DestroyImmediate(rectTransform.gameObject);
                }
            }

            pages.Clear();
            itemSkins.Clear();
        }

        private int GetPageIndex(int itemCount)
        {
            return Mathf.FloorToInt((float) (itemCount - 1) / pageSize);
        }

        private int GetTotalPage(int itemCount)
        {
            int pageIndex = GetPageIndex(itemCount);
            return pageIndex + 1;
        }

        public void Reset()
        {
            rectTransParent.anchoredPosition = Vector3.zero;
            currentIndex = 0;

            //GameManager.Instance.UiController.ShopCharacter.SetupTabBottom(currentIndex, GetTotalPage(itemSkins.Count));
        }

        private void ChangeTab(int index)
        {
            if (currentIndex == index)
                return;

            currentIndex = index;
            //GameManager.Instance.UiController.ShopCharacter.SetupTabBottom(currentIndex, GetTotalPage(itemSkins.Count));
        }

        public void Change(int id)
        {
            ShopCharacter.SkinChanger.Change(TypeEquipment, id);
        }
    }

}