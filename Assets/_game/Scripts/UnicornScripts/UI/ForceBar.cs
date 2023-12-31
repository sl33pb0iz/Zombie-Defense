using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

namespace Unicorn.UI
{
    public class ForceBar : MonoBehaviour
    {
        public DOTweenAnimation handleAnimation;
        public RectTransform handle;
        public float MaxPosition;
        public float MinPosition;
        public int[] values = {2, 4, 6, 4, 2};

        public float positionLocal;

        public void StopRunning()
        {
            handleAnimation.DOPause();
        }

        public int GetValue()
        {
            float length = MaxPosition - MinPosition;
            float lengthOfOne = length / values.Length;
            float currentLength = handle.anchoredPosition.x;
            int index = Mathf.FloorToInt(currentLength / lengthOfOne);
            if (index == 5) index = 4;
            return values[index];
        }

    }

}