using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class PlayerVisionCollideHandler : MonoBehaviour
    {
        public LayerMask AlliesLayer;
        public LayerMask EnemiesLayer;
        public float visionRange;

        public bool NearAllies => allies.Count > 0 ;
        public bool NearEnemies => enemies.Count > 0;

        [HideInInspector] public List<Damageable> enemies = new List<Damageable>();
        [HideInInspector] public List<Damageable> allies = new List<Damageable>();

        private void Update()
        {
            enemies.RemoveAll(enemy => !enemy.enabled);
            allies.RemoveAll(ally => !ally.enabled);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((AlliesLayer & (1 << other.gameObject.layer)) != 0)
            {
                if(other.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    if (!allies.Contains(damageable))
                    {
                        allies.Add(damageable);
                    }
                }
            }

            if ((EnemiesLayer & (1 << other.gameObject.layer)) != 0)
            {
                if (other.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    if (!enemies.Contains(damageable))
                    {
                        enemies.Add(damageable);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((AlliesLayer & (1 << other.gameObject.layer)) != 0)
            {
                if (other.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    if (allies.Contains(damageable))
                    {
                        allies.Remove(damageable);
                    }
                }
            }


            if ((EnemiesLayer & (1 << other.gameObject.layer)) != 0)
            {
                if (other.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    if (enemies.Contains(damageable))
                    {
                        enemies.Remove(damageable);
                    }
                }
            }
        }
    }
}
