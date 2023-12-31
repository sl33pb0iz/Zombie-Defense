using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;

public class FreezeController : MonoBehaviour
{
    public LayerMask damageLayer;
    public float damageAmount = 10f;

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
            Frozenable frozenable = other.GetComponent<Frozenable>();
            if (frozenable != null)
            {
                frozenable.InflictStartFrozening(damageAmount, 10, null);
            }
        }
    }
}
