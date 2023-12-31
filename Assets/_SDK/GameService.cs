using AppsFlyerSDK;
using Firebase;
using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;

enum VerifyFirebase
{
    Verifying,
    Done,
    Error
}

[Singleton("GameService", true)]
public class GameService : Singleton<GameService>
{
    private VerifyFirebase firebaseReady = VerifyFirebase.Verifying;
    public bool FirebaseInitialized = false;
    public bool IsLoadRemoteConfigSucces = false;
    // Start is called before the first frame update
    void Start()
    {
        InitData();
    }

    private void InitData()
    {

        try
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                DependencyStatus dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    //DebugCustom.LogError("Firebase is ready for use.");
                    firebaseReady = VerifyFirebase.Done;
                }
                else
                {
                    //DebugCustom.LogError("Firebase is not ready for use.");
                    firebaseReady = VerifyFirebase.Error;
                    Debug.LogError("firebase Ready  Error");
                }
            });
        }
        catch (Exception e)
        {
            firebaseReady = VerifyFirebase.Error;
            Debug.LogError("firebase Ready Error: " + e.ToString());
        }

        StartCoroutine(InitFirebase());
    }

    private IEnumerator InitFirebase()
    {
        while (firebaseReady == VerifyFirebase.Verifying)
        {
            yield return null;
        }

        if (firebaseReady == VerifyFirebase.Done)
            InitializeFirebase();
    }

    void InitializeFirebase()
    {
        try
        {
            FirebaseInitialized = true;

            RocketRemoteConfig.RemoteConfigFirebaseInit();
            RocketRemoteConfig.FetchData();

            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            Analytics.SetUserProperty("last_login", DateTime.Now.DayOfYear.ToString());
            Analytics.SetUserProperty("app_version", RocketConfig.versionCode.ToString());
            Analytics.SetUserProperty("current_level", GameManager.Instance.DataLevel.ToString());
            FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));

            AnalyticsRevenueAds.Init();
        }
        catch (Exception e)
        {
            Debug.LogError("Init Firebase Error: " + e.ToString());
            FirebaseInitialized = false;
            IsLoadRemoteConfigSucces = true;
        }
    }
}
