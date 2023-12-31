using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class BlackBorderBanner : MonoBehaviour
    {
        public static BlackBorderBanner Instance;
        [SerializeField] private RectTransform borderBanner;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
            Debug.LogError("CURRENT SCREEN DPI " + Screen.dpi);
        }
        private void Update()
        {
            borderBanner.sizeDelta = new Vector2(borderBanner.sizeDelta.x, GetHeightInPixel());
        }
        public void ToggleBorder(bool isToggle)
        {
            //if (isToggle)
            //{
            //    if (Screen.width > 720)
            //    {
            //        borderBanner.sizeDelta = new Vector2(borderBanner.sizeDelta.x, 90.0f);
            //    }
            //    else
            //    {
            //        borderBanner.sizeDelta = new Vector2(borderBanner.sizeDelta.x, 50.0f);
            //    }
            //}
            borderBanner.gameObject.SetActive(isToggle);
        }
        //[Button]
        //public void ChangeSize()
        //{
        //    borderBanner.sizeDelta = new Vector2(borderBanner.sizeDelta.x, 50.0f);
        //}
        public float GetHeightInPixel()
        {
            return 50 * (Screen.dpi / 160.0f);
        }
        public float GetBorderBannerHeight()
        {
            if (borderBanner.gameObject.activeSelf) return borderBanner.sizeDelta.y;

            return 0;
        }
    }
}
