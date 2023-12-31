using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class ParticalSystemController : MonoBehaviour
    {
        private ParticleSystem m_particleSystem;

        private void Awake()
        {
            m_particleSystem = GetComponent<ParticleSystem>();
            m_particleSystem.Stop();
        }

        private void OnEnable()
        {
            if (m_particleSystem != null)
            {
                m_particleSystem.Play();
                Debug.Log("play");
            }
        }

    }
}
