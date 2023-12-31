using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    /// <summary>
    /// LuckyWheel component
    /// </summary>
    public class ItemFreeReward : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI txtNumberSpin;

        //[SerializeField] private GameObject ObjTick;
        [SerializeField] private Image imgBg;
        [SerializeField] private List<Sprite> listSprBg;


        private float WidthBar = 755;

        public void Init(RewardEndGame data)
        {
            txtNumberSpin.text = data.NumberWin.ToString();
            bool unlock = PlayerDataManager.Instance.GetUnlockSkin(data.Type, data.Id);
            if (!unlock)
            {
                int numberWatchVideo = PlayerDataManager.Instance.GetNumberWatchVideoSpin();
                if (data.NumberWin <= numberWatchVideo)
                {

                    unlock = true;

                    RewardEndGame reward = new RewardEndGame();
                    reward.Type = data.Type;
                    reward.Id = data.Id;

                    GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                    PlayerDataManager.Instance.SetUnlockSkin(data.Type, data.Id);

                }
            }

            //ObjTick.SetActive(unlock);
            int indexBg = unlock ? 0 : 1;
            imgBg.sprite = listSprBg[indexBg];
            if (unlock)
            {
                icon.gameObject.SetActive(false);
            }

            icon.sprite = GameManager.Instance.PlayerDataManager.DataTextureSkin.GetIcon(data.Type, data.Id);
            icon.SetNativeSize();
            float ratio = (float) data.NumberWin / 10f;
            var v3 = new Vector3(ratio * WidthBar, 10, 0);
            this.GetComponent<RectTransform>().anchoredPosition = v3;
        }
    }

}