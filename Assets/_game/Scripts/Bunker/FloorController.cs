using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public int floorIndex;
    public List<GameObject> m_Ceiling;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for(int index = 0; index < m_Ceiling.Count; index++)
            {
                m_Ceiling[index].gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int index = 0; index < m_Ceiling.Count; index++)
            {
                m_Ceiling[index].gameObject.SetActive(true);
            }
        }
    }
}
