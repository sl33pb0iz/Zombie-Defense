using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class ShellAmmoPickup : PickUp
    {
        [Header("Parameters")]
        [Tooltip("Amount of health to heal on pickup")]
        public float shellAmount;

        protected override void OnPicked(PlayerStateMachine player)
        {
            /*WeaponController playerWeapon = player.GetComponent<WeaponController>();
            if(playerWeapon.m_CurrentShell < playerWeapon.weaponData.maxShellAmount)
            {
                playerWeapon.m_CurrentShell += shellAmount;
                PlayPickupFeedback();
                Destroy(gameObject);
            }*/
        }
    }
}
