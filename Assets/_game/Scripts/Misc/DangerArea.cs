using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class DangerArea : MonoBehaviour
    {
        [Range(0,1)]public float dangerLevel = 1.0f; // Adjust this value to set the danger level of this area

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == PlayerStateMachine.Instance.gameObject)
            {
                // Kill the player
                InvokeRepeating("KillPlayer", 0, 0.5f);
            }
        }

        private void KillPlayer()
        {
            PlayerStateMachine.Instance.m_health.TakeDamage(dangerLevel * PlayerStateMachine.Instance.m_health.CurrentHealth, gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == PlayerStateMachine.Instance.gameObject)
            {
                CancelInvoke("KillPlayer");
            }
        }
    }
}
