using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketConfig
{
    public const string GoogleMobileAds_Android = "ca-app-pub-6336405384015455~3057334254";
    public const string Appsflyer_Devkey = "Mza5CYwx7pzKhdhcFcTHdm";
    public const string package_name = "com.htt.zombiee.defense.nh";

    public static string default_cp = "default_bigfox";
    public const string VersionName = "1.0.1";
    public const int versionCode = 20001;
    public const string ProductName = "Zombie Defense: Save the City";
    public const string settingKeyStore = "com.unicorn.block.squid";
    public const string settingLogo = "GAME_ICON";

    public const string settingAliasName = "com.unicorn.block.squid";

    public const string keyaliasPass = "com.unicorn.block.squid";
    public const string keystorePass = "com.unicorn.block.squid";

    public const string MORE_GAME_URI = "http://lovemoney.vn:8080/FatCAt/apps/search/findByType";



#if UNITY_ANDROID
    public const string OPEN_LINK_RATE = "market://details?id=" + package_name;
#else 
    public static string OPEN_LINK_RATE = "itms-apps://itunes.apple.com/app/id"+Apple_App_ID;
#endif

    public const string Apple_App_ID = "1530916159";
    public static string inappAndroidKeyHash = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAt2M0VamyC0bsFIN9KX29ts1P6Ab8LVcToyBbBxHs3rIu1eSUrlj8GSn2Bl4jWuMgDgDNTvk4KO9XuIqnPJSrWBOoi65LT86lirncrGSg6aiAp/jr/Wo35SgvXvOnJhtIDIBbduQGnNGIJTl8lukO8NzeL/TVAEvRPrUym8joGdWiQ/kgjFS2aAUG8V9cs3IAt9hzPwCCCMVuGcU9ZMe0rC/kOtWVFgg3DsRIOkNvkZEXzsNw0ZJZ9g2lqon9VXPzxZh7Bj085n7qF1SrXeQ3BGnQcGuDF/gephGLI+35G8/OkVH0d05GbDI6EgKQK1sL9rDPOJcq3qOJ5Bw+GLKA7wIDAQAB";
}
