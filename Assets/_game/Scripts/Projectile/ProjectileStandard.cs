using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Unicorn
{
    public class ProjectileStandard : ProjectileBase
    {

        [SerializeField][BoxGroup("General")][Tooltip("LifeTime of the projectile")]
        private float MaxLifeTime = 5f;

        [SerializeField][BoxGroup("General")][Tooltip("VFX prefab to spawn upon impact")]
        private GameObject ImpactVfx;

        [SerializeField][BoxGroup("General")][Tooltip("Offset along the hit normal where the VFX will be spawned")]
        private float ImpactVfxSpawnOffset = 0.1f;

        [SerializeField][BoxGroup("General")][Tooltip("Layers this projectile can collide with")]
        private LayerMask HittableLayers = -1;

        [SerializeField][BoxGroup("General")][Tooltip("Layers this projectile can damage with")]
        private LayerMask damageLayer = -1;

        [SerializeField][BoxGroup("Movement")][Tooltip("Knock back")]public bool knockBack;
        [ShowIfGroup("knockBack")]
        [BoxGroup("knockBack/Knock Back Force")] public float knockbackForce;

        public AudioClip m_Sound; 
        [HideInInspector]public float Speed;
        

        ProjectileBase m_ProjectileBase;
        
        [HideInInspector] public Vector3 m_Velocity;

        [HideInInspector] public float Damage;
        private DamageArea AreaOfDamage;

        const QueryTriggerInteraction k_TriggerInteraction = QueryTriggerInteraction.Collide;

        private void Awake()
        {
            m_ProjectileBase = GetComponent<ProjectileBase>();
            AreaOfDamage = GetComponent<DamageArea>();
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
        }
        IEnumerator DecreaseMaxLifeTime()
        {
            yield return Yielders.Get(MaxLifeTime);
            gameObject.SetActive(false);
        }


        void IsShooting(Transform target)
        {
            Speed = m_ProjectileBase.InitialSpeed;
            Damage = m_ProjectileBase.InitialDamage;

            if (m_Sound)
            {
                SoundManager.Instance.PlayFxSound(m_Sound);

            }
            m_Velocity = gameObject.transform.forward * Speed;
            StartCoroutine(DecreaseMaxLifeTime());
        }

        void OnHit(Vector3 point, Vector3 normal, Damageable damageable)
        {
            // damage
            if (AreaOfDamage)
            {
                // area damage
                AreaOfDamage.InflictDamageInArea(Damage, point, damageLayer, k_TriggerInteraction, m_ProjectileBase.Owner);
            }
            else
            {
                if (damageable)
                {
                    damageable.InflictDamage(Damage, false, m_ProjectileBase.Owner);
                }
                
            }
            // impact vfx
            if (ImpactVfx)
            {
                GameObject impactVfxInstance = PoolManager.Instance.ReuseObject(ImpactVfx, point + (normal * ImpactVfxSpawnOffset), ImpactVfx.transform.rotation);
                impactVfxInstance.SetActive(true);
            }
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (HittableLayers == (HittableLayers | (1 << other.gameObject.layer)))
            {
                if (other.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    if (damageable.enabled)
                    {
                        OnHit(transform.position, -transform.forward, damageable);
                        if (knockBack)
                        {
                            Vector3 knockbackDirection = (-transform.position + other.transform.position).normalized;
                            knockbackDirection.y = 0;
                            if (other.TryGetComponent<Rigidbody>(out Rigidbody targetBody))
                            {
                                targetBody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                            }
                        }
                    }
                }
                else OnHit(transform.position, -transform.forward, null);
            }

        }
    }
}

