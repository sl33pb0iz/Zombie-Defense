using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn.Utilities
{
        
    /// <summary>
    /// Tự động chỉnh lại transform khi Enable
    /// </summary>
    public class PresetPositioner : MonoBehaviour
    {
        public Vector3 presetPosition;
        public Quaternion presetRotation;
        public Vector3 presetScale;

        public bool canSet = false;
        public bool isLocally = true;

        public void OnEnable()
        {
            if (!canSet)
                return;

            if (isLocally)
            {
                transform.localPosition = presetPosition;
                transform.localRotation = presetRotation;
                transform.localScale = presetScale;
            } else
            {
                transform.position = presetPosition;
                transform.rotation = presetRotation;
                transform.localScale = presetScale;
            }
        }

        [Button]
        public void SetPosition()
        {
            if (isLocally)
            {
                presetPosition = transform.localPosition;
                presetRotation = transform.localRotation;
                presetScale = transform.localScale;
            } else
            {
                presetPosition = transform.position;
                presetRotation = transform.rotation;
                presetScale = transform.localScale;
            }
        }
    }
}