using UnityEngine;
using Snowyy;
using System;

namespace Unicorn
{
    /// <summary>
    /// Base class cho hệ thống thay skin
    /// </summary>
    public abstract class SkinChanger<T1, T2> : MonoBehaviour where T1 : Enum where T2 : struct
    {
        public abstract void Init();
        public abstract void Change(T1 typeEquipment, T2 id);
    }
}