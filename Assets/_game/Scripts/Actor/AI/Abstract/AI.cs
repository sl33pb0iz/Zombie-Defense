using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spicyy.AI
{
    public abstract class AI : MonoBehaviour
    {
        protected virtual void RegisterAI()
        {
            AIManager.Instance.RegisterAI(this);
        }

        protected virtual void UnRegisterAI()
        {
            AIManager.Instance.UnregisterAI(this);
        }

        protected abstract void Init();
        protected abstract void UpdateState();
    }

}
