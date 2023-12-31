using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation.Samples;
using UnityEngine;

namespace Unicorn
{
    public class PlayerAttackState : PlayerBaseState
    {
        public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
            currentContext, playerStateFactory)
        {
            IsRootState = true;
            InitializeSubState();
        }

        public override void EnterState()
        {
            PlayerStateMachine.Instance.gunKeeper.gameObject.SetActive(true);
            CTX.Animator.SetBool("isAttackMode", true);
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
                case PlayerStateMachine.SupState.Defend:
                    ExitState();
                    SwitchState(Factory.Defend());
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
                case PlayerStateMachine.SubState.Shoot:
                    SetSubState(Factory.Shoot());
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
