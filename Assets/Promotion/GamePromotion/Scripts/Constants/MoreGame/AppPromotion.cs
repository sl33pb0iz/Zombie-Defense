using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[System.Serializable]
public class AppPromotion
{

    [JsonProperty("PackageName")]
    public string PackageName;

    [JsonProperty("Name")]
    public string Name;

    [JsonProperty("Description")]
    public string Description;

    [JsonProperty("Icon")]
    public string Icon;

    [JsonProperty("Bonus")]
    public string Bonus;

    [JsonProperty("Platform")]
    public long Platform;

    [JsonProperty("Url")]
    public string Url;

    [JsonProperty("App")]
    public PromotionConfig[] App;
}

public enum CreativeType
{
    Image,
    Gif,
    Video
}

[System.Serializable]
public class PromotionConfig
{
    [JsonProperty("Version")]
    public int Version;
    
    [JsonProperty("ID")]
    public int Id;


    [JsonProperty("PackageName")]
    public string PackageName;

    [JsonProperty("Name")]
    public string Name;

    [JsonProperty("Description")]
    public string Description;

    [JsonProperty("Icon")]
    public string Icon;

    [JsonProperty("Bonus")]
    public string Bonus;

    [JsonProperty("Platform")]
    public long Platform;

    [JsonProperty("Url")]
    public string Url;

    [JsonProperty("Creative")]
    public string Creative;

    [JsonProperty("Type")]
    public CreativeType Type;

    public string OpenURL { get { return Url; } }
}

