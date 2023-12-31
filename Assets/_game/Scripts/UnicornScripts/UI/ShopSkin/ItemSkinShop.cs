using System.Collections.Generic;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Unicorn.UI.Shop
{
    [RequireComponent(typeof(Image))]
    public class ItemSkinShop : MonoBehaviour
    {
        [SerializeField] private TypeEquipment typeEquipment;
        [SerializeField] protected Image icon;
        [SerializeField] private bool dontHaveToUse = false;
        [SerializeField] protected List<Button> listBtnBehavior;
        [SerializeField] private Button btnView;
        [SerializeField] protected TextMeshProUGUI txtVideo;
        [SerializeField] private TextMeshProUGUI txtCoin;
        [SerializeField] private TextMeshProUGUI txtGem;

        [SerializeField] private Sprite[] spriteBackgrounds;

        protected DataShop dataShop;
        protected ShopCharacter shopCharacter;
        private IDataSkin playerData;
        private bool isAwakeCalled;

        protected Image ImgBg { get; set; }

        public TypeEquipment TypeEquipment => typeEquipment;

        private void Awake()
        {
            if (isAwakeCalled) return;
            isAwakeCalled = true;
            ImgBg = GetComponent<Image>();

            btnView.onClick.AddListener(OnClickBtnView);
            for (int i = 0; i < listBtnBehavior.Count; i++)
            {
                int id = i;
                listBtnBehavior[i].onClick.AddListener(() => OnClickBtnBehaviour(id));
            }
        }

        public virtual void Init(DataShop data, ShopCharacter shopCharacter)
        {
            Awake();
            this.shopCharacter = shopCharacter;
            playerData = shopCharacter.DataSkin;
            dataShop = data;
            for (int i = 0; i < listBtnBehavior.Count; i++)
            {
                listBtnBehavior[i].gameObject.SetActive(false);
            }

            txtCoin.text = data.numberCoinUnlock.ToString();
            txtGem.text = data.numberGemUnlock.ToString();


            icon.sprite = shopCharacter.DataTextureSkin.GetIcon(typeEquipment, data.idSkin);
            ImgBg.sprite = spriteBackgrounds[0];

            if (PlayerDataManager.Instance.GetUnlockSkin(typeEquipment, data.idSkin))
            {
                if (dontHaveToUse)
                {
                    InitEquiped();
                }
                else
                {
                    InitUnlocked();
                }
            }
            else
            {
                InitLocked();
            }

        }

        private void InitEquiped()
        {
                if (listBtnBehavior[(int)TypeButtonBehavior.EQUIPED].gameObject)
                {
                    listBtnBehavior[(int)TypeButtonBehavior.EQUIPED].gameObject.SetActive(true);
                    ImgBg.sprite = spriteBackgrounds[1];
                }
        }

        private void InitUnlocked()
        {
        
                if (playerData.GetIdEquipSkin(typeEquipment) == dataShop.idSkin)
                {

                    if (listBtnBehavior[(int)TypeButtonBehavior.REMOVE].gameObject)
                    {
                        listBtnBehavior[(int)TypeButtonBehavior.REMOVE].gameObject.SetActive(true);
                        ImgBg.sprite = spriteBackgrounds[1];
                    }
                }
                else
                {
                    if (listBtnBehavior[(int)TypeButtonBehavior.USE].gameObject)
                    {
                        listBtnBehavior[(int)TypeButtonBehavior.USE].gameObject.SetActive(true);
                    }

                }
              
        }

        private void InitLocked()
        {

            if (dataShop.typeUnlock.HasFlag(TypeUnlockSkin.SPIN))
            {
                listBtnBehavior[(int)TypeButtonBehavior.SPIN].gameObject.SetActive(true);
            }

            if (dataShop.typeUnlock.HasFlag(TypeUnlockSkin.COIN))
            {
                listBtnBehavior[(int)TypeButtonBehavior.UNLOCK_BY_COIN].gameObject.SetActive(true);
            }

            if (dataShop.typeUnlock.HasFlag(TypeUnlockSkin.GEM))
            {
                listBtnBehavior[(int)TypeButtonBehavior.UNLOCK_BY_GEM].gameObject.SetActive(true);
            }

            if (dataShop.typeUnlock.HasFlag(TypeUnlockSkin.VIDEO))
            {
                listBtnBehavior[(int)TypeButtonBehavior.UNLOCK_BY_VIDEO].gameObject.SetActive(true);

                int numberWatchVideo = PlayerDataManager.Instance.GetVideoSkinCount(typeEquipment, dataShop.idSkin);
                txtVideo.text = $"{numberWatchVideo}/{dataShop.numberVideoUnlock}";
            }
        }

        protected virtual void OnClickBtnView()
        {
            SoundManager.Instance.PlaySoundButton();
            Apply();
        }

        protected virtual void OnClickBtnBehaviour(int idBehaviour)
        {
            SoundManager.Instance.PlaySoundButton();

            switch ((TypeButtonBehavior)idBehaviour)
            {
                case TypeButtonBehavior.EQUIPED:
                    break;
                case TypeButtonBehavior.SPIN:
                    {
                        shopCharacter.OnBackPressed();
                        GameManager.Instance.UiController.OpenLuckyWheel();
                    }
                    break;
                case TypeButtonBehavior.UNLOCK_BY_VIDEO:
                    {
                        UnicornAdManager.ShowAdsReward(OnRewardedVideo,
                            string.Format(Helper.video_shop_general, typeEquipment));
                        break;
                    }
                case TypeButtonBehavior.UNLOCK_BY_COIN:
                    {
                        if (GameManager.Instance.Profile.GetGold() < dataShop.numberCoinUnlock)
                        {
                            shopCharacter.NotifyNotEnoughGold(this.transform);

                            return;
                        }

                        playerData.SetUnlockSkin(typeEquipment, dataShop.idSkin);
                        GameManager.Instance.Profile.AddGold(-dataShop.numberCoinUnlock, "shop_skin_" + typeEquipment);
                        Apply();
                        shopCharacter.ReloadLayout(typeEquipment);
                        break;
                    }
                case TypeButtonBehavior.UNLOCK_BY_GEM:
                    {
                        if (GameManager.Instance.Profile.GetGem() < dataShop.numberGemUnlock)
                        {
                            shopCharacter.NotifyNotEnoughGem(this.transform);

                            return;
                        }

                        playerData.SetUnlockSkin(typeEquipment, dataShop.idSkin);
                        GameManager.Instance.Profile.AddGem(-dataShop.numberGemUnlock, "shop_skin_" + typeEquipment);
                        Apply();
                        shopCharacter.ReloadLayout(typeEquipment);
                        break;
                    }
                case TypeButtonBehavior.USE:
                    {
                        playerData.SetIdEquipSkin(typeEquipment, dataShop.idSkin);
                        OnClickBtnView();
                        shopCharacter.ReloadLayout(typeEquipment);
                        break;
                    }
                case TypeButtonBehavior.REMOVE:
                    {
                        playerData.SetIdEquipSkin(typeEquipment, 0);
                        Apply(false);
                        playerData.SetIdEquipSkin(typeEquipment, 0);
                        shopCharacter.ReloadLayout(typeEquipment);

                        break;
                    }
                default:
                    Debug.LogError("hello");
                    break;
            }
        }

        private void OnRewardedVideo()
        {
            int numberWatchVideo = playerData.GetVideoSkinCount(typeEquipment, dataShop.idSkin);
            numberWatchVideo++;
            if (numberWatchVideo >= dataShop.numberVideoUnlock)
            {
                playerData.SetUnlockSkin(typeEquipment, dataShop.idSkin);
            }

            playerData.SetVideoSkinCount(typeEquipment, dataShop.idSkin, numberWatchVideo);
            Apply();
            //GameManager.Instance.UiController.ShopCharacter.ReloadLayout(typeEquipment);
        }

        private void Apply(bool isApply = true)
        {
            shopCharacter.SkinChanger.Change(typeEquipment, isApply ? dataShop.idSkin : -1);
        }

    }

    public enum TypeButtonBehavior
    {
        SPIN = 0,
        UNLOCK_BY_VIDEO = 2,
        UNLOCK_BY_COIN = 1,
        USE = 4,
        REMOVE = 3,
        UNLOCK_BY_GEM = 5,
        EQUIPED = 6,

    }

}