using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unicorn
{
    public class PlayerShootState : PlayerBaseState
    {
        private Transform target = null;
        private Vector3 _moveDirection;
        private float _gravity = -9.81f;
        private float _velocity;
        private List<Damageable> damageableTargets => CTX.m_VisionCollide.enemies;
        public PlayerShootState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
            currentContext, playerStateFactory)
        {
        }
        public override void EnterState()
        {
        }
        public override void UpdateState()
        {
            ApplyGravity();
            Move();
            CTX.HandleAnimation(Animdir());
            if (TargetInRangeAttack())
            {
                target = TargetInRangeAttack();
                ShootTarget();
            }
            else
            {
                target = null;
                LookRotation();
            }
            CheckSwitchStates();
        }

        public override void ExitState()
        {
        }

        public override void CheckSwitchStates()
        {

            switch (CTX.subState)
            {
                case PlayerStateMachine.SubState.Walk:
                    SwitchState(Factory.Walk());
                    break;
            }
        }

        public override void InitializeSubState() { }

        void ApplyGravity()
        {
            if (CTX.CharacterController.isGrounded && _velocity < 0.0f)
            {
                _velocity = -1.0f;
            }
            else
            {
                _velocity += _gravity * Time.deltaTime;
            }

            _moveDirection.y = _velocity;
        }


        void Move()
        {
            _moveDirection = new Vector3(CTX.InputX, _moveDirection.y, CTX.InputZ).normalized;
            CTX.CharacterController.Move(_moveDirection * CTX.Speed * Time.deltaTime);
            if (CTX.OnMove())
            {
                SoundManager.Instance.PlayFxSound(SoundManager.GameSound.Footstep);
            }
            else SoundManager.Instance.StopSound(SoundManager.GameSound.Footstep);
        }
        public void LookRotation()
        {
            if (CTX.InputX != 0 && CTX.InputZ != 0)
            {
                _moveDirection = new Vector3(CTX.InputX, 0, CTX.InputZ).normalized;
                if (_moveDirection != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(_moveDirection);
                    float step = CTX.RotateSpeed * Time.deltaTime;
                    CTX.transform.rotation = Quaternion.RotateTowards(lookRotation, CTX.transform.rotation, step);
                }
            }
        }
        Vector3 Animdir()
        {
            Vector3 animDir = new Vector3(CTX.InputX, 0f, CTX.InputZ);
            animDir = CTX.transform.InverseTransformDirection(animDir);
            return animDir;
        }

        public void OrientWeaponsTowards(Vector3 lookPosition)
        {
            lookPosition += new Vector3(0, CTX.m_bodyCollision.m_Collider.height / 2, 0);
            Vector3 weaponForward = (lookPosition - CTX.weaponManager.GetActiveWeapon().transform.position).normalized;
            Quaternion weaponTargetRotate = Quaternion.LookRotation(weaponForward);

            // create a new quaternion that only rotates around the Y-axis
            Quaternion yRotation = Quaternion.Euler(0, weaponTargetRotate.eulerAngles.y, 0);

            // apply the Y-axis rotation to the weapon's transform
            CTX.weaponManager.GetActiveWeapon().transform.rotation = yRotation;
        }

        void ShootTarget()
        {
            Vector3 targetDirection = (target.position - CTX.weaponManager.GetActiveWeapon().aimDirection.transform.position).normalized;
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            CTX.transform.rotation = Quaternion.RotateTowards(CTX.transform.rotation, targetRotation, Time.deltaTime * CTX.RotateSpeed);
            if (CTX.weaponManager.GetActiveWeapon() && CTX.weaponManager.GetActiveWeapon().gameObject.activeSelf)
            {
                CTX.weaponManager.GetActiveWeapon().Attack();

            }
        }

        public Transform TargetInRangeAttack()
        {
            float closestEnemy = float.MaxValue;
            if (damageableTargets.Count > 0)
            {
                foreach (Damageable damageable in damageableTargets)
                {
                    float distanceToEnemy = Vector3.Distance(CTX.transform.position, damageable.transform.position);
                    if (distanceToEnemy < closestEnemy)
                    {
                        closestEnemy = distanceToEnemy;
                        target = damageable.transform;
                    }
                }
                return target;
            }
            else return null;
        }
        private void SwitchStateAndLook()
        {
            Vector3 joystickDir = new Vector3(CTX.InputX, 0f, CTX.InputZ);
            if (joystickDir != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(joystickDir);
                CTX.transform.rotation = Quaternion.RotateTowards(CTX.transform.rotation, lookRotation, Time.deltaTime * CTX.RotateSpeed);
                if (CTX.transform.rotation == lookRotation)
                {
                    SwitchState(Factory.Walk());
                }
            }

        }
    }
}
