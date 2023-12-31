using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;

namespace Spicyy.Weapon
{
    public class FlamethrowerGun : WeaponControl
    {
        [Title("HOLDER", titleAlignment: TitleAlignments.Centered)]
        public ParticleSystem shootingVFX;
        public ProjectileBase projectileBase;

        private void Start()
        {
            shootingVFX.Clear();
            shootingVFX.Stop();
        }

        private void OnDisable()
        {
            shootingVFX.Clear();
            shootingVFX.Stop();
        }

        public override void Attack(Transform target = null)
        {
            if (shootingVFX.isStopped)
            {
                shootingVFX.Play();
            }
            projectileBase.gameObject.SetActive(true);
            projectileBase.Shoot(this);
        }

        public override void StopAttack()
        {
            if (shootingVFX.isPlaying)
            {
                shootingVFX.Clear();
                shootingVFX.Stop();
            }
            projectileBase.gameObject.SetActive(false);
            projectileBase.Shoot(this);
        }

    }
}
