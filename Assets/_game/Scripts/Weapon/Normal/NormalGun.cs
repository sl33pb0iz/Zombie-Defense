using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;

namespace Spicyy.Weapon
{
    public class NormalGun : WeaponControl
    {
        [Title("PARAMETER", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private float delayBetweenShots;
        [SerializeField] private float bulletSpeed;

        [Title("HOLDER", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private Transform Muzzle; 
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private ParticleSystem shootVFX; 

        float m_LastTimeShot = Mathf.NegativeInfinity;

        private void Start()
        {
            ProjectileDamage = WeaponDamage + OwnerDamage;
        }

        public override void Attack(Transform target = null)
        {
            if (m_LastTimeShot + delayBetweenShots < Time.time)
            {
                float speedBullet = bulletSpeed;
                GameObject newProjectile = PoolManager.Instance.ReuseObject(bulletPrefab, Muzzle.position, Quaternion.LookRotation(Muzzle.forward));
                shootVFX.Clear();
                shootVFX.Play();
                newProjectile.SetActive(true);
                ProjectileBase projectileBase = newProjectile.GetComponent<ProjectileBase>();
                projectileBase.Shoot(this, target, speedBullet);
                m_LastTimeShot = Time.time;
            }
        }
       
    }

}
