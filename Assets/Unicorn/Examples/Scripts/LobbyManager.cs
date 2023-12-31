using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn.Examples
{
    public class LobbyManager : LevelManager
    {
        [SerializeField] private new Camera camera;

        protected override void Awake()
        {
            base.Awake();
            SetUpCamera();
        }

        private void SetUpCamera()
        {
            CameraController mainCamera = GameManager.Instance.MainCamera;
            var mainCameraTransform = mainCamera.transform;
            mainCameraTransform.position = camera.transform.position;
            mainCameraTransform.rotation = camera.transform.rotation;
            mainCamera.Camera.fieldOfView = camera.fieldOfView;
        }

        public override void StartLevel()
        {
            //EndGame(LevelResult.Win);
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            Debug.Log("Lobby update");
        }
    }

}