using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unicorn;

public class TutHand : MonoBehaviour
{
    [SerializeField] private List<Transform> listTransPoint;
    [SerializeField] private Transform transHands;
    private List<Vector3> paths;
    Tweener tween;

    private bool isFirstTouch = true;
    private int touchCount = 0;

    private void OnEnable()
    {
        StartCoroutine(DelayDoTween());
    }

    IEnumerator DelayDoTween()
    {
        yield return Yielders.Get(.5f);
        if (paths == null)
        {
            paths = new List<Vector3>();
            for (int i = 0; i < listTransPoint.Count; i++)
            {
                paths.Add(listTransPoint[i].position);
            }
        }


            tween = transHands.DOPath(paths.ToArray(), 2f, PathType.Linear, PathMode.TopDown2D).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        
    }

    private void OnDisable()
    {
        tween.Rewind();
    }

    private void Update()
    {
        if (isFirstTouch && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            isFirstTouch = false;
            touchCount++;
            if (tween != null)
            {
                tween.Kill();
            }
        }
        if (touchCount > 1 || Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }
}
