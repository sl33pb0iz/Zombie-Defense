using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;
using Sirenix.OdinInspector; 

namespace Spicyy.AI
{
    [RequireComponent(typeof(CharacterController), typeof(DetectionModule))]
    public class AIDrone : FlyingAI, IAlly
    {
        [Title("PAREMETER", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private float minRestTime = 1;
        [SerializeField] private float maxRestTime = 3;
        [Range(0, 1)] [SerializeField] private float actionRatio = 0.3f;
        [SerializeField] private float speed = 10;
        [SerializeField] private float rotationSpeed = 360;
        [SerializeField] private float squaredStoppingDistance = 0.3f;
        [SerializeField] private float randomRadius = 25;
        [SerializeField] private float maxDistanceFromOwner;

        private Vector3 target;
        private Quaternion targetRotation;
        private GameObject owner; 

        private float nextMoveTime;


        private CharacterController controller;
        private DetectionModule m_DetectionModule; 

        private float m_TimeStartedDetection;
        private float DetectionFireDelay = 1f;

        private enum AIState
        {
            Patrol,
            Attack,
        }
        private AIState aiState;

        #region Luồng
        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            m_DetectionModule = GetComponent<DetectionModule>();
            target = transform.position;
        }

        private void Start()
        {
            var renderer = GetComponentInChildren<Renderer>();
            controller.center = renderer.bounds.size.y / 2 * Vector3.up;
            controller.height = renderer.bounds.size.y;
            if (controller.stepOffset > controller.height)
            {
                controller.stepOffset = controller.height * transform.lossyScale.magnitude;
            }
            RegisterOwner(PlayerStateMachine.Instance.gameObject);
        }

        private void OnEnable()
        {
            m_DetectionModule.onDetectedTarget += () => aiState = AIState.Attack;
            m_DetectionModule.onLostTarget += () => aiState = AIState.Patrol;
        }

        private void OnDisable()
        {

        }

        private void Update()
        {
            m_DetectionModule.HandleTargetDetection();
            UpdateState();
        }

        protected override void Init()
        {
            aiState = AIState.Patrol;
        }
        public void RegisterAlly()
        {
            throw new global::System.NotImplementedException();
        }

        public void UnRegisterAlly()
        {
            throw new global::System.NotImplementedException();
        }

        protected override void UpdateState()
        {
            switch (aiState)
            {
                case AIState.Patrol:
                    Fly();
                    break;
                case AIState.Attack:
                    Attack();
                    break;
            }
        }
        #endregion

        #region Hành động
        protected override void Fly()
        {
            float sqrTargetDistanceToOwner = owner ? (target - owner.transform.position).sqrMagnitude : 0;

            if (owner && sqrTargetDistanceToOwner >= maxDistanceFromOwner * maxDistanceFromOwner)
            {
                FindNewPositionAroundOwner();
                nextMoveTime = Time.time;
            }

            UpdateSpeed();
            if (nextMoveTime > Time.time)
            {
                return;
            }

            var distance = (target - transform.position.Set(y: target.y)).sqrMagnitude;
            if (distance < squaredStoppingDistance)
            {
                FindNewPositionAroundOwner();
                return;
            }

            MoveToTarget();
        }

        private void Attack()
        {
            
        }

        public void RegisterOwner(GameObject Owner)
        {
            owner = Owner;
        }

        private void UpdateSpeed()
        {
            if (!owner)
            {
                speed = 10;
                return;
            }

            float sqrDistanceToOwner = (transform.position - owner.transform.position).sqrMagnitude;
            speed = Mathf.Min(2 + sqrDistanceToOwner / 5f, 20);
        }

        private void FindNewPositionAroundOwner()
        {
            nextMoveTime = Time.time + Random.Range(minRestTime, maxRestTime);

            var direction = Random.insideUnitCircle.ToVectorXZ().normalized;
            direction *= Random.Range(0, 2) == 0 ? -1 : 1;

            float newHeight = owner.transform.position.y + Random.Range(5f, 10f); // Thay đổi khoảng cách tùy ý
            target = new Vector3(owner.transform.position.x + direction.x * randomRadius, newHeight, owner.transform.position.z + direction.y * randomRadius);

            Vector3 directionRotation = target - transform.position;
            directionRotation.y = 0; 
            targetRotation = Quaternion.LookRotation(directionRotation);
        }

        private void MoveToTarget()
        {
            var newPosition = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            var motion = newPosition - transform.position;

            motion.y = 0;
            controller.Move(motion);

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

       

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.point.y < transform.position.y) return;
            target = transform.position;
        }

        #endregion
    }

}
