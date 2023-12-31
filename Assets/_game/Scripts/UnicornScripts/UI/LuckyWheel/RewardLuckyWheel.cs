using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.UI
{
    /// <summary>
    /// <see cref="LuckyWheel"/> component
    /// </summary>
    public class RewardLuckyWheel : MonoBehaviour
    {
        [SerializeField] private Image iconReward;
        [SerializeField] private TextMeshProUGUI txtNumberCoin;

        private Transform transIcon;
        private Transform tranNumberCoin;

        private void Start()
        {
            tranNumberCoin = txtNumberCoin.GetComponent<Transform>();
            transIcon = iconReward.GetComponent<Transform>();
        }

        public void Init(DataRewardLuckyWheel dataLuckyWheel)
        {
            var playerData = GameManager.Instance.PlayerDataManager;
            txtNumberCoin.gameObject.SetActive(true);
            switch (dataLuckyWheel.Type)
            {
                case TypeGift.GOLD:
                    {
                        iconReward.sprite = playerData.DataTexture.IconCoinLuckyWheel;
                        txtNumberCoin.text = dataLuckyWheel.Amount.ToString();
                    }
                    break;

                case TypeGift.GEM:
                    {
                        iconReward.sprite = playerData.DataTexture.IconGemLuckyWheel;
                        txtNumberCoin.text = dataLuckyWheel.Amount.ToString();
                    }
                    break;

                case TypeGift.HAT:
                    {
                        if (PlayerDataManager.Instance.GetUnlockSkin(TypeEquipment.Hat, dataLuckyWheel.IdType))
                        {
                            iconReward.sprite = playerData.DataTexture.IconGemLuckyWheel;
                            txtNumberCoin.text = dataLuckyWheel.NumberCoinReplace.ToString();
                        }
                        else
                        {
                            iconReward.sprite =
                                playerData.DataTextureSkin.GetIcon(TypeEquipment.Hat, dataLuckyWheel.IdType);
                            txtNumberCoin.gameObject.SetActive(false);
                        }
                    }
                    break;
                case TypeGift.SKIN:
                    {
                        if (PlayerDataManager.Instance.GetUnlockSkin(TypeEquipment.Skin, dataLuckyWheel.IdType))
                        {
                            iconReward.sprite = playerData.DataTexture.IconGem;
                            txtNumberCoin.text = dataLuckyWheel.NumberCoinReplace.ToString();
                        }
                        else
                        {
                            iconReward.sprite =
                                playerData.DataTextureSkin.GetIcon(TypeEquipment.Skin, dataLuckyWheel.IdType);
                            txtNumberCoin.gameObject.SetActive(false);
                        }
                    }
                    break;
                case TypeGift.PET:
                    {
                        if (PlayerDataManager.Instance.GetUnlockSkin(TypeEquipment.Pet, dataLuckyWheel.IdType))
                        {
                            iconReward.sprite = playerData.DataTexture.IconGem;
                            txtNumberCoin.text = dataLuckyWheel.NumberCoinReplace.ToString();
                        }
                        else
                        {
                            iconReward.sprite =
                                playerData.DataTextureSkin.GetIcon(TypeEquipment.Pet, dataLuckyWheel.IdType);
                            txtNumberCoin.gameObject.SetActive(false);
                        }
                    }
                    break;
                default:
                    break;
            }
            iconReward.preserveAspect = true;
        }

        private void Update()
        {
            //v3Rotate.z = -(_transWheel.eulerAngles.z + angle);
            tranNumberCoin.rotation = Quaternion.identity;
            transIcon.rotation = Quaternion.identity;
        }
    }

}