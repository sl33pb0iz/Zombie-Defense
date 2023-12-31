using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Unicorn
{
    public class MoneyUI : MonoBehaviour
    {
        public TextMeshProUGUI moneyUI;

        private void Update()
        {
            moneyUI.text = PlayerDataManager.Instance.GetGold().ToString();
        }

    }
}
