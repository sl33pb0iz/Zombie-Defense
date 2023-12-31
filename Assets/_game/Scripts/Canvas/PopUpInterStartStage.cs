using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unicorn;
using UnityEngine.Events;

public class PopUpInterStartStage : MonoBehaviour
{
    [SerializeField] private float interShowCDDuration;
    [SerializeField] private GameObject panel;
    private string placement;
    public void TogglePanel(string placement, UnityAction callback = null)
    {
        this.placement = placement;
        Toggle(callback);
    }

    public void Toggle(UnityAction callback = null)
    {
        var state = !panel.activeSelf;
        Debug.Log(state);
        panel.SetActive(state);
        if (state)
        {
#if !UNITY_EDITOR
            //if (DataManager.instance.currentData.isNoAds) return;
            if (!AdManager.Instance.IsInterstitialLoaded(AdEnums.ShowType.INTERSTITIAL)) return;
#endif
            StartCoroutine(IECountdownShowAd(callback));
        }
        
        Time.timeScale = state ? 0 : 1;
    }
    private IEnumerator IECountdownShowAd(UnityAction callback = null)
    {
        float t = 0;
        float remainingTime = interShowCDDuration + 1;
        while (t <= 1f)
        {
            t += Time.unscaledDeltaTime / interShowCDDuration;
            remainingTime -= Time.unscaledDeltaTime;
            yield return null;
        }

        if (Application.isMobilePlatform)
        {
            UnicornAdManager.ShowInterstitial(placement);
        }


        callback?.Invoke();
        Toggle();

    }
}
