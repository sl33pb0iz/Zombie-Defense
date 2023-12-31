using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Unicorn
{
    public class SkillController : MonoBehaviour
    {
        public enum SkillType
        {
            Stamp,
            Gore,
            Slash,
            SelfExploding,
            DeathZone,
            PollutionArea,
            Storm,
        }

        public bool CanStamp;
        [ShowIfGroup("CanStamp")]
        [BoxGroup("CanStamp/Stamp")]
        public StampSkill stampSkill;

        public bool CanGore;
        [ShowIfGroup("CanGore")]
        [BoxGroup("CanGore/Gore")]
        public GoreSkill goreSkill;

        public bool CanSlash;
        [ShowIfGroup("CanSlash")]
        [BoxGroup("CanSlash/Slash")]
        public SlashSkill jumpingSkill;

        public bool CanSelfExploding;
        [ShowIfGroup("CanSelfExploding")]
        [BoxGroup("CanSelfExploding/Self Exploding")]
        public SelfExplodingSkill explodingSkill;

        public bool CanDeathZone;
        [ShowIfGroup("CanDeathZone")]
        [BoxGroup("CanDeathZone/Death Zone")]
        public DeathZoneSkill deathZoneSkill;

        public bool CanPollutionArea;
        [ShowIfGroup("CanPollutionArea")]
        [BoxGroup("CanPollutionArea/Pollution Area")]
        public DeathZoneSkill pollutionAreaSkill;

        public bool CanCreateStorm;
        [ShowIfGroup("CanCreateStorm")]
        [BoxGroup("CanCreateStorm/Storm")]
        public StormSkill stormSkill;

        public void CastSkill(SkillType skillType)
        {
            switch (skillType)
            {
                case SkillType.Stamp:
                    break;
                case SkillType.Gore:
                    break;
                case SkillType.Slash:
                    break;
                case SkillType.SelfExploding:
                    SelfExplodingCast();
                    break;
                case SkillType.DeathZone:
                    DeathZoneCast();
                    break;
                case SkillType.PollutionArea:
                    break;
                case SkillType.Storm:
                    StormCast();
                    break;
                default:
                    Debug.LogError("no skill valid");
                    break;
            }
        }

        public void DeathZoneCast()
        {
            if (CanDeathZone)
            {
                Collider[] affectedColliders = Physics.OverlapSphere(transform.position, deathZoneSkill._range, deathZoneSkill._damageLayer, QueryTriggerInteraction.Collide);
                foreach (var coll in affectedColliders)
                {
                    Damageable damageable = coll.GetComponent<Damageable>();
                    if (damageable)
                    {
                        damageable.InflictDamage(deathZoneSkill._damage, false, gameObject);
                    }
                }
            }
        }
        public void SelfExplodingCast()
        {
            if (CanSelfExploding)
            {
                explodingSkill.m_DamageArea.InflictDamageInArea(explodingSkill._damage, transform.position, explodingSkill._damageLayer
                                                            , QueryTriggerInteraction.Collide, transform.gameObject);
                GameObject impactVfxInstance = PoolManager.Instance.ReuseObject(explodingSkill._impactVfx, transform.position, Quaternion.identity);
                impactVfxInstance.SetActive(true);
                StartCoroutine(ImpactLifeTime());
                IEnumerator ImpactLifeTime()
                {
                    yield return new WaitForSeconds(1.5f);
                    impactVfxInstance.SetActive(false);
                }
            }
        }
        public void StormCast()
        {

        }
    }

    [System.Serializable]
    public struct StampSkill
    {
    }

    [System.Serializable]
    public struct GoreSkill
    {
    }

    [System.Serializable]
    public struct SlashSkill
    {
    }

    [System.Serializable]
    public struct SelfExplodingSkill
    {
        public float _damage;
        public GameObject _impactVfx;
        public DamageArea m_DamageArea;
        public LayerMask _damageLayer;
    }

    [System.Serializable]
    public struct DeathZoneSkill
    {
        [Range(1, 3)] public float _damage;
        public float _range;
        public LayerMask _damageLayer;
    }

    [System.Serializable]
    public struct PollutionAreaSkill
    {
        [Range(1, 3)] public float _damage;
        [Range(0, 2)] public float _pollutedAfterTime;
        public float _range;
        public LayerMask _damageLayer;
        public float _timeExist;
        public GameObject _ImpactVFX;
        public DamageArea m_DamageArea;
    }

    [System.Serializable]
    public struct StormSkill
    {
        public GameObject stormFX;
        public float duration;
        public float minRadius;
        public float maxRadius;
        public float boltsPerCircle;
    }


}
