using System.Collections;
using Common.FSM;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn.FSM
{
    public class TycoonAction : UnicornFSMAction
    {
        public TycoonAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameManager.Instance.UiController.UiMainLobby.Show(false);
            GameManager.Instance.UiController.UiTop.ActiveQuitButton();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            GameManager.Instance.TycoonManager.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
