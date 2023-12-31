using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unicorn.Utilities
{
    /// <summary>
    /// Các hàm hỗ trợ chô UI Components
    /// </summary>
    public static class UIUlti
    {
        public static bool IsPointerOverUIObject()
        {
            //check mouse
            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            //check touch
            if (Input.touchCount > 0)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return true;
            }

            return false;
        }
        
        /// <summary>
        /// Sets the size of a rect transform to the specified one
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="newSize"></param>
        public static void SetSize(this RectTransform rectTransform, Vector2 newSize) 
        {
            Vector2 currSize = rectTransform.rect.size;
            Vector2 sizeDiff = newSize - currSize;
            rectTransform.offsetMin = rectTransform.offsetMin - new Vector2(sizeDiff.x * rectTransform.pivot.x, sizeDiff.y * rectTransform.pivot.y);
            rectTransform.offsetMax = rectTransform.offsetMax + new Vector2(sizeDiff.x * (1.0f - rectTransform.pivot.x), sizeDiff.y * (1.0f - rectTransform.pivot.y));
        }
    }
}