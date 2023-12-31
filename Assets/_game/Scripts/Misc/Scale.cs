using UnityEngine;
using DG.Tweening;

public class Scale : MonoBehaviour
{
    #region Variables

    private Vector3 _originalScale;
    private Vector3 _scaleTo;

    #endregion


    void Start()
    {
        _originalScale = transform.localScale;
        _scaleTo = _originalScale * 1.5f;

        OnScale();
    }

    private void OnScale()
    {
        transform.DOScale(_scaleTo, 2.0f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOScale(_originalScale, 2.0f)
                    .SetEase(Ease.OutQuad)
                    .SetDelay(1.0f)
                    .OnComplete(OnScale);
            });
    }
}