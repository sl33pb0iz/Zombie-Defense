using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;

public class Lightning : MonoBehaviour
{
    public LayerMask damageLayer;
    public float damageAmount = 10f;
    public AudioClip m_Sound; 

    private ParticleSystem particles;
    

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        particles.Clear();
        particles.Play();
    }


    private void OnTriggerEnter(Collider other)
    {
        if ((damageLayer.value & 1 << other.gameObject.layer) != 0)
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.InflictDamage(damageAmount, false, null);
            }
        }
    }
}
