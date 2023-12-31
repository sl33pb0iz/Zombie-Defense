using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using TMPro;

namespace Unicorn
{
    public class LoadingStartManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup group;
        [SerializeField] private Image imgLoading;
        [SerializeField] private float timeLoading = 5;
        public TextMeshProUGUI Narratore;
        public TextMeshProUGUI Version;


        private AsyncOperation loadSceneAsync;
        private AppOpenAdManager appOpenAdManager;
        private readonly string CurrentChecking = "Loading";
        private bool GameLoaded = false;
        public static LoadingStartManager Instance { get; set; }

        [SerializeField] private PopupGDPR popupGDPR;

        private void Awake()
        {
            appOpenAdManager = AppOpenAdManager.Instance;
            Instance = this;
        }

        void Start()
        {
            popupGDPR.ActionClose = Init;
            Version.text = Application.version + ".000.0";

            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

            if (popupGDPR.IsChecked())
            {
                Init();
            }

        }

        public void Init()
        {
            UnicornAdManager.Init();
            LoadAppOpen();
            DontDestroyOnLoad(gameObject);
            LoadMasterLevel();
            RunLoadingBar();
            StartCoroutine(TextNarrator());
        }

        private void LoadAppOpen()
        {
#if UNITY_EDITOR
            return;
#endif
            MobileAds.Initialize(initStatus => { appOpenAdManager.LoadAd(); });
        }

        private void LoadMasterLevel()
        {
            loadSceneAsync = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }

        private void RunLoadingBar()
        {
            imgLoading.DOFillAmount(0.9f, timeLoading)
                .SetEase(Ease.OutQuint)
                .OnComplete(() => { StartCoroutine(Fade()); });
        }


        private IEnumerator Fade()
        {
            yield return new WaitUntil(() => loadSceneAsync.isDone);
            imgLoading.DOFillAmount(1f, 0.1f);
            group.DOFade(0, 0.2f)
                .OnComplete(() => { GameLoaded = true;  Destroy(group.gameObject); });
        }

        IEnumerator TextNarrator()
        {
            if (GameLoaded == false)
            {
                yield return new WaitForSeconds(0.5f);
                Narratore.text = CurrentChecking;
                yield return new WaitForSeconds(0.5f);
                Narratore.text = CurrentChecking + ".";
                yield return new WaitForSeconds(0.5f);
                Narratore.text = CurrentChecking + "..";
                yield return new WaitForSeconds(0.5f);
                Narratore.text = CurrentChecking + "...";
                yield return new WaitForEndOfFrame();
                StartCoroutine(TextNarrator());
            }
        }

        private void OnAppStateChanged(AppState state)
        {
            // Display the app open ad when the app is foregrounded.
            Debug.Log("App State is " + state);
            if (state == AppState.Foreground)
            {
                appOpenAdManager.ShowAdIfAvailable();
            }
        }

    }
}