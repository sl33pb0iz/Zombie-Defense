using System;
using UnityEngine;

namespace Unicorn.Utilities
{
    /// <summary>
    /// Khiến gameObject xoay quanh trục
    /// </summary>
    public class Spinner : MonoBehaviour
    {
        [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 120, 0);

        private Vector3 lastEulerAngles;

        private void OnEnable()
        {
            lastEulerAngles = transform.eulerAngles;
        }

        private void Update()
        {
            lastEulerAngles += rotationSpeed * Time.deltaTime;
            transform.eulerAngles = lastEulerAngles;
        }
    }
}