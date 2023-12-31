using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using Snowyy;
using Snowyy.EquipmentSystem;

public class PlayerSkinChanger : SkinChanger<EquipmentType, int>
{
    private Transform armorTransform;
    private GameObject armor;
    private Transform shoeTransform;
    private GameObject shoe;
    private Transform hatTransform;
    private GameObject hat;
    private Transform backpackTransform;
    private GameObject backpack;
    private Transform weaponTransform;
    private GameObject weapon;

    [HideInInspector] public int weaponActiveId; 
    
    public int TypeHat { get; private set; }

    public int TypeWeapon { get; private set; }

    public int TypeArmor { get; private set; }

    public int TypeShoe { get; private set; }

    public int TypeBackpack { get; private set; }

    public int Number { get; private set; } = -1;

    public int PetId { get; private set; } = -1;

    private void Awake()
    {
        hatTransform = transform.GetChild(0).Find("Armature/body_d/body_u/neck/head/Hat");
        armorTransform = transform.GetChild(0).Find("Armor");
        backpackTransform = transform.GetChild(0).Find("Armature/body_d/body_u/Backpack");
        shoeTransform = transform.GetChild(0).Find("Shoe");
        weaponTransform = transform.GetChild(0).Find("Armature/body_d/body_u/collar.R/arm_u.R/arm_d.R/hand.R/WeaponParentSocket");
    }

    public override void Init()
    {
        InitHat();
        InitShoe();
        InitBackpack();
        InitArmor();
        InitWeapon();
    }

    private void InitHat()
    {
        int hatId = 0;
        var hatobj = EquipmentDataManager.Instance.GetCurrentEquippedEquipment(EquipmentType.Helmet);
        if (hatobj != null)
        {
            hatId = EquipmentDataManager.Instance.GetCurrentEquippedEquipment(EquipmentType.Helmet).itemTemplate.ordinalNumber;
        }
        else hatId = 0;
        ChangeHat(hatId);
    }

    void InitWeapon()
    {
        int weaponId = 0;
        var weaponobj = EquipmentDataManager.Instance.GetCurrentEquippedEquipment(EquipmentType.Weapon);
        if (weaponobj != null)
        {
            weaponId = EquipmentDataManager.Instance.GetCurrentEquippedEquipment(EquipmentType.Weapon).itemTemplate.ordinalNumber;
        }
        else weaponId = 0;
        weaponActiveId = weaponId;
        ChangeWeapon(weaponId);
    }

    void InitArmor()
    {
        int armorId = 0;
        var armorobj = EquipmentDataManager.Instance.GetCurrentEquippedEquipment(EquipmentType.Armor);
        if (armorobj != null)
        {
            armorId = EquipmentDataManager.Instance.GetCurrentEquippedEquipment(EquipmentType.Armor).itemTemplate.ordinalNumber;
        }
        else armorId = 0;
        ChangeArmor(armorId);
    }

    void InitShoe()
    {
        int shoeId = 0;
        var shoeobj = EquipmentDataManager.Instance.GetCurrentEquippedEquipment(EquipmentType.Boot);
        if (shoeobj != null)
        {
            shoeId = EquipmentDataManager.Instance.GetCurrentEquippedEquipment(EquipmentType.Boot).itemTemplate.ordinalNumber;
        }
        else shoeId = 0;
        ChangeShoe(shoeId);
    }

    void InitBackpack()
    {
        int backpackId = 0;
        var backpackobj = EquipmentDataManager.Instance.GetCurrentEquippedEquipment(EquipmentType.Boot);
        if (backpackobj != null)
        {
            backpackId = EquipmentDataManager.Instance.GetCurrentEquippedEquipment(EquipmentType.Boot).itemTemplate.ordinalNumber;
        }
        else backpackId = 0;
        ChangeBackpack(backpackId);
    }

    public override void Change(EquipmentType typeEquipment, int id)
    {
        switch (typeEquipment)
        {
            case EquipmentType.Helmet:
                ChangeHat(id);
                break;
            case EquipmentType.Weapon:
                ChangeWeapon(id);
                break;
            case EquipmentType.Armor:
                ChangeArmor(id);
                break;
            case EquipmentType.Boot:
                ChangeShoe(id);
                break;
            case EquipmentType.Backpack:
                ChangeBackpack(id);
                break;
        }
    }

    public virtual void ChangeHat(int id)
    {
        if (!hatTransform)
        {
            Debug.LogError($"{name} does not have a transform to wear hat!");
            return;
        }

        if (hat != null)
        {
            hat.SetActive(false);
        }

        TypeHat = id;

        if (id < 0)
        {
            id = 0;
            TypeHat = 0;
        }
        hat = hatTransform.GetChild(id).gameObject;
        hat.SetActive(true);
    }

    void ChangeWeapon(int id)
    {
        if (!weaponTransform)
        {
            Debug.LogError($"{name} does not have a transform to wear weapon!");
            return;
        }

        if (weapon != null)
        {
            weapon.SetActive(false);
        }

        TypeWeapon = id;

        if (id < 0)
        {
            id = 0;
            TypeWeapon = 0;
        }

        weapon = weaponTransform.GetChild(id).gameObject;
        weaponActiveId = id; 
        weapon.SetActive(true);
    }

    void ChangeArmor(int id)
    {


        if (!armorTransform)
        {
            Debug.LogError($"{name} does not have a transform to armor weapon!");
            return;
        }

        if (armor != null)
        {
            armor.SetActive(false);
        }

        TypeArmor = id;

        if (id < 0)
        {
            id = 0;
            TypeArmor = 0;
            ;
        }

        armor = armorTransform.GetChild(id).gameObject;
        armor.SetActive(true);
    }

    void ChangeShoe(int id)
    {
        if (!shoeTransform)
        {
            Debug.LogError($"{name} does not have a transform to shoe !");
            return;
        }

        if (shoe != null)
        {
            shoe.SetActive(false);
        }

        TypeShoe = id;

        if (id < 0)
        {
            id = 0;
            TypeShoe = 0;
            ;
        }

        shoe = shoeTransform.GetChild(id).gameObject;
        shoe.SetActive(true);
    }

    void ChangeBackpack(int id)
    {
        if (!backpackTransform)
        {
            Debug.LogError($"{name} does not have a transform to backpacks !");
            return;
        }

        if (backpack != null)
        {
            backpack.SetActive(false);
        }

        TypeBackpack = id;

        if (id < 0)
        {
            id = 0;
            TypeBackpack = 0;
            ;
        }

        backpack = backpackTransform.GetChild(id).gameObject;
        backpack.SetActive(true);
    }
}
