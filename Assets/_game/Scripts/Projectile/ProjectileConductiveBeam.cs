using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;
using Sirenix.OdinInspector;

namespace Unicorn
{
    public class ProjectileConductiveBeam : ProjectileBase
    {
        [FoldoutGroup("Parameter")] public GameObject ImpactVfx;
        [FoldoutGroup("Parameter")] public float ImpactVfxLifetime = 5f;
        [FoldoutGroup("Parameter")] public float ImpactVfxSpawnOffset = 0.1f;
        [FoldoutGroup("Parameter")] public LayerMask HittableLayers = -1;


        private float Speed;
        ProjectileBase m_ProjectileBase;
        Transform m_Target;
        public float MaxLifeTime;

        private float Damage;

        private void Awake()
        {
            m_ProjectileBase = GetComponent<ProjectileBase>();
        }


        private void OnEnable()
        {
            m_ProjectileBase.OnShoot += IsShooting;
            StartCoroutine(DecreaseMaxLifeTime());
        }
        private void OnDisable()
        {
            StopAllCoroutines();
            m_ProjectileBase.OnShoot -= IsShooting;

        }
        private void Update()
        {
            if (m_Target)
            {
                // Xác định hướng vector tới mục tiêu
                Vector3 targetDirection = m_Target.position - transform.position;
                targetDirection.y = 0f; // Đặt y = 0 để chỉ xoay trong mặt phẳng ngang

                // Xoay viên đạn theo hướng vector tới mục tiêu
                transform.rotation = Quaternion.LookRotation(targetDirection);

                transform.position = Vector3.Slerp(transform.position, m_Target.position + new Vector3(0, 2, 0), Speed * Time.deltaTime);
            }
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
            m_Target = target;


        }
        void OnHit(Conductable target)
        {
            target.InflictElectricDamage(Damage,5 ,Owner);
            if (ImpactVfx)
            {

                GameObject impactVfxInstance = PoolManager.Instance.ReuseObject(ImpactVfx, target.transform.position, Quaternion.identity);
                impactVfxInstance.SetActive(true);
                StartCoroutine(ImpactLifeTime(impactVfxInstance));
            }
            gameObject.SetActive(false);
            
        }
        IEnumerator ImpactLifeTime(GameObject impactObj)
        {
            yield return Yielders.Get(1.5f);
            impactObj.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (other.TryGetComponent<Conductable>(out Conductable conductable))
                {
                    OnHit(conductable);
                }
            }
        }
    }
}
