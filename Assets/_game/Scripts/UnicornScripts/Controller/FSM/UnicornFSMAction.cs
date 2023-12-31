using System;
using Common.FSM;

namespace Unicorn.FSM
{
    public abstract class UnicornFSMAction : FSMAction
    {
        public event Action Entered;
        public event Action Exited;
        
        protected GameManager GameManager { get; }
        
        protected UnicornFSMAction(GameManager gameManager, FSMState owner) : base(owner)
        {
            GameManager = gameManager;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Entered?.Invoke();
        }

        public override void OnExit()
        {
            base.OnExit();
            Exited?.Invoke();
        }
    }
}