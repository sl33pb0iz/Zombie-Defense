using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class Timer : MonoBehaviour
    {
        public float timeRemaining = 10;
        public TextMeshProUGUI timeTMP;
        private float timeLevel;

     
        void Update()
        {
            DisplayTime();
        }
        
        void DisplayTime()
        {
            timeLevel = Time.realtimeSinceStartup;
            string timeText = System.TimeSpan.FromSeconds(timeLevel).ToString("mm':'ss");
            timeTMP.text = timeText;
        }

        float GetTime()
        {
            return timeLevel;
        }
    }
}
