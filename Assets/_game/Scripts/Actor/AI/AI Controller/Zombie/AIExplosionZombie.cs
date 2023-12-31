using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;
using Sirenix.OdinInspector;


namespace Spicyy.AI
{
    [RequireComponent(typeof(DamageArea))]
    public class AIExplosionZombie : MobileAI, IEnemy
    {
        [Title("SELF-EXPLOSION", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private Transform BodyTransform;
        [SerializeField] private ParticleSystem ImpactVFX;
        [SerializeField] private float DamageExplosion;
        [SerializeField] private DamageArea DamageArea;
        [SerializeField] private LayerMask DamageLayer;

        private enum AIState
        {
            Invade,
            Die,
            Explosion,
        }
        private AIState aiState;

        private AIManager m_AIManager;
        private Health m_Health;
        private Rigidbody m_Rigidbody;
        private Target m_Target;
        private Collider[] m_Collider;
        private Animator m_Animator;
        private Damageable m_Damagaeble;
        private Burnable m_Burnable;
        private Conductable m_Conductable;
        private DropItemModule m_DropItemModule;

        #region Luồng
        private void Awake()
        {
            m_AIManager = AIManager.Instance;
            m_Health = GetComponent<Health>();
            m_Target = GetComponent<Target>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Collider = GetComponents<Collider>();
            m_Animator = GetComponent<Animator>();
            m_Damagaeble = GetComponent<Damageable>();
            m_Burnable = GetComponent<Burnable>();
            m_Conductable = GetComponent<Conductable>();
            m_DropItemModule = GetComponent<DropItemModule>();
        }

        private void OnEnable()
        {
            Init();

            for (int index = 0; index < m_Collider.Length; index++)
            {
                m_Collider[index].enabled = true;
            }

            RegisterEnemy();
            RegisterAI();

            m_Rigidbody.isKinematic = false;
            m_Target.enabled = true;
            m_Damagaeble.enabled = true;
            m_Burnable.enabled = true;
            m_Conductable.enabled = true;
            m_Health.OnDie += () => { Die(); aiState = AIState.Die; };
        }

        private void OnDisable()
        {
            UnRegisterEnemy();
            UnRegisterAI();

            m_Health.OnDie -= () => { Die(); aiState = AIState.Die; };
        }

        private void Update()
        {
            UpdateState();
        }

        protected override void Init()
        {
            BodyTransform.gameObject.SetActive(true);
            m_Animator.SetBool("Idle", false);
            aiState = AIState.Invade;
        }

        public void RegisterEnemy()
        {
            m_AIManager.RegisterEnemy(gameObject);
        }

        public void UnRegisterEnemy()
        {
            m_AIManager.UnRegisterEnemy(gameObject);
        }


        protected override void UpdateState()
        {
            switch (aiState)
            {
                case AIState.Invade:
                    Move(PlayerStateMachine.Instance.transform);
                    break;
                case AIState.Die:
                    break;
                case AIState.Explosion:
                    break;
            }
        }
        #endregion

        #region Hành động
        protected override void Move(Transform target)
        {
            if (target == null) return;
            Vector3 direction = target.position - transform.position;
            direction.y = 0f; // lock movement to the horizontal plane

            // Rotate the AI towards the target using the direction vector and a rotation speed
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        protected override void Die()
        {
            m_DropItemModule.DropItems();
            m_Animator.SetBool("Idle", true);

            for (int index = 0; index < m_Collider.Length; index++)
            {
                m_Collider[index].enabled = false;
            }

            m_Damagaeble.enabled = false;
            m_Burnable.enabled = false;
            m_Conductable.enabled = false;
            m_Target.enabled = false;
            m_Rigidbody.isKinematic = true;

            StartCoroutine(SelfExplosion());
        }

        public IEnumerator SelfExplosion()
        {
            aiState = AIState.Explosion;
            DamageArea.InflictDamageInArea(DamageExplosion, transform.position, DamageLayer, QueryTriggerInteraction.Collide, transform.gameObject);
            BodyTransform.gameObject.SetActive(false);
            ImpactVFX.Clear();
            ImpactVFX.Play();

            yield return new WaitForSeconds(ImpactVFX.main.duration);

            gameObject.SetActive(false);
        }

       
        private void OnTriggerEnter(Collider other)
        {
            if (DamageLayer == (DamageLayer | (1 << other.gameObject.layer)))
            {
                if (aiState == AIState.Invade)
                {
                    aiState = AIState.Die;
                    Die();
                }
            }
        }
        #endregion
    }
}
