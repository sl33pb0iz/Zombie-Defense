using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn.Utilities
{
    /// <summary>
    /// Dùng class này để destroy FX. Lưu ý phải set particleSystem StopAction là callback.
    /// </summary>
    public class AutoDestructorFX : MonoBehaviour
    {
        private void OnParticleSystemStopped()
        {
            SimplePool.Despawn(gameObject);
        }
    }
}
