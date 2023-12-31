using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using TMPro;
using UnityEngine.UI; 

public class UITutNode : MonoBehaviour
{
    private Camera cam;
    public Node m_Node;

    public TextMeshProUGUI m_Text;
    public Image image;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        transform.forward = cam.transform.forward;
        if (m_Node.Tower != null) gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_Text.gameObject.SetActive(false);
            image.gameObject.SetActive(false); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_Text.gameObject.SetActive(true);
            image.gameObject.SetActive(true);
        }
    }
}
