using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using Sirenix.OdinInspector;
using DigitalRuby.LightningBolt;

public class Conductable : MonoBehaviour
{
    [FoldoutGroup("Parameter")] public Damageable m_Damageable;

    [FoldoutGroup("Lightning Effect")] public LightningBoltScript m_Lightning;
    [FoldoutGroup("Lightning Effect")] public LineRenderer m_LightningLine;
    [FoldoutGroup("Parameter")] public AudioClip m_Sound;
    [FoldoutGroup("Parameter")] public ParticleSystem m_Partical;

    public void Awake()
    {
        m_Damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        m_Lightning.StartObject = gameObject;
        m_LightningLine.enabled = false;
    }

    public void InflictElectricDamage(float damage,int conductiveTime ,GameObject owner)
    {
        if (m_Damageable.enabled)
        {
            m_Damageable.InflictDamage(damage, false, owner);
        }
        if (m_Sound)
        {
            SoundManager.Instance.PlayFxSound(m_Sound);
        }
        m_Partical.Clear();
        m_Partical.Play();
        ConductElectricDamage(damage,conductiveTime ,owner);
    }

    private void ConductElectricDamage(float electricDamage,int conductiveTime ,GameObject owner)
    {
        conductiveTime--;
        if(conductiveTime > 0)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 20, LayerMask.GetMask("Enemy"));

            Conductable closestConductable = null;
            float closestDistanceSqr = float.MaxValue;
            Vector3 currentPosition = transform.position;

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    if (collider.TryGetComponent<Conductable>(out Conductable conductable))
                    {
                        if (conductable)
                        {
                            Vector3 directionToTarget = collider.transform.position - currentPosition;
                            float sqrDistanceToTarget = directionToTarget.sqrMagnitude;

                            if (sqrDistanceToTarget < closestDistanceSqr)
                            {
                                closestConductable = conductable;
                                closestDistanceSqr = sqrDistanceToTarget;
                            }
                        }
                    }
                }
            }

            if (closestConductable == null || !closestConductable.gameObject.activeSelf)
            {
                return;
            }

            closestConductable.InflictElectricDamage(electricDamage, conductiveTime, owner);
            m_Lightning.EndObject = closestConductable.gameObject;
            m_LightningLine.enabled = true;

            StartCoroutine(DeactiveFX());

            IEnumerator DeactiveFX()
            {
                yield return Yielders.Get(0.5f);
                m_LightningLine.enabled = false;
            }
        }
        
    }
}
