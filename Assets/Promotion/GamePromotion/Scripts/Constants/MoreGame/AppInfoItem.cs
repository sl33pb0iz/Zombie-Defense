using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.Analytics;
using UnityEngine.Video;

public class AppInfoItem : MonoBehaviour
{
    private string RocketPromoVersionKey
    {
        get
        {
            return "RocketPromoVersion_" + config != null ? config.PackageName : "";
        }
    }

    private int Version
    {
        get
        {
            return PlayerPrefs.GetInt(RocketPromoVersionKey);
        }
        set
        {
            PlayerPrefs.SetInt(RocketPromoVersionKey, value);
            PlayerPrefs.Save();
        }
    }


    public enum State
    {
        None,
        Loading,
        Playing
    }
    public GameObject gAds;
    public RawImage Icon;
    public Text Name;
    public Text Des;

    private PromotionConfig config;

    private VideoPlayer videoPlayer;

    private TypeMoreGamePanel typeMoreGamePanel;

    /// <summary>
    /// Now state
    /// </summary>
    public State nowState
    {
        get;
        private set;
    }

    private void OnEnable()
    {
        //if (nowState != State.Playing)
        //nowState = State.None;

        LoadData();
    }

    private void Awake()
    {
        if (Icon == null)
        {
            Icon = GetComponent<RawImage>();
        }

        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null)
            videoPlayer = gameObject.AddComponent<VideoPlayer>();

