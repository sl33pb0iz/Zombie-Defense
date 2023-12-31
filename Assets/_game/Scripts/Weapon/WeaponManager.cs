using Spicyy.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Unicorn
{
    public class WeaponManager : MonoBehaviour
    {
        public List<WeaponControl> Weapons = new List<WeaponControl>();

        private PlayerSkinChanger playerSkinChanger;

        private void Awake()
        {
            playerSkinChanger = GetComponent<PlayerSkinChanger>();
        }

        public void Init(GameObject owner, float damage)
        {
            foreach (var weaponInstance in Weapons)
            {
                weaponInstance.Owner = owner;
                weaponInstance.OwnerDamage = damage;
            }
        }

        #region GetWeapon
        public WeaponControl GetActiveWeapon()
        {
            int ActiveWeaponIndex = playerSkinChanger.weaponActiveId;
            return GetWeaponAtSlotIndex(ActiveWeaponIndex);
        }
        public WeaponControl GetWeaponAtSlotIndex(int index)
        {
            // find the active weapon in our weapon slots based on our active weapon index
            if (index >= 0 && index < Weapons.Count)
            {
                return Weapons[index];
            }
            // if we didn't find a valid active weapon in our weapon slots, return null
            return null;
        }
        #endregion


    }

}
