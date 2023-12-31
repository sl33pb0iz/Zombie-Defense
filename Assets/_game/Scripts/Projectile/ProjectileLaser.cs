using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn; 

public class ProjectileLaser : ProjectileBase
{
    private LayerMask _hittableLayer;
    private LayerMask _damageLayer; 
    private float _damage;
    private float _timeDelayDamage;
    private float _maxLength;

    public GameObject HitEffect;
    public float HitOffset = 0;
    public AudioClip m_Sound; 

    private LineRenderer Laser;

    public float MainTextureLength = 1f;
    public float NoiseTextureLength = 1f;
    private Vector4 Length = new Vector4(1, 1, 1, 1);

    private bool LaserSaver = false;
    private bool UpdateSaver = false;

    private ParticleSystem[] Effects;
    private ParticleSystem[] Hit;

    private GameObject targetObject;
    private bool canDamage = true; // Flag to track if damage can be inflicted

    public LayerMask HittableLayer {set { _hittableLayer = value; } }
    public LayerMask DamageLayer { set { _damageLayer = value;  } }
    public float Damage { set { _damage = value; } }
    public float TimeDelayDamage { set { _timeDelayDamage = value; } }
    public float MaxLength { set { _maxLength = value; } }

    private void Awake()
    {
        Laser = GetComponent<LineRenderer>();
        Effects = GetComponentsInChildren<ParticleSystem>();
        Hit = HitEffect.GetComponentsInChildren<ParticleSystem>(); 
    }

    private void Start()
    {
        if (Effects != null)
        {
            foreach (var AllPs in Effects)
            {
                AllPs.Clear();
                AllPs.Stop();
            }
        }
    }


    private void OnEnable()
    {
        canDamage = true; 
    }

    public void IsShooting()
    {

    }

    public void FireLaser()
    {
        EnablePrepare(); 
        Laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));
        Laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));
        //To set LineRender position
        if (Laser != null && UpdateSaver == false)
        {
            Laser.SetPosition(0, transform.position);
            RaycastHit hit;      
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _maxLength, _hittableLayer))
            {
                Laser.SetPosition(1, hit.point);
                HitEffect.transform.position = hit.point + hit.normal * HitOffset;
                HitEffect.transform.rotation = Quaternion.identity;
                foreach (var AllPs in Effects)
                {
                    if (AllPs.isStopped) AllPs.Play();
                }
                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, hit.point));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, hit.point));


                // Inflict damage to damageLayer target
                if (_damageLayer != 0)
                {
                    if (canDamage)
                    {
                        targetObject = hit.collider.gameObject;
                        if (_damageLayer == (_damageLayer | (1 << targetObject.layer)))
                        {
                            Damageable damageable = targetObject.GetComponent<Damageable>();
                            if (damageable != null)
                            {
                                damageable.InflictDamage(_damage, false, Owner);
                                StartCoroutine(ResetDamageCooldown());
                            }
                        }
                    }
                }
            }
            else
            {
                var EndPos = transform.position + transform.forward * _maxLength;
                Laser.SetPosition(1, EndPos);
                HitEffect.transform.position = EndPos;
                foreach (var AllPs in Hit)
                {
                    if (AllPs.isPlaying) AllPs.Play();
                }
                //Texture tiling
                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, EndPos));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, EndPos));
            }
            //Insurance against the appearance of a laser in the center of coordinates!
            if (Laser.enabled == false && LaserSaver == false)
            {
                LaserSaver = true;
                Laser.enabled = true;
            }
        }
    }

    public void DisablePrepare()
    {
        if (Laser != null)
        {
            Laser.enabled = false;
        }
        UpdateSaver = true;
        //Effects can = null in multiply shooting
        if (Effects != null)
        {
            foreach (var AllPs in Effects)
            {
                if (AllPs.isPlaying) AllPs.Stop();
            }
        }
    }

    public void EnablePrepare()
    {
        if (Laser != null)
        {
            Laser.enabled = true;
        }
        UpdateSaver = false;
        //Effects can = null in multiply shooting
        if (Effects != null)
        {
            foreach (var AllPs in Effects)
            {
                if (AllPs.isStopped) AllPs.Play();
            }
        }
    }

    private IEnumerator ResetDamageCooldown()
    {
        canDamage = false;
        yield return Yielders.Get(_timeDelayDamage);
        canDamage = true;
    }


}
