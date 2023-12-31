using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;
using Unicorn.Examples;
using Spicyy.System;

namespace Unicorn
{
    public class PlayerStateMachine : MonoBehaviour
    {
        public enum SupState
        {
            Attack,
            Defend,
            Die,
        }
        [HideInInspector] public SupState supState;
        public enum SubState
        {
            Build,
            Shoot,
            Walk,
        }
        [HideInInspector] public SubState subState;

        //State Machine

        [TitleGroup("HOLD UP", alignment: TitleAlignments.Centered)]
        public PlayerSkinChanger m_PlayerEquipment;
        public CurrencyManager m_CurrencyManager;
        public PlayerBodyCollisionHandler m_bodyCollision;
        public PlayerVisionCollideHandler m_VisionCollide;
        public PlayerBuildManager m_BuildManager;
        public Animator animator;
        public GameObject gunKeeper;
        public UIPlayer m_UI;
        public PlayerSkillController m_PlayerSkillController;
        public WorldspaceHealthBar m_HealthBar;
        public Health m_health;
        public List<ParticleSystem> m_UpgradeStatVFXs;

        [TitleGroup("PARAMETER", alignment: TitleAlignments.Centered)]
        [SerializeField] private float rotateSpeed;
        [SerializeField] private float speedStat;

        public float reloadPaid = 1;

        [HideInInspector] public float timeDelayCountDown;

        //private
        private UltimateJoystick joystick;
        private CharacterController characterController;
        private PlayerBaseState _currentState;
        private PlayerStateFactory _states;
        private PlayerDataManager PlayerDataManager => PlayerDataManager.Instance;
        [HideInEditorMode] public WeaponManager weaponManager;
        public static PlayerStateMachine Instance;
        

        //getter and setter
        public Animator Animator { get { return animator; } }
        public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
        public CharacterController CharacterController { get { return characterController; } set { characterController = value; } }
        public float InputX { get; set; }
        public float InputZ { get; set; }
        public float RotateSpeed { get { return rotateSpeed; } set { rotateSpeed = value; } }
        public float Speed { get { return speedStat; } set { speedStat = value; } }
        public float Attack { get { return PlayerDataManager.DataPlayerStat.GetTotalATK() ; } }
        public float Health { get { return PlayerDataManager.DataPlayerStat.GetTotalHealth(); } }
        public float Defense { get { return PlayerDataManager.DataPlayerStat.GetTotalDefense(); } }
        public Vector3 GetPlayerDestination() { return transform.position; }

        //action
        public UnityAction onDie;
        public UnityAction onRevive;

        public void Awake()
        {
            Instance = this;
            joystick = FindObjectOfType<UltimateJoystick>();
            weaponManager = GetComponent<WeaponManager>();
            characterController = GetComponent<CharacterController>();
            m_PlayerEquipment = GetComponent<PlayerSkinChanger>();
            _states = new PlayerStateFactory(this);
        }

        private void OnEnable()
        {
            weaponManager.Init(this.gameObject, Attack);
            m_health.MaxHealth = Health;
            m_health.Defense = Defense;
            m_health.Init();

            EventManager.AddListener<InitLevelEvent>(OnInitLevel, 0);
            EventManager.AddListener<PlayerReviveEvent>(OnRevive);
            EventManager.AddListener<LevelWinEvent>(OnWin);
            EventManager.AddListener<StartLevelEvent>(OnStartLevel);
            EventManager.AddListener<PlayerChangeEquipmentEvent>(OnChangeEquipment);
            EventManager.AddListener<PlayerUpgradeStatEvent>(OnUpgradeStat);

            m_health.OnDie += OnDie;
            m_health.OnDamaged += OnDamaged;
        }

        public void OnInitLevel(InitLevelEvent evt)
        {
            _currentState = _states.Defend();
            _currentState.EnterState();
        }
        public void OnStartLevel(StartLevelEvent evt)
        {
            m_PlayerEquipment.Init();
            weaponManager.Init(this.gameObject, Attack);

            SwitchSubState();
        }

        public void OnUpdate()
        {
            InputX = joystick.GetHorizontalAxis();
            InputZ = joystick.GetVerticalAxis();

            _currentState.UpdateStates();
            SwitchSupState();
            SwitchSubState();
        }

        public void OnUpgradeStat(PlayerUpgradeStatEvent evt)
        {
            m_UpgradeStatVFXs[(int)evt.upgradeType].Clear();
            m_UpgradeStatVFXs[(int)evt.upgradeType].Play();
        }

        //SwitchState
        public void OnChangeEquipment(PlayerChangeEquipmentEvent evt) 
        { 
            m_PlayerEquipment.Init(); 
            weaponManager.Init(this.gameObject, Attack);
            m_health.MaxHealth = Health;
            m_health.Defense = Defense;
            m_health.Init(); 
        }
        public bool OnMove() => InputX != 0 || InputZ != 0;
        public bool OnDefend() => m_VisionCollide.NearAllies;
        public bool OnAttack() => m_VisionCollide.NearEnemies;
        public void OnDie() { onDie?.Invoke(); supState = SupState.Die; EventManager.Broadcast(Events.PlayerDeathEvent); }
        public void OnRevive(PlayerReviveEvent evt) { supState = SupState.Defend; m_health.HandleRevive();  }
        public void OnDamaged(float damaged, GameObject damageSource) { EventManager.Broadcast(Events.PlayerDamagedEvent); }
        public void OnWin(LevelWinEvent evt) 
        { 
            animator.SetTrigger("isWin");
            m_HealthBar.ActiveHealthBar(false);
            Vector3 cameraPosition = Camera.main.transform.position;
            cameraPosition.y = transform.position.y;
            transform.forward = cameraPosition - transform.position;
            weaponManager.GetActiveWeapon().StopAttack();
        }

        // Trasition State
        void SwitchSupState()
        {
                if ((OnDefend())&& OnAttack() )
                {
                    supState = SupState.Attack;
                }
                else if((OnDefend()) && !OnAttack())
                { 
                supState = SupState.Defend; 
                }
                else if((!OnDefend()) && OnAttack()) 
                {
                supState = SupState.Attack; 
                }
                else 
                {    
                supState = SupState.Defend;  
                }
        }
        void SwitchSubState()
        {
            switch (supState)
            {
                case SupState.Attack:
                    if (OnAttack()) { subState = SubState.Shoot; }
                    else subState = SubState.Walk;
                    break;
                case SupState.Defend:
                    subState = SubState.Walk;
                    break;
                case SupState.Die:
                    break;
                default: break;
            }
        }
        
        //Animation
        public void HandleAnimation(Vector3 animDir)
        {
            Animator.SetFloat("Horizontal", animDir.normalized.x);
            Animator.SetFloat("Vertical", animDir.normalized.z);
        }

    }
}
