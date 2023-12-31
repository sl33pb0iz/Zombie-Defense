using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using UnityEngine.Events;

public class ProjectileFlame : ProjectileBase
{

    public LayerMask HittableLayers = -1;

    [Space]
    public int BurningDPS = 5;
    public float BurnDuration;

    ProjectileBase m_ProjectileBase;

    private void Awake()
    {
        m_ProjectileBase = GetComponent<ProjectileBase>();    
    }

    private void OnEnable()
    {
        m_ProjectileBase.OnShoot += IsShooting;
    }
    private void OnDisable()
    {
        m_ProjectileBase.OnShoot -= IsShooting;
    }

    private void IsShooting(Transform target)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (HittableLayers == (HittableLayers | (1 << other.gameObject.layer)))
        {
            if(other.TryGetComponent<Burnable>(out Burnable burnable))
            {
                burnable.BurnDuration = BurnDuration;
                burnable.InflictStartBurning(BurningDPS, Owner);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (HittableLayers == (HittableLayers | (1 << other.gameObject.layer)))
        {
            if (other.TryGetComponent<Burnable>(out Burnable burnable))
            {
                StartCoroutine(burnable.StopBurning());
            }
        }
    }

}
