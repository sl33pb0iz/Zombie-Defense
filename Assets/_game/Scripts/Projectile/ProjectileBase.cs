using Spicyy.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unicorn
{
    public class ProjectileBase : MonoBehaviour
    {
        public GameObject Owner { get; private set; }
        public Vector3 InitialPosition { get; private set; }
        public Vector3 InitialDirection { get; private set; }
        public float InitialSpeed { get; private set; }
        public float InitialDamage { get; private set; }
        public Vector3 InheritedMuzzleVelocity { get; private set; }

        public UnityAction<Transform> OnShoot;

        public void Shoot(WeaponControl controller, Transform target = null, float speed = 0)
        {
            Owner = controller.Owner;
            InitialPosition = transform.position;
            InitialSpeed = speed;
            InitialDamage = controller.ProjectileDamage;
            InheritedMuzzleVelocity = controller.MuzzleWorldVelocity;
            OnShoot?.Invoke(target);
        }
    }
}
