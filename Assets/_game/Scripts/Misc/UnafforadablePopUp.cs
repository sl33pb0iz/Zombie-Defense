using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnafforadablePopUp : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve heightCurve;

    private TextMeshProUGUI tmp;
    private float time = 0;
    private Vector3 origin;

    private Color originColor;
    private Vector3 originPosition;

    private void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        originColor = tmp.color;
        originPosition = transform.position;
    }

    public void ResetStatus()
    {
        origin = transform.position;

        // Reset the color, scale, and position to their original values
        tmp.color = originColor;
        transform.position = originPosition;
        time = 0;
    }

    private void OnEnable()
    {
        ResetStatus();
    }

    private void Update()
    {
        tmp.color = new Color(0f, 0f, 0, opacityCurve.Evaluate(time));
        transform.position = origin + new Vector3(0, 1 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }
}
