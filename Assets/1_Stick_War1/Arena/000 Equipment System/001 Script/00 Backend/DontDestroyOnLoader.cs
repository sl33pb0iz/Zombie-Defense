using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowyy.EquipmentSystem
{
    public class DontDestroyOnLoader : MonoBehaviour
    {
        public static DontDestroyOnLoader Instance;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
