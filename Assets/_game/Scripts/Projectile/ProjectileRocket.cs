using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using DG.Tweening;
using Sirenix.OdinInspector;

public class ProjectileRocket : ProjectileBase
{
    [FoldoutGroup("General")] public float MaxLifeTime = 5f;
    [FoldoutGroup("General")] public GameObject ImpactVfx;
    [FoldoutGroup("General")] public float ImpactVfxLifetime = 5f;
    [FoldoutGroup("General")] public float ImpactVfxSpawnOffset = 0.1f;
    [FoldoutGroup("General")] public LayerMask HittableLayers = -1;
    [FoldoutGroup("General")] public LayerMask damageLayers = -1;
    [FoldoutGroup("General")] public float smoothTimeFactor = 1f;
    private float m_Gravity = 9.81f;

    [FoldoutGroup("Set Up")] public DamageArea AreaOfDamage;
    [FoldoutGroup("Set Up")] public AudioClip m_Sound;
    ProjectileBase m_ProjectileBase;

    const QueryTriggerInteraction k_TriggerInteraction = QueryTriggerInteraction.Collide;

    [HideInInspector] public float Damage;

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
        m_ProjectileBase.OnShoot -= IsShooting;
        StopAllCoroutines();
    }
    float m_InitialAngle;


    IEnumerator SimulateProjectile(Transform target)
    {
        //float distance = Vector3.Distance(transform.position, target.position);
        var delta = target.position - transform.position;
        float distance = new Vector2(delta.x, delta.z).magnitude;

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectileVelocity = distance / (Mathf.Sin(2 * m_InitialAngle * Mathf.Deg2Rad) / m_Gravity);

        // Extract the X and Y components of the velocity
        float Vxz = Mathf.Sqrt(projectileVelocity) * Mathf.Cos(m_InitialAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectileVelocity) * Mathf.Sin(m_InitialAngle * Mathf.Deg2Rad);

        // Calculate the angle between the target position and the forward direction of the rocket
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float angleToTarget = Mathf.Atan2(directionToTarget.z, directionToTarget.x);
        angleToTarget *= Mathf.Rad2Deg;

        // Extract the X and Z components of the velocity based on the angle
        float Vz = Vxz * Mathf.Sin(angleToTarget * Mathf.Deg2Rad);
        float Vx = Vxz * Mathf.Cos(angleToTarget * Mathf.Deg2Rad);

        float flightDuration = (distance / Mathf.Sqrt(Vx * Vx + Vz * Vz)) + ((Vy + Mathf.Sqrt((Vy * Vy) + (2 * m_Gravity * (transform.position.y - target.position.y)))) / m_Gravity);

        // Calculate smooth time based on distance and velocity
        float smoothTime = distance / (projectileVelocity * smoothTimeFactor);

        float elapsedTime = 0;
        Vector3 currentVelocity = Vector3.zero;

        while (elapsedTime < flightDuration)
        {
            Vector3 nextPosition = transform.position + new Vector3(Vx * Time.deltaTime, (Vy - (m_Gravity * elapsedTime)) * Time.deltaTime, Vz * Time.deltaTime);
            transform.position = Vector3.SmoothDamp(transform.position, nextPosition, ref currentVelocity, smoothTime);

            Vector3 lookDirection = (nextPosition - transform.position).normalized;
            Quaternion lookAt = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAt, Time.deltaTime * projectileVelocity);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target.position;
    }


    void IsShooting(Transform target)
    {
        m_Gravity = m_ProjectileBase.InitialSpeed;
        Damage = m_ProjectileBase.InitialDamage;
        m_InitialAngle = 60;
        StartCoroutine(SimulateProjectile(target));
        if (m_Sound)
        {
            SoundManager.Instance.PlayFxSound(m_Sound);

        }
    }
    IEnumerator DecreaseMaxLifeTime()
    {
        yield return Yielders.Get(MaxLifeTime);
        gameObject.SetActive(false);
    }

    void OnHit(Vector3 point, Vector3 normal, Collider collider)
    {
        // damage
        if (AreaOfDamage)
        {
            // area damage
            AreaOfDamage.InflictDamageInArea(Damage, point, damageLayers, k_TriggerInteraction, m_ProjectileBase.Owner);
        }
        else
        {
            // point damage
            Damageable damageable = collider.GetComponent<Damageable>();
            if (damageable)
            {
                damageable.InflictDamage(Damage, false, m_ProjectileBase.Owner);
            }
        }
        // impact vfx
        if (ImpactVfx)
        {
            GameObject impactVfxInstance = PoolManager.Instance.ReuseObject(ImpactVfx, point + (normal * ImpactVfxSpawnOffset), Quaternion.identity);
            impactVfxInstance.SetActive(true);
            StartCoroutine(ImpactLifeTime());
            IEnumerator ImpactLifeTime()
            {
                yield return Yielders.Get(1.5f);
                impactVfxInstance.SetActive(false);
            }
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HittableLayers == (HittableLayers | (1 << other.gameObject.layer)))
        {
            OnHit(transform.position, -transform.forward, other);
        }

    }
}
