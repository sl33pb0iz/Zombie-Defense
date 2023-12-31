using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class ProjectilePlasma : ProjectileBase
    {
        [Header("General")]
        [Tooltip("Radius of this projectile's collision detection")]
        public float Radius = 0.01f;

        [Tooltip("Transform representing the root of the projectile (used for accurate collision detection)")]
        public Transform Root;

        [Tooltip("Transform representing the tip of the projectile (used for accurate collision detection)")]
        public Transform Tip;

        [Tooltip("LifeTime of the projectile")]
        public float MaxLifeTime = 5f;

        [Tooltip("VFX prefab to spawn upon impact")]
        public GameObject ImpactVfx;

        [Tooltip("LifeTime of the VFX before being destroyed")]
        public float ImpactVfxLifetime = 5f;

        [Tooltip("Offset along the hit normal where the VFX will be spawned")]
        public float ImpactVfxSpawnOffset = 0.1f;

        [Tooltip("Layers this projectile can collide with")]
        public LayerMask HittableLayers = -1;

        [Header("Movement")]
        [Tooltip("Speed of the projectile")]
        private float Speed;

        [Header("Damage")]
        ProjectileBase m_ProjectileBase;
        Vector3 m_LastRootPosition;
        Vector3 m_Velocity;
        List<Collider> m_IgnoredColliders;
        public int throughNumber = 2;

        const QueryTriggerInteraction k_TriggerInteraction = QueryTriggerInteraction.Collide;

        private float Damage;

        private void Awake()
        {
            m_ProjectileBase = GetComponent<ProjectileBase>();
        }

        private void OnEnable()
        {
            m_ProjectileBase.OnShoot += IsShooting;
        }

        private void OnDisable()
        {
            m_ProjectileBase.OnShoot -= IsShooting;
        }

        private void Update()
        {
            transform.position += m_Velocity * Time.deltaTime;
            transform.forward = m_Velocity.normalized;
            m_LastRootPosition = Root.position;
        }

        bool IsHitValid(Collider hit)
        {
            // ignore hits with triggers that don't have a Damageable component
            if (hit.isTrigger && hit.GetComponent<Damageable>() == null)
            {
                return false;
            }
            // ignore hits with specific ignored colliders (self colliders, by default)
            if (m_IgnoredColliders != null && m_IgnoredColliders.Contains(hit))
            {
                return false;
            }
            return true;
        }

        void IsShooting(Transform target)
        {
            m_LastRootPosition = Root.position;
            Speed = m_ProjectileBase.InitialSpeed;
            Damage = m_ProjectileBase.InitialDamage;
            m_Velocity = transform.forward * Speed;
            m_IgnoredColliders = new List<Collider>();
            transform.position += m_ProjectileBase.InheritedMuzzleVelocity * Time.deltaTime;

            Vector3 directionToTarget = target.position - transform.position + new Vector3(0, 2, 0);
            Vector3 currentDirection = transform.forward;
            float maxTurnSpeed = 360f; // degrees per second
            Vector3 resultingDirection = Vector3.RotateTowards(currentDirection, directionToTarget, maxTurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 1f);
            transform.rotation = Quaternion.LookRotation(resultingDirection);
            
        }

        void OnHit(Vector3 point, Vector3 normal, Collider collider)
        {
            // point damage
            Damageable damageable = collider.GetComponent<Damageable>();
            if (damageable)
            {
                damageable.InflictDamage(Damage, false, m_ProjectileBase.Owner);
                throughNumber--;
            }

            // impact vfx
            if (ImpactVfx)
            {
                GameObject impactVfxInstance = Instantiate(ImpactVfx, point + (normal * ImpactVfxSpawnOffset),
                    Quaternion.LookRotation(normal));
                if (ImpactVfxLifetime > 0)
                {
                    Destroy(impactVfxInstance.gameObject, ImpactVfxLifetime);
                }
            }
            //Check Self Destruct
            if(throughNumber <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (HittableLayers == (HittableLayers | (1 << other.gameObject.layer)))
            {
                if (IsHitValid(other))
                {
                    OnHit(Root.position, -transform.forward, other);
                }
            }
        }
    }
}
