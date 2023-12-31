using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DataTexture", menuName = "ScriptableObjects/Data Texture")]
public class DataTexture : SerializedScriptableObject
{
    public Sprite IconCoin;
    public Sprite IconCoinLuckyWheel;

    public Sprite IconGem;
    public Sprite IconGemLuckyWheel;

    [SerializeField] private List<Sprite> ListIconSprKey;

    public Sprite GetIconKey(bool isActive)
    {
        int index = isActive ? 0 : 1;
        return ListIconSprKey[index];
    }
}