        videoPlayer.prepareCompleted -= OnVideoReady;
        videoPlayer.prepareCompleted += OnVideoReady;

    }

    public void LoadInfo(PromotionConfig info, TypeMoreGamePanel _typeMoreGamePanel)
    {
        nowState = State.None;

        config = info;

        //Debug.Log("id: " +  config);
        gAds.SetActive(false);
        Name.text = info.Name;
        Name.gameObject.SetActive(false);
        Icon.gameObject.SetActive(false);
        if (Des != null)
        {
            Des.text = info.Description;
            Des.gameObject.SetActive(false);
        }
        typeMoreGamePanel = _typeMoreGamePanel;

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.Stop();
        }

        LoadData();
    }

    private void LoadData()
    {
        //DebugCustom.Log("Load QC cheo " + config + "  " + nowState);
        if (config == null || nowState != State.None) return;

#if UNITY_EDITOR
        //config.Type = CreativeType.Video;
        //config.Creative = "http://192.168.1.208/resources/BanRuoi_Final/bow.mp4";
#endif
        if (gameObject.activeInHierarchy)
        {
            switch (config.Type)
            {
                case CreativeType.Image:
                    StartCoroutine(LoadImage());
                    break;
                case CreativeType.Video:
                    StartCoroutine(PlayVideo());
                    break;
            }
        }
    }

    private IEnumerator LoadImage()
    {
        //DebugCustom.Log("LoadImage");
        if (config == null || nowState != State.None) yield break;

        nowState = State.Loading;

        string directory = Application.persistentDataPath + @"/AppPromotion/Image";
        string filePath = System.IO.Path.Combine(directory, config.PackageName + config.Id);
        //DebugCustom.Log("LoadImage pathhh " + filePath);

        bool useCached = false;
        if (System.IO.File.Exists(filePath))
        {
            if (Version == config.Version)
                useCached = true;

            //Debug.Log("cache exist Version " + Version);
        }

        if (useCached)
        {
            // Load file
            using (WWW www = new WWW("file:///" + filePath))
            {
                yield return www;

                if (string.IsNullOrEmpty(www.error) == false)
                {
                    Debug.LogError("cache load error.\n" + www.error);
                }
                else
                {
                    Debug.Log("use file from cache");
                    // Set textures
                    OnLoadImageComplete(www.texture);
                }
            }
        }

        DebugCustom.Log("nowState " + nowState);

        if (nowState == State.Playing) yield break;

        if (string.IsNullOrEmpty(config.Creative))
        {
            DebugCustom.LogError("URL is nothing.");
            yield break;
        }

        using (WWW www = new WWW(config.Creative))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error) == false)
            {
                DebugCustom.LogError("File load error.\n" + www.error);
                nowState = State.None;
            }
            else
            {
                DebugCustom.Log("use file from url");
                // Set textures
                OnLoadImageComplete(www.texture);

                try
                {
                    if (Directory.Exists(directory) == false)
                        Directory.CreateDirectory(directory);
                    else
                        File.Delete(filePath);

                    File.WriteAllBytes(filePath, www.bytes);
                    Version = config.Version;
                }
                catch (System.Exception e)
                {
                    DebugCustom.LogError("File write error " + e.ToString());
                }
            }
        }
    }

    private void OnLoadImageComplete(Texture2D texture)
    {
        gAds.SetActive(true);
        if (Des != null)
            Des.gameObject.SetActive(true);
        Name.gameObject.SetActive(true);
        Icon.gameObject.SetActive(true);
        Icon.color = Color.white;

        Icon.texture = texture;
        nowState = State.Playing;
    }

    private IEnumerator PlayVideo()
    {
        DebugCustom.Log("PlayVideo");
        if (videoPlayer == null || config == null || nowState != State.None)
        {
            DebugCustom.LogError("Already loading." + " | " + videoPlayer + " | " + config);
            yield break;
        }

        nowState = State.Loading;

        string directory = Application.persistentDataPath + @"/AppPromotion/Video";
        string filePath = System.IO.Path.Combine(directory, config.PackageName + ".mp4");
        DebugCustom.Log("LoadVideo " + filePath);

        bool useCached = false;
        if (System.IO.File.Exists(filePath))
        {
            if (Version == config.Version)
                useCached = true;

            DebugCustom.Log("cache exist Version " + Version);
        }

        if (useCached)
        {
            DebugCustom.Log("Use cache file " + filePath);
            videoPlayer.url = "file:///" + filePath;
            videoPlayer.Prepare();
            yield break;
        }

        if (string.IsNullOrEmpty(config.Creative))
        {
            DebugCustom.LogError("URL is nothing.");
            yield break;
        }

        DebugCustom.Log("load new file " + config.Creative);
        videoPlayer.url = config.Creative;
        videoPlayer.Prepare();
        while (videoPlayer.isPrepared == false)
            yield return null;

        //cache file
        using (var www = new WWW(config.Creative))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error) == false)
            {
                DebugCustom.LogError("File load error.\n" + www.error);
            }
            else
            {
                try
                {
                    DebugCustom.Log("Save new file " + filePath);
                    if (Directory.Exists(directory) == false)
                        Directory.CreateDirectory(directory);
                    else
                        File.Delete(filePath);

                    File.WriteAllBytes(filePath, www.bytes);
                    Version = config.Version;

                }
                catch (System.Exception e)
                {
                    DebugCustom.LogError("File write error " + e.ToString());
                }
            }
        }
    }

    private void OnVideoReady(VideoPlayer vp)
    {
        DebugCustom.Log("OnVideoReady");
        gAds.SetActive(true);
        nowState = State.Playing;
        vp.playOnAwake = true;
        vp.Play();
        Icon.texture = vp.texture;
        if (Des != null)
            Des.gameObject.SetActive(true);
        Name.gameObject.SetActive(true);
        Icon.gameObject.SetActive(true);
        Icon.color = Color.white;
    }

    public void OnOpenButtonClick()
    {
        if (config != null)
        {
            try
            {
                Application.OpenURL($"{config.OpenURL}&c={ConfigGame.package_name}&af_adset={typeMoreGamePanel}&af_ad={config.Type}");

            }
            catch (Exception e)
            {
                DebugCustom.Log(e + "không có package name làm ơn điền vào");
            }
        }
    }
}
