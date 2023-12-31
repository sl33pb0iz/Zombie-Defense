using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System.Collections;

namespace Unicorn
{
    public class Damageable : MonoBehaviour
    {
        [Tooltip("Multiplier to apply to the received damage")]
        public float DamageMultiplier = 1f;

        [Range(0, 1)]
        [Tooltip("Multiplier to apply to self damage")]
        public float SensibilityToSelfdamage = 0.5f;

        public bool isBlink;
        [ShowIfGroup("isBlink")]
        
        [BoxGroup("isBlink/Blink")]
        public float blink;

        [BoxGroup("isBlink/Blink")]
        public float immuned;

        [BoxGroup("isBlink/Blink")]
        [Tooltip("Time delay to next take damage if character take damage from damage percent health")]
        public float delayEachDamage;

        [BoxGroup("isBlink/Blink")]
        public List<Renderer> modelRenderer;

        private float blinkTime = 0.1f;
        private float immunedTime;
        private float delayTimeToNextDamage;

        public Health Health
        { get; private set; }

        void Awake()
        {
            // find the health component either at the same level, or higher in the hierarchy
            Health = GetComponent<Health>();
            if (!Health)
            {
                Health = GetComponentInParent<Health>();
            }
        }

        private void Start()
        {
            delayTimeToNextDamage = 0;
        }

        private void Update()
        {
            if(delayTimeToNextDamage > 0)
            {
                delayTimeToNextDamage -= Time.deltaTime; 
            }

            if (isBlink)
            {
                if (immunedTime > 0)
                {
                    immunedTime -= Time.deltaTime;

                    blinkTime -= Time.deltaTime;

                    if (blinkTime <= 0)
                    {
                        foreach (var renderer in modelRenderer)
                        {
                            renderer.enabled = !renderer.enabled;
                        }

                        blinkTime = blink;
                    }
                    if (immunedTime <= 0)
                    {
                        foreach (var renderer in modelRenderer)
                        {
                            renderer.enabled = true;
                        }
                    }
                }
            }
        }

        public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
        {
            if (Health)
            {
                var totalDamage = damage;
                // skip the crit multiplier if it's from an explosion
                if (!isExplosionDamage)
                {
                    totalDamage *= DamageMultiplier;
                }
                // potentially reduce damages if inflicted by self
                if (Health.gameObject == damageSource)
                {
                    totalDamage *= SensibilityToSelfdamage;
                }

                // apply the damages
                Health.TakeDamage(totalDamage, damageSource);      

                if(damageSource == PlayerStateMachine.Instance.gameObject)
                {
                    VibrationManager.instance.VibratePop();
                }

                if (isBlink)
                {
                    if (immunedTime <= 0)
                    {
                        immunedTime = immuned;
                        foreach (var renderer in modelRenderer)
                        {
                            renderer.enabled = false;
                        }
                        blinkTime = blink;
                    }
                }
            }
        }

        public void InflictPercentHealthDamage(float percentage, bool isExplosionDamage, GameObject damageSource)
        {
            if (Health)
            {
                if(delayTimeToNextDamage <= 0)
                {
                    var totalDamage = Health.maxHealth * percentage;
                    // apply the damages
                    if (!isExplosionDamage)
                    {
                        totalDamage *= DamageMultiplier;
                    }
                    Health.TakeDamage(totalDamage, damageSource);
                    delayTimeToNextDamage = delayEachDamage;

                    if (isBlink)
                    {
                        if (immunedTime <= 0)
                        {
                            immunedTime = immuned;
                            foreach (var renderer in modelRenderer)
                            {
                                renderer.enabled = false;
                            }

                            blinkTime = blink;
                        }
                    }
                }
            }
        }
    }
}