using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class VibrationManager : MonoBehaviour
    {
        public static VibrationManager instance;
        private void Awake()
        {
            instance = this;
            Vibration.Init();
        }
        private bool isVibrationMuted => !PlayerDataManager.Instance.GetVibrateSetting();
        public void VibratePop()
        {
            if (isVibrationMuted) return;
            Vibration.VibratePop();
        }
    }
}

