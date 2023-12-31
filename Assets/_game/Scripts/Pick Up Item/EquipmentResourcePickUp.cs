using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using Snowyy.EquipmentSystem;
using Snowyy;

public class EquipmentResourcePickUp : PickUp
{
    [Header("Parameters")]
    [Tooltip("Amount of money to add on pickup")]
    public int quantity;
    public ItemInfoPopUp _resourcePopUp;

    public ResourceType resourceType;

    private EquipmentDataManager equipmentDataManager => EquipmentDataManager.Instance;

    protected override void OnPicked(PlayerStateMachine player)
    {
        base.OnPicked(player);
        AddResource(resourceType, quantity);
        PopUp(player.m_UI.transform.position);
        gameObject.SetActive(false);
    }

    private void PopUp(Vector3 position)
    {
        _resourcePopUp.SetUp(quantity.ToString());
        GameObject objPopup = PoolManager.Instance.ReuseObject(_resourcePopUp.gameObject, position, Quaternion.identity);
        objPopup.SetActive(true);
    }

    public void AddResource(ResourceType type, int quantity)
    {
        equipmentDataManager.AddResource(type, quantity);
    }
}
