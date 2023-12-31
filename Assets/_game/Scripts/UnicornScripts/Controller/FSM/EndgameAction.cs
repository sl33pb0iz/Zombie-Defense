using Common.FSM;
using Unicorn.Utilities;
using UnityEngine;

namespace Unicorn.FSM
{
    public class EndgameAction : UnicornFSMAction
    {
        public EndgameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        
            SoundManager.Instance.StopFootStep();
            

            ProcessWinLose();

            SoundManager.Instance.PlayFxSound(GameManager.LevelManager.Result);
        }

        private void ProcessWinLose()
        {

            switch (GameManager.LevelManager.Result)
            {
                case LevelResult.Win:
                    GameManager.UiController.OpenUiWin(50);
                    Analytics.LogEndGameWin(GameManager.Instance.CurrentLevel);
                    break;
                case LevelResult.Lose:
                    GameManager.UiController.OpenUiLose();
                    Analytics.LogEndGameLose(GameManager.Instance.CurrentLevel);
                    break;
                default:
                    break;
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            SoundManager.Instance.StopSound(GameManager.LevelManager.Result);
        }
    }
}