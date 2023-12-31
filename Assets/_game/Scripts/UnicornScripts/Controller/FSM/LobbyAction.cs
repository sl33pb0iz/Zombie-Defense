using Common.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn.FSM
{
    public class LobbyAction : UnicornFSMAction
    {
        public LobbyAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameManager.Instance.UiController.UiMainLobby.Show(true);
        }
        public override void OnExit()
        {
            base.OnExit();
            SoundManager.Instance.StopSound(SoundManager.GameSound.BGM);
        }
    }
}