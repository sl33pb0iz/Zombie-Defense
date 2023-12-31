using System;
using System.Collections.Generic;
using Unicorn.UI;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public class MoreGamePanel : UICanvas
{
    [SerializeField] private AppInfoItem[] listGames;
    private static AppPromotion data;
    private const string RocketPromoJsonCacheKey = "RocketPromoJsonCache";
    [SerializeField] private string PromoteDefault = "";
    [SerializeField] private TypeMoreGamePanel type;

    public float timeToLoadPromo;

    private string RocketPromoJsonCache
    {
        get => PlayerPrefs.GetString(RocketPromoJsonCacheKey, PromoteDefault);
        set
        {
            PlayerPrefs.SetString(RocketPromoJsonCacheKey, value);
            PlayerPrefs.Save();
        }
    }
    private bool IntenetAvaiable
    {
        get { return Application.internetReachability != NetworkReachability.NotReachable; }
    }
    protected void Start()
    {
        type = TypeMoreGamePanel.EndGame_SceneLevel;

        foreach (var item in listGames) item.gameObject.SetActive(false);
#if UNITY_ANDROID
        int Platform = 0;
#else
        var Platform = 1;
#endif
        var PackageName = ConfigGame.package_name;
        var url =
            $"http://promotion.rocketstudio.com.vn:2688/api/cross_promotion?PackageName={PackageName}&Platform={Platform}";

        if (data == null)
        {
            if (IntenetAvaiable)
                ObservableWWW.Get(url)
                    .CatchIgnore()
                    .Subscribe(x => // onSuccess
                    {
                        //Debug.Log(x);
                        RocketPromoJsonCache = x;
                        OnLoadAdData(RocketPromoJsonCache);
                    }, ex => // onError
                    {
                        //Debug.LogException(ex);
                        OnLoadAdData(PromoteDefault);
                    });
            else
                OnLoadAdData(RocketPromoJsonCache);
        }
        else
        {
            HandleData();
        }

        var changeTime = timeToLoadPromo;
        Observable.Interval(TimeSpan.FromSeconds(changeTime), Scheduler.MainThreadIgnoreTimeScale)
            .Where(isActive => gameObject.activeInHierarchy)
            .Subscribe(_ => { HandleData(); })
            .AddTo(this);
    }

    private void OnLoadAdData(string jsonData)
    {
        //DebugCustom.Log(jsonData);
        if (string.IsNullOrEmpty(jsonData)) return;
        data = JsonUtility.FromJson<AppPromotion>(jsonData);
        HandleData();
    }

    private void HandleData()
    {
        if (data == null || data.App == null || data.App.Length == 0) return;
        var listConfig = new List<PromotionConfig>(data.App);

        for (var i = listConfig.Count - 1; i >= 0; i--)
            if (PackageUtils.CheckInstalled(listConfig[i].PackageName))
                listConfig.RemoveAt(i);

        for (var i = 0; i < listGames.Length; i++)
        {
            if (listConfig.Count == 0) break;

            var Index = Random.Range(0, listConfig.Count);
            var config = listConfig[Index];
            listConfig.RemoveAt(Index);
            listGames[i].gameObject.SetActive(true);
            listGames[i].LoadInfo(config, type);
        }
    }

    public void OnClickBack()
    {
        //UIMainMenuController.Instance.ShowMoreGames(false);
    }
}

public enum TypeMoreGamePanel
{
    SceneMainHome = 0,
    SceneChoose = 1,
    Pause_SceneLevel = 2,
    EndGame_SceneLevel = 3,
    Reborn_SceneLevel = 4
}

[Serializable]
public class AppInfo
{
    public string Id;
    public string AppId { get; set; }
    public string PackageName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconURL { get; set; }
    public int Gold { get; set; }
    public string URL { get; set; }
    public string Display_App { get; set; }
    public int zOrder { get; set; }
}

public class ConfigGame
{
#if UNITY_EDITOR
    public const string package_name = "com.casual.impostor.smasher";
#else
    public const string package_name = "com.casual.impostor.smasher"; // đây là package name của game
#endif
}

public class PackageUtils
{
#if UNITY_IOS
  [DllImport("__Internal")]
 extern static private bool _checkInstalled(string message);
#endif

    // Use this for initialization
    public static bool CheckInstalled(string package)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject launchIntent = null;
        //if the app is installed, no errors. Else, doesn't get past next line
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", package);
            if (launchIntent != null)
                return true;
            else return false;
        }
        catch (Exception)
        {
            return false;
        }
#elif UNITY_IOS
        return _checkInstalled(package);
#else
        return false;
#endif
    }
}