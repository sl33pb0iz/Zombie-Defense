using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class PlayerDefendState : PlayerBaseState
    {
        public PlayerDefendState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
            currentContext, playerStateFactory)
        {
            IsRootState = true;
            InitializeSubState();
        }

        public override void EnterState()
        {
            PlayerStateMachine.Instance.gunKeeper.SetActive(false);
            CTX.Animator.SetBool("isAttackMode", false);
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }
        public override void ExitState(){
            
        }
        public override void CheckSwitchStates()
        {
            
            switch (CTX.supState)
            {
                case PlayerStateMachine.SupState.Attack:
                    ExitState();
                    SwitchState(Factory.Attack());
                    break;

                case PlayerStateMachine.SupState.Die:
                    ExitState();
                    SwitchState(Factory.Die());
                    break;
            }
        }

        public override void InitializeSubState()
        {

            switch (CTX.subState)
            {
                case PlayerStateMachine.SubState.Build:
                    SetSubState(Factory.Build());
                    break;

                case PlayerStateMachine.SubState.Walk:
                    SetSubState(Factory.Walk());
                    break;
                default:
                    break;
            }
        }
    }
}
