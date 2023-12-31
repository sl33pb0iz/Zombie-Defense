using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using PlayFab;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Firebase.RemoteConfig;
using System.Threading.Tasks;
using UniRx;
using Firebase.Extensions;

//===============================================================
//Developer:  CuongCT
//Company:    ONESOFT
//Date:       2017
//================================================================
public class RocketRemoteConfig
{
    #region Variables

    //private static bool LoadedConfig;
    //private static bool LoadingConfig;
    private static bool isInit;

    private static Dictionary<string, string> playfabConfig = new Dictionary<string, string>();
    public static bool InitSuccess = false;

    #endregion Variables



    #region Public Methods

    public static Task FetchData()
    {
        //Context.Waiting.ShowWaiting("Fetching data...");
        DebugLog("Fetching data...");
        // FetchAsync only fetches new data if the current data is older than the provided
        // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
        // By default the timespan is 12 hours, and for production apps, this is a good
        // number.  For this example though, it's set to a timespan of zero, so that
        // changes in the console will always show up immediately.
        //#if ROCKET_TEST || UNITY_EDITOR
        //        System.Threading.Tasks.Task fetchTask = FirebaseRemoteConfig.FetchAsync(
        //            TimeSpan.FromSeconds(30));
        //#else
        //System.Threading.Tasks.Task fetchTask = FirebaseRemoteConfig.FetchAsync(
        //    TimeSpan.FromHours(6));

        //fetchTask.ContinueWith(FetchComplete);

        Task fetchTask =
        FirebaseRemoteConfig.DefaultInstance.FetchAsync(
           TimeSpan.FromHours(3));
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    public static string GetStringConfig(string key, string defaultValue)
    {
        if (!InitSuccess) return defaultValue;
        if (!FirebaseRemoteConfig.DefaultInstance.Keys.Contains(key)) return defaultValue;
        //if (!defaults.ContainsKey(key)) return defaultValue;
        return FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
    }


    public static bool GetBoolConfig(string key, bool defaultValue)
    {
        if (!InitSuccess) return defaultValue;
        if (!FirebaseRemoteConfig.DefaultInstance.Keys.Contains(key)) return defaultValue;
        return FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;
    }


    public static float GetFloatConfig(string key, float defaultValue)
    {
        if (!InitSuccess) return defaultValue;
        if (!FirebaseRemoteConfig.DefaultInstance.Keys.Contains(key)) return defaultValue;
        string val = FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
        try
        {
            return float.Parse(val);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }


    public static int GetIntConfig(string key, int defaultValue)
    {
        if (!InitSuccess) return defaultValue;
        if (!FirebaseRemoteConfig.DefaultInstance.Keys.Contains(key)) return defaultValue;
        string val = FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
        try
        {
            return int.Parse(val);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }



    public static string PlayfabStringConfig(string key, string defaultValue)
    {
        return playfabConfig.ContainsKey(key) ? playfabConfig[key] : defaultValue;
    }

    public static int PlayfabIntConfig(string key, int defaultVal)
    {
        try
        {
            return playfabConfig.ContainsKey(key) ? int.Parse(playfabConfig[key]) : defaultVal;
        }
        catch (Exception)
        {
            return defaultVal;
        }
    }

    public static float PlayfabFloatConfig(string key, float defaultVal)
    {
        try
        {
            return playfabConfig.ContainsKey(key) ? float.Parse(playfabConfig[key]) : defaultVal;
        }
        catch (Exception)
        {
            return defaultVal;
        }
    }

    public static bool PlayfabBoolConfig(string key, bool defaultVal)
    {
        try
        {
            return playfabConfig.ContainsKey(key) ? int.Parse(playfabConfig[key]) == 1 : defaultVal;
        }
        catch (Exception)
        {
            return defaultVal;
        }
    }

    #endregion Public Methods

    #region Private Methods

    public static void OnLoadConfig(Dictionary<string, string> Data)
    {
        if (Data != null)
        {
            Debug.Log("JsonUtilityOnLoadConfig" + JsonUtility.ToJson(Data));
            playfabConfig = Data;
            //MessageBroker.Default.Publish(new ReloadPlayfabConfig());
            //throw new Exception("ReloadPlayfabConfig");
            //RocketObservable.RocketConfigReload.OnNext(Unit.Default);
            //PlayerPrefs.SetString("CONFIG_CACHED", JsonUtility.ToJson(Data));
        }
    }

    private static void FetchComplete(Task fetchTask)
    {
        GameService.Instance.IsLoadRemoteConfigSucces = true;
        if (fetchTask.IsCanceled)
        {
            DebugLog("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            DebugLog("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            DebugLog("Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                .ContinueWithOnMainThread(task =>
                {
                    DebugLog(String.Format("Remote data loaded and ready (last fetch time {0}).",
                                   info.FetchTime));
                });

                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        DebugLog("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        DebugLog("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                DebugLog("Latest Fetch call still pending.");
                break;
        }

        //AdmobAds.Instance.Init();
    }

    //static Dictionary<string, object> defaults = new Dictionary<string, object>();
    public static void RemoteConfigFirebaseInit()
    {
        if (isInit) return;
        isInit = true;
        //if (GameService.Instance.FirebaseInitialized)
        //{
        //    InitSuccess = true;
        //}

        System.Collections.Generic.Dictionary<string, object> defaults =
            new System.Collections.Generic.Dictionary<string, object>();

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
         .ContinueWithOnMainThread(task => {
            // [END set_defaults]
            DebugLog("RemoteConfig configured and ready!");

             InitSuccess = true;
        });
        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:      
        try
        {
            string config_cached = PlayerPrefs.GetString("CONFIG_CACHED", string.Empty);
            if (!string.IsNullOrEmpty(config_cached))
            {
                playfabConfig = JsonUtility.FromJson<Dictionary<string, string>>(config_cached);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }


    }

    private static void DebugLog(string s)
    {
        Debug.Log(s);
    }

    #endregion Private Methods
}