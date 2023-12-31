using System.Collections;
using UnityEngine;

namespace Unicorn
{
    public class Burnable : MonoBehaviour
    {

        public bool isBurning; 
        public ParticleSystem OnFire;
        private Coroutine BurnCoroutine;
        public Health Health{ get; private set; }
        public float BurnDuration { get; set;}
        
        void Awake()
        {
            // find the health component either at the same level, or higher in the hierarchy
            Health = GetComponent<Health>();
            if (!Health)
            {
                Health = GetComponentInParent<Health>();
            }
        }

        private void OnDisable()
        {
            OnFire.Clear();
            OnFire.Stop();
            StopAllCoroutines();
        }

        public void InflictStartBurning(float damagePerSecond, GameObject damageSource)
        {
            if (Health)
            {
                isBurning = true;
                if (BurnCoroutine != null)
                {
                    StopCoroutine(BurnCoroutine);
                }
                BurnCoroutine = StartCoroutine(Burn(damagePerSecond, damageSource));
            }
        }
        private IEnumerator Burn(float DamagePerSecond, GameObject damageSource)
        {
            //WaitForSeconds wait = new WaitForSeconds(1);
            Health.TakeDamage(DamagePerSecond, damageSource);
            if (OnFire.isStopped)
            {
                OnFire.Clear();
                OnFire.Play();
            }
            while (isBurning)
            {
                yield return Yielders.Get(1);
                Health.TakeDamage(DamagePerSecond, damageSource);
            }
        }


        public IEnumerator StopBurning()
        {
            yield return new WaitForSeconds(BurnDuration);
            isBurning = false;
            if (BurnCoroutine != null)
            {
                if (OnFire.isPlaying)
                {
                    OnFire.Clear();
                    OnFire.Stop();
                }
                StopCoroutine(BurnCoroutine);
            }
        }


    }
}

