using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class ItemTurretShop : MonoBehaviour
    {
        [Title("BUTTONS", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private Button btnPurchase;

        [Title("IMAGES", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private Image imgIconItem;

        [Title("TEXTS", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private TextMeshProUGUI txtName;
        [SerializeField] private TextMeshProUGUI txtPrice;

        [Title("HOLDERS", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private GameObject currencyGo;
        [SerializeField] private GameObject purchasedGo;

        [Title("DATA", titleAlignment: TitleAlignments.Centered)]
        public D_SurvivorShop dataSurvivorShop;


        private SurvivorShopDataManager survivorShopData => SurvivorShopDataManager.Instance;
        private int index;


        private void Start()
        {
            btnPurchase.onClick.AddListener(OnClickBtnPurchase);
        }

        private void OnClickBtnPurchase()
        {
            SoundManager.Instance.PlaySoundButton();

            if (survivorShopData.GetItemTowerUnlocked(dataSurvivorShop.dataTowerShopItems[index].towerType))
            {
                GameManager.Instance.UiController.PopupNotEnough("YOU HAVE PURCHASED THIS ITEM!!");
                return;
            }

            var price = dataSurvivorShop.dataTowerShopItems[index].priceTower;

            if (GameManager.Instance.Profile.GetGem() < price)
            {
                //TODO: POP UP NOT ENOUGH GEM
                GameManager.Instance.UiController.PopupNotEnough("NOT ENOUGH GEM");
                return;
            }
            GameManager.Instance.Profile.AddGem(-price, "gem_spent_dailyShop");
            GrantReward();
        }

        private void GrantReward(int x = 0)
        {
            survivorShopData.SetItemTowerUnlock(dataSurvivorShop.dataTowerShopItems[index].towerType, true);
            currencyGo.gameObject.SetActive(false);
            purchasedGo.gameObject.SetActive(true);
            SoundManager.Instance.PlaySoundReward();
            //REFRESH
            GameManager.Instance.UiController.UiSurvivorShop.UiDailyShop.Init();
        }

        public void InitItem(int index)
        {
            this.index = index;

            if (survivorShopData.GetItemTowerUnlocked(dataSurvivorShop.dataTowerShopItems[index].towerType))
            {
                currencyGo.SetActive(false);
                purchasedGo.SetActive(true); 
            }
            else
            {
                currencyGo.SetActive(true);
                purchasedGo.SetActive(false); 
            }

            InitName();
            InitIcon();
            InitPrice();
        }

        public void InitIcon()
        {
            imgIconItem.sprite = dataSurvivorShop.dataTowerShopItems[index].towerIcon;
        }

        public void InitPrice()
        {
            txtPrice.text = dataSurvivorShop.dataTowerShopItems[index].priceTower.ToString();
        }

        public void InitName()
        {
            txtName.text = dataSurvivorShop.dataTowerShopItems[index].towerType.ToString();
        }
    }
}
