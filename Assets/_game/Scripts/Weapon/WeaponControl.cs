using Sirenix.OdinInspector;
using UnityEngine;

namespace Spicyy.Weapon
{
    public abstract class WeaponControl : MonoBehaviour
    {
        [Title("MAIN", titleAlignment: TitleAlignments.Centered)]
        public Transform aimDirection;

        public GameObject Owner { get; set; }
        public bool IsWeaponActive { get;  set; }
        public Vector3 MuzzleWorldVelocity { get; set; }
        public float ProjectileDamage { get; set; }
        public float WeaponDamage { get; set; }
        public float OwnerDamage { get; set; }

        public abstract void Attack(Transform target = null);
        public virtual void StopAttack() { }
    }

}
