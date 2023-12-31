using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class GemPickUp : PickUp
    {
        [Header("Parameters")]
        [Tooltip("Amount of gem to add on pickup")]
        public int gemBonus;
        protected override void OnPicked(PlayerStateMachine player)
        {
            CurrencyManager.AddGem(gemBonus);
            PlayPickupFeedback(player);
            gameObject.SetActive(false);
        }
    }
}

