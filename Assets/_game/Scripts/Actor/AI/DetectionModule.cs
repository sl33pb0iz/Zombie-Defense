using System.Collections.Generic;
using System.Linq;
using Unicorn;
using UnityEngine;
using UnityEngine.Events;

namespace Spicyy.AI
{
    public class DetectionModule : MonoBehaviour
    {
        [Tooltip("Layer that object can detected")]
        public LayerMask HittableLayer = -1;

        [Tooltip("The point representing the source of target-detection raycasts for the enemy AI")]
        public Transform DetectionSourcePoint;

        [Tooltip("The max distance at which the enemy can see targets")]
        public float DetectionRange = 20f;

        [Tooltip("The max distance at which the enemy can attack its target")]
        public float AttackRange;

        [Tooltip("Time before an enemy abandons a known target that it can't see anymore")]
        public float KnownTargetTimeout = 4f;

        [Header("Debug")]
        [Tooltip("Color of the area of effect radius")]
        public Color AreaOfDetectedColor = Color.red * 0.5f;

        public UnityAction onDetectedTarget;
        public UnityAction onLostTarget;
        public UnityAction onAttack;

        public GameObject KnownDetectedTarget { get; private set; }
        public bool IsTargetInAttackRange { get; private set; }
        public bool IsSeeingTarget { get; private set; }
        public bool HadKnownTarget { get; private set; }

        protected float TimeLastSeenTarget = float.MaxValue;
        private float sqrDetectionRange;
        private List<Damageable> actorInRange = new List<Damageable>();
        private SphereCollider m_Collider;

        protected virtual void Awake()
        {
            m_Collider = GetComponent<SphereCollider>();
        }
        private void Start()
        {
            sqrDetectionRange = DetectionRange * DetectionRange;
        }
        public virtual void HandleTargetDetection()
        {
            // Handle known target detection timeout
            if (KnownDetectedTarget && !IsSeeingTarget && (Time.time - TimeLastSeenTarget) > KnownTargetTimeout)
            {
                KnownDetectedTarget = null;
            }
            FindClosestOtherActor();
            float distanceToTarget = KnownDetectedTarget != null ? Vector3.Distance(transform.position, KnownDetectedTarget.transform.position) : float.MaxValue;
            IsTargetInAttackRange = KnownDetectedTarget != null && distanceToTarget <= AttackRange;

            // Detection events
            if (!HadKnownTarget && KnownDetectedTarget != null)
            {
                OnDetect();
            }
            else if (HadKnownTarget && KnownDetectedTarget == null)
            {
                OnLostTarget();
            }
            else if (HadKnownTarget && IsTargetInAttackRange)
            {
                OnAttack();
            }
            // Remember if we already knew a target (for next frame)
            HadKnownTarget = KnownDetectedTarget != null;
        }
        private void FindClosestOtherActor()
        {
            IsSeeingTarget = false;
            GameObject closestActor = null;
            actorInRange.RemoveAll(actor => !actor.gameObject.activeSelf);
            actorInRange.RemoveAll(actor => !actor.enabled);
            if (actorInRange.Count > 0)
            {
                float closestSqrDistance = float.MaxValue;
                for (int i = 0; i < actorInRange.Count; i++)
                {
                    float sqrDistance = (actorInRange[i].transform.position - transform.position).sqrMagnitude;

                    if (sqrDistance < sqrDetectionRange && sqrDistance < closestSqrDistance)
                    {
                        closestActor = actorInRange[i].gameObject;
                        closestSqrDistance = sqrDistance;
                    }
                }
            }
            if (closestActor != null)
            {
                IsSeeingTarget = true;
                TimeLastSeenTarget = Time.time;
                KnownDetectedTarget = closestActor;
                return;
            }
            else
            {
                IsSeeingTarget = false;
                KnownDetectedTarget = null;
            }
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = AreaOfDetectedColor;
            Gizmos.DrawSphere(transform.position, DetectionRange);
        }
        public virtual void OnLostTarget() => onLostTarget?.Invoke();
        public virtual void OnDetect() => onDetectedTarget?.Invoke();
        public virtual void OnAttack() =>  onAttack?.Invoke(); 

        private void OnTriggerEnter(Collider other)
        {
            if ((HittableLayer & (1 << other.gameObject.layer)) != 0)
            {
                if(other.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    if (!actorInRange.Contains(damageable) && damageable.enabled)
                    {
                        actorInRange.Add(damageable);
                    }
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if ((HittableLayer & (1 << other.gameObject.layer)) != 0)
            {
                if (other.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    if (actorInRange.Contains(damageable))
                    {
                        actorInRange.Remove(damageable);
                    }
                }
            }
        }
    }
}