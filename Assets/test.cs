using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Unicorn
{
    public class test : MonoBehaviour
    {

        public GameObject coin;
        protected  Vector3 teststart()
        {
            /*float randomJumpForce = Random.Range(1, 10);
            Vector3 randomPosition = new Vector3(transform.position.x + Random.Range(-10, 10), 2.5f, transform.position.z + Random.Range(-10, 10));
            transform.DOJump(randomPosition, randomJumpForce, 1, 1).OnComplete(() => m_Collider.enabled = true );*/

            var randomDirection = Random.insideUnitSphere * 5f;
            randomDirection += transform.position;
            NavMesh.SamplePosition(randomDirection, out var hit, 5f, 1);
            return hit.position;

            //transform.DOJump(finalPosition, 8f, 1, 0.5f);
        }

        [Sirenix.OdinInspector.Button]
        public void kkkk()
        {
            int count = 5;
            while(count > 0)
            {
                count--;
                Instantiate(coin, teststart(), coin.transform.rotation);

            }
        }
    }
}
