using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class PlayerDieState : PlayerBaseState
    {
        public PlayerDieState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory):base(currentContext, playerStateFactory)
        {
            IsRootState = true;
            InitializeSubState();
        }

        public override void EnterState()
        {
            CTX.m_bodyCollision.enabled = false;
            CTX.m_VisionCollide.enabled = false;
            CTX.CharacterController.enabled = false;
            CTX.weaponManager.GetActiveWeapon().StopAttack();
            CTX.Animator.SetBool("isDieMode", true);
        }

        public override void UpdateState()
        {
            CheckSwitchStates();

        }
        public override void ExitState()
        {
            CTX.Animator.SetBool("isDieMode", false);
            CTX.m_bodyCollision.enabled = true;
            CTX.m_VisionCollide.enabled = false;
            CTX.CharacterController.enabled = true;
        }
        public override void CheckSwitchStates()
        {
            switch (CTX.supState)
            {
                case PlayerStateMachine.SupState.Attack:
                    ExitState();
                    SwitchState(Factory.Attack());
                    break;

                case PlayerStateMachine.SupState.Defend:
                    Debug.Log("switch to defend");
                    ExitState();
                    SwitchState(Factory.Defend());
                    break;
            }
        }
        public override void InitializeSubState(){}
    }
}
