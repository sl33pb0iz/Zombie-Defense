using System.Collections.Generic;
using UnityEngine;
using Spicyy.System;

namespace Unicorn
{
    public class DamageArea : MonoBehaviour
    {
        [Tooltip("Area of damage when the projectile hits something")]
        public float AreaOfEffectDistance = 5f;

        [Header("Debug")]
        [Tooltip("Color of the area of effect radius")]
        public Color AreaOfEffectColor = Color.red * 0.5f;

        public AudioClip m_Sound; 

        public LayerMask playerLayer;

        public void InflictDamageInArea(float damage, Vector3 center, LayerMask layers,
            QueryTriggerInteraction interaction, GameObject owner)
        {
            Dictionary<Health, Damageable> uniqueDamagedHealths = new Dictionary<Health, Damageable>();

            // Create a collection of unique health components that would be damaged in the area of effect (in order to avoid damaging a same entity multiple times)
            Collider[] affectedColliders = Physics.OverlapSphere(center, AreaOfEffectDistance, layers, interaction);
            foreach (var coll in affectedColliders)
            {
                Damageable damageable = coll.GetComponent<Damageable>();
                if (damageable)
                {
                    Health health = damageable.GetComponentInParent<Health>();
                    if (health && !uniqueDamagedHealths.ContainsKey(health))
                    {
                        uniqueDamagedHealths.Add(health, damageable);
                    }
                }
            }

            if (Physics.CheckSphere(center, AreaOfEffectDistance, playerLayer))
            {
                EventManager.Broadcast(Events.PlayerVibratedEvent);
            }

            // Apply damages with distance falloff
            foreach (Damageable uniqueDamageable in uniqueDamagedHealths.Values)
            {
                float distance = Vector3.Distance(uniqueDamageable.transform.position, transform.position);
                uniqueDamageable.InflictDamage(
                    damage * (distance / AreaOfEffectDistance), true, owner);
            }

            if (m_Sound)
            {
                SoundManager.Instance.PlayFxSound(m_Sound);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = AreaOfEffectColor;
            Gizmos.DrawSphere(transform.position, AreaOfEffectDistance);
        }
    }
}