using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class CameraSwitcher : MonoBehaviour
    {
        private enum CameraState
        {
            World,
            Player,
        }
        public CameraRotation CamWorld; 
        private CameraState camState;
        private Animator animator;


        void Awake()
        {
            animator = GetComponent<Animator>();
            camState = CameraState.World;
        }


        public void SwitchCamera()
        {
            switch (camState)
            {
                case CameraState.World:
                    {
                        camState = CameraState.Player;
                        break;
                    }
                case CameraState.Player:
                    {
                        camState = CameraState.World;
                        break;
                    }
            }
            UpdateCameraAction();
        }

        void UpdateCameraAction()
        {
            switch (camState)
            {
                case CameraState.World:
                    {
                        animator.Play("WorldCamera");
                        break;
                    }
                case CameraState.Player:
                    {
                        animator.Play("PlayerCamera");
                        break;
                    }
            }
        }
    }
}
