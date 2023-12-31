using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class MoneyPaid : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(Vector3.up, 360 * Time.deltaTime, Space.Self);
        }
        
    }
}
