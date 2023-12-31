using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;

namespace Spicyy.Weapon
{
    public class MultipleMuzzlesGun : WeaponControl
    {
        [Title("PARAMETER", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private float delayBetweenShots;
        [SerializeField] private float bulletSpeed;

        [Title("HOLDER", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform[] weaponMuzzles;
        [SerializeField] private ParticleSystem[] shootVFX;

        private float m_LastTimeShot = Mathf.NegativeInfinity;
        private float timeDelayCountinuous;

        private void Start()
        {
            timeDelayCountinuous = delayBetweenShots / weaponMuzzles.Length;
            ProjectileDamage = WeaponDamage + OwnerDamage;
        }

        public override void Attack(Transform target = null)
        {
            if (m_LastTimeShot + delayBetweenShots < Time.time)
            {
                for (int index = 0; index < weaponMuzzles.Length; index++)
                {
                    StartCoroutine(ContinuousShoot(weaponMuzzles[index], target, index));
                }
                m_LastTimeShot = Time.time;
            }
        }

        public override void StopAttack()
        {
            base.StopAttack();
            StopAllCoroutines();
        }

        IEnumerator ContinuousShoot(Transform muzzle, Transform target, int index)
        {
            yield return new WaitForSeconds(index * timeDelayCountinuous);
            float speedBullet = bulletSpeed;
            GameObject newProjectile = PoolManager.Instance.ReuseObject(bulletPrefab, muzzle.position, Quaternion.LookRotation(muzzle.forward));
            shootVFX[index].Clear();
            shootVFX[index].Play();
            newProjectile.SetActive(true);
            ProjectileBase projectileBase = newProjectile.GetComponent<ProjectileBase>();
            projectileBase.Shoot(this, target, speedBullet);
        }
    }

}
