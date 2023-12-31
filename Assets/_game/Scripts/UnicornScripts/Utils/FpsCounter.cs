using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.Utilities
{
    /// <summary>
    /// Lấy được từ Standard Assets của Unity. 
    /// </summary>
    public class FpsCounter : MonoBehaviour
    {
        public TextMeshProUGUI fpsCounter;

        int m_frameCounter = 0;
        float m_timeCounter = 0.0f;
        float m_lastFramerate = 0.0f;
        public float m_refreshTime = 0.5f;

        // Update is called once per frame
        void Update()
        {

            if (m_timeCounter < m_refreshTime)
            {
                m_timeCounter += Time.deltaTime;
                m_frameCounter++;
            }
            else
            {
                //This code will break if you set your m_refreshTime to 0, which makes no sense.
                m_lastFramerate = (float) m_frameCounter / m_timeCounter;
                m_frameCounter = 0;
                m_timeCounter = 0.0f;
            }

            fpsCounter.text = m_lastFramerate.ToString();
        }
    }
}
