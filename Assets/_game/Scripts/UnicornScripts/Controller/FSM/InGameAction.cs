using Common.FSM;
using UnityEngine;

namespace Unicorn.FSM
{
    public class InGameAction : UnicornFSMAction
    {

        public InGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            LevelManager.Instance.StartLevel();
            GameManager.Instance.UiController.UiMainLobby.Show(false);
            SoundManager.Instance.PlayFxSound(soundEnum: SoundManager.GameSound.Lobby);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
      
        public override void OnUpdate()
        {
            base.OnUpdate();
            GameManager.Instance.LevelManager.OnUpdate();
        }
    }

    public class CopyOfInGameAction : UnicornFSMAction
    {

        public CopyOfInGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameManager.UiController.UiMainLobby.Show(false);
            SoundManager.Instance.PlayFxSound(soundEnum: SoundManager.GameSound.Lobby);
        }

        public override void OnExit()
        {
            base.OnExit();

        }

        public override void OnUpdate()
        {
            Debug.Log("In game update");
            base.OnUpdate();
            GameManager.Instance.LevelManager.OnUpdate();
        }
    }
}