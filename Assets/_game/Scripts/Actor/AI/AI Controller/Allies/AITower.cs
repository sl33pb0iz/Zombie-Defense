using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unicorn;
using Spicyy.Weapon;

namespace Spicyy.AI
{
    public class AITower : StaticAI, IAlly
    {

        [Title("PARAMETER", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private float OrientationSpeed = 360f;
        [SerializeField] private float DetectionFireDelay = 1f;
        [SerializeField] private float weaponMaxRotateAngle = 20f;
        [SerializeField] private float weaponRotateSpeed = 5f;
        [SerializeField] private float damage = 0f; 

        [Title("HOLD UP", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private Transform m_BodyDirection;
        [SerializeField] private DetectionModule m_DetectionModule;
        [SerializeField] private WeaponControl m_CurrentWeapon;
        [SerializeField] private Animation[] m_TurretAttackAnim;

        private enum AIState
        {
            Patrol,
            Attack,
        }

        private AIState AiState;
        private float m_TimeStartedDetection;
        private Quaternion m_StartWeaponRotation;

        private GameObject KnownDetectedTarget => m_DetectionModule.KnownDetectedTarget;

        private void Start()
        {
            m_CurrentWeapon.OwnerDamage = damage;
            m_CurrentWeapon.Owner = this.gameObject;
        }

        private void OnEnable()
        {
            m_TimeStartedDetection = Mathf.NegativeInfinity;
            m_StartWeaponRotation = m_CurrentWeapon.gameObject.transform.localRotation;
            m_DetectionModule.onDetectedTarget += OnDetectedTarget;
            m_DetectionModule.onLostTarget += OnLostTarget;

            AiState = AIState.Patrol;
        }

        private void OnDisable()
        {
            m_DetectionModule.onDetectedTarget -= OnDetectedTarget;
            m_DetectionModule.onLostTarget -= OnLostTarget;
        }

        public void RegisterAlly()
        {
            throw new global::System.NotImplementedException();
        }

        public void UnRegisterAlly()
        {
            throw new global::System.NotImplementedException();
        }

        private void Update()
        {
            UpdateCurrentAiState();
            m_DetectionModule.HandleTargetDetection();

        }

        void UpdateCurrentAiState()
        { 
            switch (AiState)
            {
                case AIState.Patrol:
                    if (m_TurretAttackAnim.Length > 0)
                    {
                        foreach (var anim in m_TurretAttackAnim)
                        {
                            anim.Stop();
                        }
                    }
                    OrientWeaponToRoot();
                    RotateBodyAroundToDetect();
                    break;
                case AIState.Attack:
                    bool mustShoot = Time.time > m_TimeStartedDetection + DetectionFireDelay;
                    RotateBodyForwardToTarget();
                    if (mustShoot)
                    {
                        if (KnownDetectedTarget)
                        {
                            OrientWeaponTowards(KnownDetectedTarget.transform);
                            m_CurrentWeapon.Attack(KnownDetectedTarget.transform);
                            if (m_TurretAttackAnim.Length > 0)
                            {
                                foreach (var anim in m_TurretAttackAnim)
                                {
                                    anim.Play();
                                }
                            }
                        }
                    }
                    break;
            }
        }
        void OnDetectedTarget()
        {
            AiState = AIState.Attack;
            m_TimeStartedDetection = Time.time;
        }
        void OnLostTarget()
        {
            AiState = AIState.Patrol;
            m_CurrentWeapon.StopAttack();
        }

        #region Hành động
        public void RotateBodyAroundToDetect()
        {
            if (m_BodyDirection)
            {
                Vector3 currentRotation = m_BodyDirection.eulerAngles;
                m_BodyDirection.eulerAngles = currentRotation;
                m_BodyDirection.Rotate(Vector3.up, OrientationSpeed / 5 * Time.deltaTime);
            }
        }
        public void RotateBodyForwardToTarget()
        {
            if (m_BodyDirection)
            {
                Vector3 directionToTarget =
                (KnownDetectedTarget.transform.position - m_BodyDirection.position);
                Quaternion offsettedTargetRotation = Quaternion.LookRotation(directionToTarget.normalized, Vector3.up);
                var euler = offsettedTargetRotation.eulerAngles;
                euler.x = 0;
                euler.z = 0;
                offsettedTargetRotation = Quaternion.Euler(euler);
                m_BodyDirection.localRotation = Quaternion.Slerp(m_BodyDirection.localRotation, offsettedTargetRotation,
                    5 * Time.deltaTime);
                // shoot
            }
        }
        public void OrientWeaponToRoot()
        {
            if (m_CurrentWeapon)
            {
                Quaternion lookDirection = m_StartWeaponRotation;
                if (m_CurrentWeapon.transform.localRotation != lookDirection)
                {
                    m_CurrentWeapon.transform.localRotation = Quaternion.Slerp(m_CurrentWeapon.transform.localRotation, lookDirection, weaponRotateSpeed / 5 * Time.deltaTime);
                }
            }
        }
        public void OrientWeaponTowards(Transform target)
        {
            Vector3 weaponDirection = (target.position - m_CurrentWeapon.transform.position).normalized;
            if (weaponDirection.sqrMagnitude != 0f)
            {
                Quaternion lookDirection = Quaternion.LookRotation(weaponDirection.normalized, Vector3.up);
                var euler = lookDirection.eulerAngles;
                if (euler.x <= 180)
                {
                    euler.x = Mathf.Clamp(euler.x, 0, weaponMaxRotateAngle);
                }
                else euler.x = Mathf.Clamp(euler.x, 360 - weaponMaxRotateAngle, 360);
                euler.y = 0;
                euler.z = 0;
                lookDirection = Quaternion.Euler(euler);
                m_CurrentWeapon.transform.localRotation = Quaternion.Slerp(m_CurrentWeapon.transform.localRotation, lookDirection, weaponRotateSpeed * Time.deltaTime);
            }
        }
        #endregion
    }
}
