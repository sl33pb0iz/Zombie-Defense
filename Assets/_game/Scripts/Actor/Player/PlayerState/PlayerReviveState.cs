using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class PlayerReviveState : PlayerBaseState
    {
        public PlayerReviveState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

        public override void EnterState()
        {
            CTX.m_bodyCollision.enabled = true;
            CTX.m_VisionCollide.enabled = true;

            CTX.Animator.SetBool("isDieMode", false);
        }

        public override void UpdateState()
        {
            CheckSwitchStates();

        }
        public override void ExitState() { }
        public override void CheckSwitchStates() { }
        public override void InitializeSubState() { }
    }
}

