using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;

public class UIBillboarding : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        transform.forward = cam.transform.forward;
    }
}
