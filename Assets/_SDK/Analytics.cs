using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics
{
    public const string tap_to_play = "tap_to_play";
    public const string first_use_joystik = "first_use_joystik";
    public const string collect_coin_lvl_1 = "collect_coin_lvl_1";
    public const string level_win = "level_win_";
    public const string level_lose = "level_lose_";
    public const string level_hider_win = "level_hider_win_";
    public const string level_hider_lose = "level_hider_lose_";

    public const string level_seeker_win = "level_seeker_win_";
    public const string level_seeker_lose = "level_seeker_lose_";

    public const string time_win_level_1 = "time_win_level_1";
    public const string time_lose_level_1 = "time_lose_level_1";
    public const string lvl_collect_key_the_first = "collect_key_first_lvl_";
    public const string lvl_collect_key_the_second = "collect_key_second_lvl_";

    public const string lvl_collect_rain_bow_the_first = "collect_rain_bow_first_lvl_";
    public const string lvl_collect_rain_bow_the_second = "collect_rain_bow_second_lvl_";

    public static void SetUserProperty(string key, string value)
    {
        if (!GameService.Instance.FirebaseInitialized) return;

#if ENABLE_FIREBASE 
        FirebaseAnalytics.SetUserProperty(key, value);
#endif
    }

    public static void LogTapToPlay()
    {
        if (!GameService.Instance.FirebaseInitialized) return;

#if ENABLE_FIREBASE 
        FirebaseAnalytics.LogEvent(tap_to_play);
#endif
    }

    public static void LogFirstLogJoystick()
    {
        if (!GameService.Instance.FirebaseInitialized) return;

#if ENABLE_FIREBASE
        if (PlayerPrefs.GetInt("first_use_joystik", 0) == 0)
        {
            FirebaseAnalytics.LogEvent(first_use_joystik);
            PlayerPrefs.SetInt("first_use_joystik", 1);
        }

#endif
    }

    public static void LogCollectCoinLevel1(int value)
    {
        if (!GameService.Instance.FirebaseInitialized) return;
#if ENABLE_FIREBASE
        FirebaseAnalytics.LogEvent(collect_coin_lvl_1, "coin", value);
#endif
    }

    public static void LogEndGameWin(int lvl)
    {
        if (!GameService.Instance.FirebaseInitialized) return;
#if ENABLE_FIREBASE
        var formatLvl = string.Format("level_win_{0:000}", lvl);
        FirebaseAnalytics.LogEvent(formatLvl);
#endif
    }

    public static void LogEndGameLose(int lvl)
    {
        if (!GameService.Instance.FirebaseInitialized) return;
#if ENABLE_FIREBASE
        var formatLvl = string.Format("level_lose_{0:000}", lvl);
        FirebaseAnalytics.LogEvent(formatLvl);
#endif
    }

    public static void LogTimeWinLevel1(int value)
    {
        if (!GameService.Instance.FirebaseInitialized) return;
#if ENABLE_FIREBASE
        FirebaseAnalytics.LogEvent(time_win_level_1, "time", value);
#endif
    }

    public static void LogTimeLoseLevel1(int value)
    {
        if (!GameService.Instance.FirebaseInitialized) return;
#if ENABLE_FIREBASE
        FirebaseAnalytics.LogEvent(time_lose_level_1, "time", value);
#endif
    }

    public static void LogLevelCollectKeyTheFirst(int lvl)
    {
        if (!GameService.Instance.FirebaseInitialized) return;
#if ENABLE_FIREBASE
        if (PlayerPrefs.GetInt("collect_key_the_first", 0) == 0)
        {
            FirebaseAnalytics.LogEvent(lvl_collect_key_the_first + lvl);
            PlayerPrefs.SetInt("collect_key_the_first", 1);
        }

#endif
    }

    public static void LogLevelCollectKeyTheSecond(int lvl)
    {
        if (!GameService.Instance.FirebaseInitialized) return;
#if ENABLE_FIREBASE

        if (PlayerPrefs.GetInt("collect_key_the_second", 0) == 0 && PlayerPrefs.GetInt("collect_key_the_first", 0) == 1)
        {
            FirebaseAnalytics.LogEvent(lvl_collect_key_the_second + lvl);

            PlayerPrefs.SetInt("collect_key_the_second", 1);
        }
#endif
    }

    public static void LogLevelCollectRainBowTheFirst(int lvl)
    {
        if (!GameService.Instance.FirebaseInitialized) return;
#if ENABLE_FIREBASE
        if (PlayerPrefs.GetInt("collect_key_the_first", 0) == 0)
        {
            FirebaseAnalytics.LogEvent(lvl_collect_rain_bow_the_first + lvl);
            PlayerPrefs.SetInt("collect_key_the_first", 1);
        }

#endif
    }

    public static void LogLevelCollectRainBowTheSecond(int lvl)
    {
        if (!GameService.Instance.FirebaseInitialized) return;
#if ENABLE_FIREBASE

        if (PlayerPrefs.GetInt("collect_rain_bow_the_second", 0) == 0 && PlayerPrefs.GetInt("collect_key_the_first", 0) == 1)
        {
            FirebaseAnalytics.LogEvent(lvl_collect_rain_bow_the_second + lvl);

            PlayerPrefs.SetInt("collect_rain_bow_the_second", 1);
        }
#endif
    }

    public static void LogEventByName(string name)
    {
        if (!GameService.Instance.FirebaseInitialized) return;
#if ENABLE_FIREBASE

        FirebaseAnalytics.LogEvent(name);
#endif
    }

    public static void LogEventWatchVideo(string name)
    {
        if (!GameService.Instance.FirebaseInitialized) return;
#if ENABLE_FIREBASE

         FirebaseAnalytics.LogEvent("event_watch_video", "watch_video", name);
#endif

    }
}
