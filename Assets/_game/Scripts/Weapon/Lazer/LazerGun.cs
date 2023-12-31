using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spicyy.Weapon
{
    public class LazerGun : WeaponControl
    {

        [Title("PARAMETER", titleAlignment: TitleAlignments.Centered)]
        public float _damage;
        public float _timeDelayDamage;
        public float _maxLength;

        [Title("HOLDERS", titleAlignment: TitleAlignments.Centered)]
        public ProjectileLaser m_LaserLine;
        public LayerMask _hittableLayer;
        public LayerMask _damageLayer;

        private void Start()
        {
            m_LaserLine.HittableLayer = _hittableLayer;
            m_LaserLine.DamageLayer = _damageLayer;
            m_LaserLine.Damage = _damage;
            m_LaserLine.TimeDelayDamage = _timeDelayDamage;
            m_LaserLine.MaxLength = _maxLength;
        }

        public override void Attack(Transform target = null)
        {
            m_LaserLine.FireLaser();
        }

        public override void StopAttack()
        {
            m_LaserLine.DisablePrepare();
        }
    }

}
