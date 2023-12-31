using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class UiNotEnough : UICanvas
    {
        [SerializeField] private TextMeshProUGUI txtNotEnough;

        [SerializeField] private Image imgBg;
        [SerializeField] private TextMeshProUGUI txtDisplay;

        private bool isStarted = false;

        private Sequence animSequence;

        private void Start()
        {
            if (isStarted) return;
            isStarted = true;
            animSequence = DOTween.Sequence()
                .Append(imgBg.transform.DOLocalMoveY(0f, 0.5f).SetEase(Ease.OutQuad))
                .Join(imgBg.DOFade(1f, 0.5f).SetEase(Ease.OutQuad))
                .Join(txtDisplay.DOFade(1f, 0.5f).SetEase(Ease.OutQuad))
                .AppendInterval(0.75f)
                .Append(imgBg.DOFade(0f, 0.75f).SetEase(Ease.Linear))
                .Join(txtDisplay.DOFade(0f, 0.5f).SetEase(Ease.Linear))
                .SetAutoKill(false)
                .OnComplete(() => { Show(false); });
            animSequence.Rewind();
        }

        public void Init(string txtDisplay)
        {
            Start();
            Show(false);
            animSequence.Rewind();
            txtNotEnough.text = txtDisplay;
            Show(true);
            animSequence.Play();
        }
    }
}