using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class PlayerWalkState : PlayerBaseState
    {
        private Vector3 animDir;
        private Vector3 _moveDirection;
        private float _gravity = -9.81f;
        private float _velocity;


        public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
            currentContext, playerStateFactory)
        {
        }

        public override void EnterState() {
            LookRotation();
            CTX.HandleAnimation(AnimDir()); }

        public override void UpdateState()
        {
            ApplyGravity();
            Move();
            LookRotation();
            CTX.HandleAnimation(AnimDir());
            CheckSwitchStates();
        }

        public override void ExitState(){}
        public override void CheckSwitchStates()
        {
            switch (CTX.subState)
            {
                case PlayerStateMachine.SubState.Shoot:
                    SwitchState(Factory.Shoot());
                    break;
                case PlayerStateMachine.SubState.Build:
                    SwitchState(Factory.Build());
                    break;
            }
        }
        public override void InitializeSubState() { }
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
        Vector3 AnimDir()
        {
            animDir = new Vector3(CTX.InputX, 0f, CTX.InputZ);
            animDir = CTX.transform.InverseTransformDirection(animDir);
            return animDir;
        }

    }
}
