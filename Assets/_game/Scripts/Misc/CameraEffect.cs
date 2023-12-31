using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Spicyy.System;

public class CameraEffect : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    [BoxGroup("Shake")] public float _intensity;
    [BoxGroup("Shake")] public float _time;

    [BoxGroup("Hurt")] public Image _bloodOverlay;
    [BoxGroup("Hurt")] public float _appearDuration;
    [BoxGroup("Hurt")] public float _fadeDuration; 

    private float shakeTimer;
    private float shakeTimerTotal;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        EventManager.AddListener<LevelWinEvent>(OnPlayerWin);
        EventManager.AddListener<PlayerDamagedEvent>(OnPlayerDamaged);
        EventManager.AddListener<PlayerVibratedEvent>(OnPlayerVibrated);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<LevelWinEvent>(OnPlayerWin);
        EventManager.RemoveListener<PlayerDamagedEvent>(OnPlayerDamaged);
        EventManager.RemoveListener<PlayerVibratedEvent>(OnPlayerVibrated);
    }

    void OnPlayerWin(LevelWinEvent evt) => StartCoroutine(ZoomInToPlayer());
    void OnPlayerDamaged(PlayerDamagedEvent evt) => HurtEffect(_appearDuration, _fadeDuration);
    void OnPlayerVibrated(PlayerVibratedEvent evt ) => ShakeCamera(_intensity, _time);

    public void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(_intensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
            }
        }
    }

    
    private IEnumerator ZoomInToPlayer()
    {
        // Get the initial camera field of view
        float initialFOV = cinemachineVirtualCamera.m_Lens.FieldOfView;

        // Set the target field of view for the zoom-in effect
        float targetFOV = initialFOV / 2f;

        // Define the duration of the zoom-in effect
        float zoomDuration = 2f;

        // Get the current time
        float currentTime = 0f;

        // Zoom in gradually
        while (currentTime < zoomDuration)
        {
            // Increment the time
            currentTime += Time.deltaTime;

            // Calculate the interpolation factor
            float t = currentTime / zoomDuration;

            // Interpolate the field of view between initialFOV and targetFOV
            float currentFOV = Mathf.Lerp(initialFOV, targetFOV, t);
            cinemachineVirtualCamera.m_Lens.FieldOfView = currentFOV;

            yield return null;
        }
    }

    private void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        shakeTimer = time;
        shakeTimerTotal = time;
    }

    private void HurtEffect(float appearDuration, float fadeDuration)
    {
        IEnumerator Animate(float duration, float state)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / duration;
                var bloodColor = _bloodOverlay.color;
              
                bloodColor.a = Mathf.Abs(state - t);
                _bloodOverlay.color = bloodColor;
                yield return null;
            }
            if (state == 0) StartCoroutine(Animate(fadeDuration, 0.5f));
            else
            {
                var bloodColor = _bloodOverlay.color;
                bloodColor.a = 0;
                _bloodOverlay.color = bloodColor;
            }
        }
        StartCoroutine(Animate(appearDuration, 1f));
    }



}
