using Sirenix.OdinInspector;
using Spicyy.Weapon;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;

namespace Spicyy.AI
{
    public class AISoldierZombie : MobileAI, IEnemy
    {
        [Title("PARAMETER", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private float weaponDamage;

        [Title("VFX", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private GameObject DeathVfx;
        [SerializeField] private Transform DeathVfxSpawnPoint;

        [Title("HOLDERS", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private WeaponControl m_Weapon;

        private enum AIState
        {
            Invade,
            Attack,
            Die,
        }
        private AIState aiState;

        private AIManager m_AIManager;
        private Health m_Health;
        private Rigidbody m_Rigidbody;
        private Target m_Target;
        private DetectionModule m_DetectionModule;
        private Collider[] m_Collider;
        private Animator m_Animator;
        private Damageable m_Damagaeble;
        private Burnable m_Burnable;
        private Conductable m_Conductable;
        private DropItemModule m_DropItemModule;

        private GameObject KnownDetectedTarget => m_DetectionModule.KnownDetectedTarget;

        private void Awake()
        {
            m_AIManager = AIManager.Instance;
            m_DetectionModule = GetComponentInChildren<DetectionModule>();
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

        private void Start()
        {
            m_Weapon.OwnerDamage = weaponDamage;
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

            m_Health.OnDie += OnDie;

            m_DetectionModule.onAttack += OnAttack;
            m_DetectionModule.onDetectedTarget += OnDetectedTarget;
            m_DetectionModule.onLostTarget += OnLostTarget;

        }

        private void OnDisable()
        {
            UnRegisterEnemy();
            UnRegisterAI();

            m_DetectionModule.onAttack -= OnAttack;
            m_DetectionModule.onDetectedTarget -= OnDetectedTarget;
            m_DetectionModule.onLostTarget -= OnLostTarget;

            m_Health.OnDie -= OnDie;
        }

        private void Update()
        {
            m_DetectionModule.HandleTargetDetection();
            UpdateState();

        }

        protected override void Init()
        {
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
                case AIState.Attack:
                    Move(PlayerStateMachine.Instance.transform);
                    m_Weapon.Attack(KnownDetectedTarget.transform);
                    break;
                case AIState.Die:
                    m_Weapon.StopAttack();
                    break;
            }
        }

        protected override void Die()
        {
            m_DropItemModule.DropItems();

            for (int index = 0; index < m_Collider.Length; index++)
            {
                m_Collider[index].enabled = false;
            }

            m_Damagaeble.enabled = false;
            m_Burnable.enabled = false;
            m_Conductable.enabled = false;
            m_Target.enabled = false;
            m_Rigidbody.isKinematic = true;

            StartCoroutine(PlayDeathAnimation());
        }

        private IEnumerator PlayDeathAnimation()
        {
            if (DeathVfx)
            {
                var vfx = PoolManager.Instance.ReuseObject(DeathVfx, DeathVfxSpawnPoint.position, Quaternion.identity);
                vfx.SetActive(true);
            }

            int dieValue = Random.Range(0, 2);
            string die = (dieValue == 0) ? "Die1" : "Die2";
            m_Animator.SetTrigger(die);
            float animationTime = m_Animator.GetCurrentAnimatorStateInfo(0).length;

            float elapsedTime = 0f;
            while (elapsedTime < animationTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null; // Cho task khác chạy trong lúc chờ
            }

            // Khi animation kết thúc, setactive false.
            gameObject.SetActive(false);
        }

        protected override void Move(Transform target)
        {
            if (target == null) return;
            Vector3 direction = target.position - transform.position;
            direction.y = 0f; // lock movement to the horizontal plane

            // Rotate the AI towards the target using the direction vector and a rotation speed
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        void OnAttack()
        {
            aiState = AIState.Attack;
            m_Animator.SetBool("Attack", true);
        }

        void OnDetectedTarget()
        {
            aiState = AIState.Invade;
            m_Animator.SetBool("Attack", false);

        }

        void OnLostTarget()
        {
            aiState = AIState.Invade;
            m_Animator.SetBool("Attack", false);

        }

        void OnDie()
        {
            aiState = AIState.Die;
            Die(); 
        }
    }

}
