using Sirenix.OdinInspector;
using Snowyy;
using Snowyy.EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class ItemDailyShop : MonoBehaviour
    {
        [Title("BUTTONS", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private Button btnPurchase;

        [Title("IMAGES", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private Image imgIconItem;
        [SerializeField] private Image imgIconBorder;
        [SerializeField] private Image imgIconCurrency;

        [Title("TEXTS", titleAlignment: TitleAlignments.Centered)]

        [SerializeField] private TextMeshProUGUI txtName;
        [SerializeField] private TextMeshProUGUI txtQuantity;
        [SerializeField] private TextMeshProUGUI txtPrice;

        [Title("HOLDERS", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private GameObject currencyGo;
        [SerializeField] private GameObject freeGo;
        [SerializeField] private GameObject videoGo;
        [SerializeField] private GameObject purchasedGo;

        [Title("DATA", titleAlignment: TitleAlignments.Centered)]
        public D_SurvivorShop dataSurvivorShop;
        public DataRarityEffect dataRarityEffect;
        public DataResources dataResources;

        public ItemToPurchase currentItem { get; private set; }

        private int index = 0;

        private void Start()
        {
            btnPurchase.onClick.AddListener(OnClickBtnPurchase);
        }

        private void OnClickBtnPurchase()
        {
            SoundManager.Instance.PlaySoundButton();

            if (SurvivorShopDataManager.Instance.GetItemDailyPurchased(index))
            {
                GameManager.Instance.UiController.PopupNotEnough("YOU HAVE PURCHASED THIS ITEM!!");
                return;
            }

            var price = currentItem.pricePerItem * currentItem.quantity;
            switch (currentItem.currencyType)
            {
                case CurrencyType.FREE:
                    GrantReward();
                    break;
                case CurrencyType.VIDEO:
                    UnicornAdManager.ShowAdsReward(() => GrantReward(), StringHelper.REWARD_VIDEO_DAILYSHOP);
                    break;
                case CurrencyType.GEM:
                    if (GameManager.Instance.Profile.GetGem() < price)
                    {
                        //TODO: POP UP NOT ENOUGH GEM
                        GameManager.Instance.UiController.PopupNotEnough("NOT ENOUGH GEM");
                        return;
                    }

                    GameManager.Instance.Profile.AddGem(-price, "gem_spent_dailyShop");
                    GrantReward();
                    break;
                case CurrencyType.GOLD:
                    if (GameManager.Instance.Profile.GetGold() < price)
                    {
                        //TODO: POP UP NOT ENOUGH GOLD
                        GameManager.Instance.UiController.PopupNotEnough("NOT ENOUGH GOLD");
                        return;
                    }

                    GameManager.Instance.Profile.AddGold(-price, "gold_spent_dailyShop");
                    GrantReward();
                    break;
            }
        }

        private void GrantReward(int x = 0)
        {
            switch (currentItem.shopItemType)
            {
                case ShopItemType.EQUIPMENT:
                    EquipmentDataManager.Instance.AddEquipment(currentItem.equipment);
                    EquipmentDataManager.Instance.SaveAllEquipments();
                    SurvivorShopDataManager.Instance.SetItemDailyPurchased(index, true);
                    break;
                case ShopItemType.GEM:
                    GameManager.Instance.Profile.AddGem(currentItem.quantity, "DAILY_FREE_GEM");
                    var dailyGemCount = SurvivorShopDataManager.Instance.GetGemEachDayCount();
                    SurvivorShopDataManager.Instance.SetGemEachDayCount(dailyGemCount + 1);
                    break;
                case ShopItemType.DESIGN:
                    var currentResourceQuantity =
                        EquipmentDataManager.Instance.GetResource(currentItem.resourceType).quantity;
                    currentResourceQuantity += currentItem.quantity;
                    EquipmentDataManager.Instance.SetResource(currentItem.resourceType,
                        new Resource(currentItem.resourceType, currentResourceQuantity));
                    SurvivorShopDataManager.Instance.SetItemDailyPurchased(index, true);

                    break;
                default:
                    Debug.Log($"this ShopItemType {currentItem.shopItemType} isn't implemented!!");
                    break;
            }

            SoundManager.Instance.PlaySoundReward();
            //REFRESH
            GameManager.Instance.UiController.UiSurvivorShop.UiDailyShop.Init();
        }

        public void InitItem(ItemToPurchase itemToPurchase, int index)
        {
            this.index = index;
            if (itemToPurchase == null) return;
            currentItem = itemToPurchase;
            InitIconItem();
            InitTextQuantity();
            InitName();
            InitPrice();
            //InitBorder();
        }

        public void InitIconItem()
        {
            if (currentItem.shopItemType == ShopItemType.EQUIPMENT)
            {
                imgIconItem.sprite =
                    currentItem.equipment.itemTemplate.arrUpgradeIcons[(int)currentItem.equipment.Rarity];
                return;
            }

            if (currentItem.shopItemType == ShopItemType.DESIGN)
            {
                imgIconItem.sprite = dataResources.dictResourceInfos[currentItem.resourceType].iconResource;
                return;
            }

            if (currentItem.shopItemType == ShopItemType.GEM)
            {
                imgIconItem.sprite = dataSurvivorShop.gemSprite;
                return;
            }

            if (currentItem.shopItemType == ShopItemType.GOLD)
            {
                imgIconItem.sprite = dataSurvivorShop.goldSprite;
                return;
            }
        }

        public void InitTextQuantity()
        {
            if (currentItem.shopItemType == ShopItemType.EQUIPMENT)
            {
                txtQuantity.gameObject.SetActive(false);
                return;
            }

            txtQuantity.text = $"X{currentItem.quantity}";
            txtQuantity.gameObject.SetActive(true);
        }

        public void InitBorder()
        {
            if (currentItem.shopItemType == ShopItemType.EQUIPMENT)
            {

                imgIconBorder.gameObject.SetActive(true);
                imgIconBorder.sprite =
                    dataRarityEffect.dictRarityBasedSprites[currentItem.equipment.Rarity].borderSprite;
                return;
            }

            if (imgIconBorder.gameObject.activeSelf)
            {

            imgIconBorder.sprite = dataRarityEffect.dictRarityBasedSprites[Snowyy.Rarity.COMMON].borderSprite;
            imgIconBorder.gameObject.SetActive(false);
            }
        }

        public void InitName()
        {
            if (currentItem.shopItemType == ShopItemType.EQUIPMENT)
            {
                txtName.text = currentItem.equipment.itemTemplate.name;
                return;
            }

            if (currentItem.shopItemType == ShopItemType.DESIGN)
            {

                txtName.text = currentItem.resourceType switch
                {
                    ResourceType.Blueprint_Weapon => "WEAPON DESIGNS",
                    ResourceType.Blueprint_Backpack => "BACKPACK DESIGNS",
                    ResourceType.Blueprint_Helmet => "HELMET DESIGNS",
                    ResourceType.Blueprint_Armor => "ARMOR DESIGNS",
                    ResourceType.Blueprint_Boot => "BOOT DESIGNS",
                    _ => "UNKNOWN",
                };
                return;
            }

            txtName.text = $"{currentItem.shopItemType}";
        }

        public void InitPrice()
        {
            if (SurvivorShopDataManager.Instance.GetItemDailyPurchased(index))
            {
                purchasedGo.SetActive(true);
                freeGo.SetActive(false);
                videoGo.SetActive(false);
                currencyGo.SetActive(false);
                return;
            }

            purchasedGo.SetActive(false);

            if (currentItem.currencyType == CurrencyType.FREE)
            {
                freeGo.SetActive(true);
                videoGo.SetActive(false);
                currencyGo.SetActive(false);
                return;
            }

            if (currentItem.currencyType == CurrencyType.VIDEO)
            {
                freeGo.SetActive(false);
                videoGo.SetActive(true);
                currencyGo.SetActive(false);
                return;
            }

            freeGo.SetActive(false);
            videoGo.SetActive(false);
            currencyGo.SetActive(true);

            txtPrice.text = $"{currentItem.quantity * currentItem.pricePerItem}";

            imgIconCurrency.sprite = currentItem.currencyType == CurrencyType.GOLD
                ? dataSurvivorShop.goldSprite
                : dataSurvivorShop.gemSprite;
        }
    }


    
}