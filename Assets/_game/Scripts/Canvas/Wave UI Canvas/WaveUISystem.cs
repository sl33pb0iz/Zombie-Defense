using UnityEngine;
using System.Text;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Spicyy.System;
using Sirenix.OdinInspector;
using System.Collections;

public class WaveUISystem : MonoBehaviour
{
    [FoldoutGroup("Text")] public TextMeshProUGUI waveCount;
    [FoldoutGroup("Text")] public TextMeshProUGUI waveMission;
    [FoldoutGroup("Text")] public TextMeshProUGUI waveWarning;


    [FoldoutGroup("Component")] public DOTweenAnimation waveTween;
    [FoldoutGroup("Component")] public WaveProgressBar m_WaveProgressBar;

    private Coroutine countdownCoroutine;

    public enum WaveUIStateTime
    {
        Time_0 = 0,
        Time0 = 1,
        Time1 = 2,
        Time6 = 4,
    }

    private StringBuilder txtWaveWarningBuilder = new StringBuilder();

    private void OnEnable()
    {
        EventManager.AddListener<NewStageStartEvent>(OnStartNewStage);
        EventManager.AddListener<StartLevelEvent>(OnStartProgressBar);
        EventManager.AddListener<StartSpawnEnemyEvent>(OnSpawningZombies);
        EventManager.AddListener<EnemyDeathEvent>(OnZombieDeath);
        EventManager.AddListener<EnemySpawnEvent>(UpdateProgressBar);
    }

    private void OnDisable()
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);
    }
    private void OnSpawningZombies(StartSpawnEnemyEvent evt) => m_WaveProgressBar.OnSpawningZombies(evt);

    private void OnZombieDeath(EnemyDeathEvent evt) => m_WaveProgressBar.OnZombieDeath(evt);

    private void OnStartProgressBar(StartLevelEvent evt) => m_WaveProgressBar.OnStartProgressBar(evt);

    private void UpdateProgressBar(EnemySpawnEvent evt) => m_WaveProgressBar.UpdateProgressBar(evt);

    private void OnStartNewStage(NewStageStartEvent evt)
    {
        int currStage = evt.currStage + 1;
        SetWaveCount("STAGE " + currStage);
        if(countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        yield return new WaitForSeconds(0f); 
        UpdateWaveUIState(WaveUIStateTime.Time6);

        yield return new WaitForSeconds(5f); 
        UpdateWaveUIState(WaveUIStateTime.Time1);

        yield return new WaitForSeconds(1f); 
        UpdateWaveUIState(WaveUIStateTime.Time0);

        yield return new WaitForSeconds(1f); 
        UpdateWaveUIState(WaveUIStateTime.Time_0);
    }

    public void UpdateWaveUIState(WaveUIStateTime state)
    {
        switch (state)
        {
            case WaveUIStateTime.Time6:
                ActiveWaveWarningUI(true, Color.white);
                PlayWaveTweenAnimation(true);
                txtWaveWarningBuilder.Length = 0;
                txtWaveWarningBuilder.Append("ZOMBIES ARE COMING!");
                break;
            case WaveUIStateTime.Time1:
                ActiveWaveWarningUI(true, Color.red);
                break;
            case WaveUIStateTime.Time0:
                PlayWaveTweenAnimation(false);
                txtWaveWarningBuilder.Length = 0;
                txtWaveWarningBuilder.Append("KILL THEM ALL");
                break;
            case WaveUIStateTime.Time_0:
                ActiveWaveWarningUI(false);
                break; 
        }
        SetWaveWarningText(txtWaveWarningBuilder.ToString());
    }

    public void PlayWaveTweenAnimation(bool isPlay)
    {
        if (isPlay)
        {
            waveTween.DOPlay();
        }
        else waveTween.DOPause();
    }

    public void ActiveWaveWarningUI(bool value)
    {
        waveWarning.gameObject.SetActive(value);
    }

    public void SetWaveWarningText(string text)
    {
        waveWarning.text = text;
    }

    public void ActiveWaveWarningUI(bool value, Color color)
    {
        waveWarning.gameObject.SetActive(value);
        // Sử dụng 'color' trong các xử lý khác nếu cần.
        waveWarning.color = color;
    }

    public void SetWaveCount(string value)
    {
        waveCount.text = value;
    }
}
