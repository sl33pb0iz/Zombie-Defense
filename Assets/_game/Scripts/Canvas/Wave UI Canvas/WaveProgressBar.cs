using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Spicyy.System;
using Sirenix.OdinInspector;
public class WaveProgressBar : MonoBehaviour
{

    // Set up UI
    public Image progressBar;
    public RectTransform zombieWavePointPrefab;
    public TextMeshProUGUI zombieCount;

    private float wavePointSpacing;
    private int zombiesTotal;
    private int totalWaves;
    private float currProgress;
    private int index = 0; 

    private int zombiesTotalProgress;

    public float transitionDuration = 0.5f; // Thời gian chuyển đổi fillAmount
    private float targetFillAmount; // Giá trị fillAmount mục tiêu
    private Coroutine fillTransitionCoroutine; // Coroutine để chuyển đổi fillAmount

    public void SetZombiesTotal(int quantity)
    {
        zombieCount.text = quantity.ToString();
    }

    public void OnInitLevel(InitLevelEvent evt)
    {
        totalWaves = evt.wavesCount;
        zombieCount.text = "0";
        progressBar.fillAmount = 0;
    }

    public void OnSpawningZombies(StartSpawnEnemyEvent evt)
    {
        zombiesTotal = evt.enemiesTotal;
        zombiesTotalProgress = zombiesTotal;
        SetZombiesTotal(zombiesTotal);
    }

    public void OnZombieDeath(EnemyDeathEvent evt)
    {
        zombiesTotal--;
        SetZombiesTotal(zombiesTotal);
    }


    public void OnStartProgressBar(StartLevelEvent evt)
    {
        float progressBarWidth = progressBar.rectTransform.rect.width;
        wavePointSpacing = 1f / totalWaves; // Spacing between each wave point

        for (int i = 0; i < totalWaves; i++)
        {
            //Debug.Log("total wave point " + totalWaves);
            //GameObject zombieWavePoint = PoolManager.Instance.ReuseObject(zombieWavePointPrefab.gameObject, progressBar.transform.position, progressBar.transform.rotation);
            Instantiate(zombieWavePointPrefab.gameObject, progressBar.transform);
            //zombieWavePoint.SetActive(true);
            /*RectTransform wavePointRect = zombieWavePointPrefab;

            // Set the position of the wave point image based on the progress bar
            float wavePointXPosition = (i - 0.5f) * wavePointSpacing * progressBarWidth; // Center the wave point at each interval
            wavePointRect.anchoredPosition = new Vector2(wavePointXPosition, 0f);*/
        }
    }

    public void UpdateProgressBar(EnemySpawnEvent evt)
    {
        currProgress = (evt.enemiesSpawned / (float)zombiesTotalProgress) * wavePointSpacing;
        if(currProgress == wavePointSpacing)
        {
            index++;
            currProgress = 0;
        }
        targetFillAmount = currProgress + index * wavePointSpacing;
        if (fillTransitionCoroutine != null)
        {
            StopCoroutine(fillTransitionCoroutine);
        }

        fillTransitionCoroutine = StartCoroutine(UpdateFillAmountSmoothly(targetFillAmount));
    }

    private IEnumerator UpdateFillAmountSmoothly(float targetFill)
    {
        float initialFill = progressBar.fillAmount;
        float timeElapsed = 0f;

        while (timeElapsed < transitionDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / transitionDuration);
            progressBar.fillAmount = Mathf.Lerp(initialFill, targetFill, t);
            yield return null;
        }

        progressBar.fillAmount = targetFill;
    }

}