using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform m_Transform;
        private void Start()
        {
            m_Transform = GetComponent<Transform>();
        }

        private void Update()
        {
            m_Transform.LookAt(Camera.main.transform.position);
        }
    }
}
