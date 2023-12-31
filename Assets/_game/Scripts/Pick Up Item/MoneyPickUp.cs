using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class MoneyPickUp : PickUp
    {
        [Header("Parameters")]
        [Tooltip("Amount of money to add on pickup")]
        public int moneyAddAmount;
        public ItemInfoPopUp _moneyPopUp;

        protected override void OnPicked(PlayerStateMachine player)
        {
            base.OnPicked(player);
            CurrencyManager.AddGold(moneyAddAmount);
            PopUp(player.m_UI.m_InitPopUpTransform.position);
            gameObject.SetActive(false);
        }

        void PopUp(Vector3 position)
        {
            _moneyPopUp.SetUp(moneyAddAmount.ToString());
            GameObject objPopUp = PoolManager.Instance.ReuseObject(_moneyPopUp.gameObject, position, Quaternion.identity);
            objPopUp.SetActive(true);
        }
    }
}
