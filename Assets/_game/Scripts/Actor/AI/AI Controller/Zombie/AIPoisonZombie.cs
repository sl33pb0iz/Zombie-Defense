using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;

namespace Spicyy.AI
{
    public class AIPoisonZombie : MobileAI, IEnemy
    {
        [Title("VFX", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private GameObject DeathVfx;
        [SerializeField] private Transform DeathVfxSpawnPoint;

        [Title("DISCHARGE POISON", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private float damage;
        [SerializeField] private float range;
        [SerializeField] private LayerMask damageLayer;
        [SerializeField] private ParticleSystem poisonVFX;

        private enum AIState
        {
            Invade,
            Die,
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

            poisonVFX.Clear();
            poisonVFX.Play(); 
            InvokeRepeating(nameof(DischargePoison), 0, 0.5f);
        }

        private void OnDisable()
        {
            UnRegisterEnemy();
            UnRegisterAI();

            m_Health.OnDie -= () => { Die(); aiState = AIState.Die; };

            poisonVFX.Clear();
            poisonVFX.Stop();
            CancelInvoke(nameof(DischargePoison));
        }

        private void Update()
        {
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
                case AIState.Die:
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
        private void DischargePoison()
        {
                Collider[] affectedColliders = Physics.OverlapSphere(transform.position, range, damageLayer, QueryTriggerInteraction.Collide);
                foreach (var coll in affectedColliders)
                {
                    Damageable damageable = coll.GetComponent<Damageable>();
                    if (damageable)
                    {
                        damageable.InflictDamage(damage, false, gameObject);
                    }
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

            int dieValue = UnityEngine.Random.Range(0, 2);
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
       

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.2f); // Đặt màu cho vùng xung quanh
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.up, range); // Vẽ vùng tròn

            UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.8f); // Đặt màu cho viền vùng xung quanh
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, range); // Vẽ viền vùng tròn
        }
#endif
        #endregion
    }
}
