using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;

namespace Spicyy.Weapon
{
    public class ShotGun : WeaponControl
    {
        [Title("PARAMETER", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private float bulletsPerShot;
        [SerializeField] private float bulletSpreadAngle;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float delayBetweenShots;
        
        [Title("HOLDER", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private ParticleSystem shootVFX;

        float m_LastTimeShot = Mathf.NegativeInfinity;

        public void Start()
        {
            ProjectileDamage = WeaponDamage + OwnerDamage;
        }

        public override void Attack(Transform target = null)
        {
            if (m_LastTimeShot + delayBetweenShots < Time.time)
            {
                for (int index = 1; index <= bulletsPerShot; index++)
                {
                    float speedBullet = GetShotWithinRandomSpeed();
                    Vector3 shotDirection = GetShotDirectionWithinSpread(aimDirection);
                    GameObject newProjectile = PoolManager.Instance.ReuseObject(bulletPrefab, aimDirection.position, Quaternion.LookRotation(index * shotDirection));
                    shootVFX.Clear();
                    shootVFX.Play();
                    newProjectile.SetActive(true);
                    ProjectileBase projectileBase = newProjectile.GetComponent<ProjectileBase>();
                    projectileBase.Shoot(this, null, speedBullet);

                }
                    m_LastTimeShot = Time.time;
            }
        }

        public Vector3 GetShotDirectionWithinSpread(Transform muzzle)
        {
            float spreadAngleRatio = bulletSpreadAngle / 180f;
            Vector3 spreadWorldDirection = Vector3.Slerp(muzzle.rotation * Vector3.forward, Random.insideUnitSphere, spreadAngleRatio);
            return spreadWorldDirection;
        }

        public float GetShotWithinRandomSpeed()
        {
            float randomSpeed = bulletSpeed;
            if (bulletsPerShot > 1)
            {
                randomSpeed += UnityEngine.Random.Range(-8f, 8f);
            }
            else randomSpeed = bulletSpeed;
            return randomSpeed;
        }

    }
}
