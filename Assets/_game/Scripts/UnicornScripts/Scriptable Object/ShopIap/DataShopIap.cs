using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "DataShopIap", menuName = "ScriptableObjects/Data Shop Iap")]
public class DataShopIap : SerializedScriptableObject
{
    public Dictionary<IdPack, InfoPackage> dictInfoPackage;

    [PropertyOrder(0)]
    [Button]
    public void AutoSetNameInfoPackage()
    {
        if (dictInfoPackage == null)
            return;

        // Get all keys (IdPack) from the dictionary
        var keys = dictInfoPackage.Keys;

        // Set the name of each IdPack as the key
        foreach (var key in keys)
        {
            dictInfoPackage[key].name = key.ToString();
        }

    }
}

public class InfoPackage
{
    public bool isNew;
    public string name;
    public List<DataElementGift> listRewardPack;

}
[Serializable]
public class DataElementGift
{
    public TypeGift type;
    public int amount;

    public DataElementGift(TypeGift _type, int _amount)
    {
        type = _type;
        amount = _amount;
    }

    public DataElementGift()
    {

    }
}