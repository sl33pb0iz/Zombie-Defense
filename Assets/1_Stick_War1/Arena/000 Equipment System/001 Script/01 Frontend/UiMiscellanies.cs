using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;
using UnityEngine.UI;

namespace Snowyy.EquipmentSystem
{
    public class UiMiscellanies : MonoBehaviour
    {
        [Title("Buttons")]
        [SerializeField] private Button btnResources;
        private void Start()
        {
            btnResources.onClick.AddListener(OnClickBtnResources);
        }
        private void OnClickBtnResources()
        {
            SoundManager.Instance.PlaySoundButton();
            UiEquipmentSystemBrain.Instance.UiPopupResources.Show(true);
        }
    }
}
