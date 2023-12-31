using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUpAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;

    private TextMeshPro tmp;
    private float time = 0;
    private Vector3 origin;

    private Color originColor;
    private Vector3 originScale;
    private Vector3 originPosition;

    private void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshPro>();

        originColor = tmp.color;
        originScale = transform.localScale;
        originPosition = transform.position;
    }

    public void ResetStatus()
    {
        origin = transform.position + new Vector3(Random.Range(-2,2), 0, Random.Range(-2,2));

        // Reset the color, scale, and position to their original values
        tmp.color = originColor;
        transform.localScale = originScale;
        transform.position = originPosition;
        time = 0; 
    }

    private void OnEnable()
    {
        ResetStatus();
    }

    private void Update()
    {
        tmp.color = new Color(0.8f, 0.2f, 0, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0, 1 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }
}
