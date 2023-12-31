using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unicorn;

public class Tornado : MonoBehaviour
{
    public float timeExist;
    public float suctionForce;

    public float damage;
    public float damageInterval;

    public LayerMask damageLayer;

    private readonly List<Rigidbody> suctionObj = new List<Rigidbody>();
    private readonly List<Damageable> damageables = new List<Damageable>();

    private float timer;

    void OnEnable()
    {
        timer = 0;
        StartCoroutine(DamageOverTime());
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeExist)
        {
            gameObject.SetActive(false);
        }

        if (suctionObj.Count > 0)
        {
            foreach (var obj in suctionObj)
            {
                if (obj.gameObject.activeSelf)
                {
                    Vector3 directionToTornado = transform.position - obj.transform.position;
                    obj.AddForce(directionToTornado.normalized * suctionForce, ForceMode.Force);
                }
            }
        }
    }

    IEnumerator DamageOverTime()
    {
        while (true)
        {
            if (damageables.Count > 0)
            {
                foreach (var damageable in damageables)
                {
                    if (damageable.gameObject.activeSelf)
                    {
                        damageable.InflictDamage(damage, false, gameObject);
                    }
                }
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damageLayer == (damageLayer | (1 << other.gameObject.layer)))
        {
            if (other.TryGetComponent<Rigidbody>(out Rigidbody rigid))
            {
                suctionObj.Add(rigid);
            }
            if (other.TryGetComponent<Damageable>(out Damageable damageable))
            {
                damageables.Add(damageable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (damageLayer == (damageLayer | (1 << other.gameObject.layer)))
        {
            if (other.TryGetComponent<Rigidbody>(out Rigidbody rigid))
            {
                suctionObj.Remove(rigid);
            }
            if (other.TryGetComponent<Damageable>(out Damageable damageable))
            {
                damageables.Remove(damageable);
            }
        }
    }
}
