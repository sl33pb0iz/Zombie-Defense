using Common.FSM;

namespace Unicorn.FSM
{
    public class ReviveAction : UnicornFSMAction
    {
        public ReviveAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }
    }
}