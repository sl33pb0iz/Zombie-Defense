using UnityEngine;

namespace Unicorn.Utilities
{
    /// <summary>
    /// Chỉnh rotation hướng về camera. Hữu dụng cho các UI ở world space.
    /// </summary>
    public class LookAtCamera : MonoBehaviour {
 
 
        private new Transform camera;
        void Start () {
 
            camera = GameManager.Instance.MainCamera.transform;
        }
 
        void Update()
        {
            // Rotate the camera every frame so it keeps looking at the target
            transform.forward = camera.forward;
        }
 
    }
}