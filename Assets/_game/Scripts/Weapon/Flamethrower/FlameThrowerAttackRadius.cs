using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unicorn
{
    [RequireComponent(typeof(Collider))]
    [DisallowMultipleComponent]
    public class FlameThrowerAttackRadius : MonoBehaviour
    {
        public UnityAction<Transform> OnEnemyEnter;
        public UnityAction<Transform> OnEnemyExit;

        public LayerMask HittableLayers = -1; 

        private List<Transform> EnemiesInRadius = new List<Transform>();
        
        private void OnDisable()
        {
            foreach (Transform enemy in EnemiesInRadius)
            {
                OnEnemyExit?.Invoke(enemy);
            }
            EnemiesInRadius.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (HittableLayers == (HittableLayers | (1 << other.gameObject.layer)))
            {
                EnemiesInRadius.Add(other.transform);
                OnEnemyEnter?.Invoke(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (HittableLayers == (HittableLayers | (1 << other.gameObject.layer)))
            {
                EnemiesInRadius.Remove(other.transform);
                OnEnemyExit?.Invoke(other.transform);
            }
        }

    }
}

