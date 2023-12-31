using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using Spicyy.System;
using Snowyy;
using Snowyy.EquipmentSystem;

public class ItemsLevelManager : MonoBehaviour
{
    public static ItemsLevelManager Instance;
    [HideInInspector] public List<PickUp> ItemPickups;

    [SerializeField] private int LimitQuantityItemToLoot;

    //public Dictionary<ResourceType, >

    public int NumberOfItem { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public void RegisterItem(PickUp item)
    {
        ItemPickups.Add(item);
        NumberOfItem++;
    }

    public void UnRegisterItem(PickUp item)
    {

        ItemPickups.Remove(item);
        NumberOfItem--;
    }



    
}
