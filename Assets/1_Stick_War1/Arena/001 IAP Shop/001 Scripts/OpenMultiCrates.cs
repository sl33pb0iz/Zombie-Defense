using Snowyy.EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class OpenMultiCrates : UICanvas
    {
        [SerializeField] private Button btnClose;

        [SerializeField] private TextMeshProUGUI txtTapToClose;

        [SerializeField] private ItemMultiCrateResult[] itemMultiCrateResults;
        private bool canShow = false;

        private void Start()
        {
            btnClose.onClick.AddListener(OnClickBtnClose);
        }

        public void Init(List<Equipment> listEquipment)
        {
            txtTapToClose.gameObject.SetActive(false);
            canShow = false;
            for (int i = 0; i < itemMultiCrateResults.Length; i++)
            {
                itemMultiCrateResults[i].Init(listEquipment[i]);
                itemMultiCrateResults[i].gameObject.SetActive(false);
            }

            Show(true);
            StartCoroutine(ShowResult());
        }

        private void OnClickBtnClose()
        {
            if (!canShow) return;
            canShow = false;
            OnClosePopupPressed();
            GameManager.Instance.UiController.UiSurvivorShop.UiOpenCrate.OnBackPressed();
            GameManager.Instance.UiController.UiSurvivorShop.Init();
        }

        IEnumerator ShowResult()
        {
            for (int i = 0; i < itemMultiCrateResults.Length; i++)
            {
                //SoundManager.Instance.PlayFxSound(soundEnum: SoundManager.GameSound.MultiCrateResult);
                itemMultiCrateResults[i].gameObject.SetActive(true);
                yield return Yielders.Get(0.2f);
            }

            txtTapToClose.gameObject.SetActive(true);
            canShow = true;
        }
    }
}