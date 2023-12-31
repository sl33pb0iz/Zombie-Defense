using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class HealthPickup : PickUp
    {
        [Header("Parameters")]
        [Tooltip("Amount of health to heal on pickup")]
        public float HealAmount;
        protected override void OnPicked(PlayerStateMachine player)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth && playerHealth.CanPickup())
            {
                playerHealth.Heal(HealAmount);
                PlayPickupFeedback(player);
                gameObject.SetActive(false);
            }
        }
    }
}
