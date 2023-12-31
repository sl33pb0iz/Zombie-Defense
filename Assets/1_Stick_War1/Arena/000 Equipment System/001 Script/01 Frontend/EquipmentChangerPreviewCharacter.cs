using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using Sirenix.OdinInspector;
using Snowyy.EquipmentSystem;
using Snowyy;

public class EquipmentChangerPreviewCharacter : SkinChanger<EquipmentType, int>
{
    [SerializeField] protected DataItemTemplate dataItemTemplate;

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

    private float rotationSpeed = 360f;  // Tốc độ quay
    private Vector3 lastMousePosition;  // Vị trí chuột cuối cùng
    private Vector3 currentMousePosition;

    private void Update()
    {
        RotatePreview();   
    }

    public void RotatePreview()
    {
        // Kiểm tra sự kiện bấm chuột hoặc chạm màn hình
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Lưu vị trí chuột hoặc chạm màn hình
            lastMousePosition = Input.mousePosition;
        }
        // Kiểm tra sự kiện giữ chuột hoặc di chuyển chạm màn hình
        else if (Input.GetMouseButton(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Tính toán vị trí di chuyển chuột hoặc chạm màn hình
            currentMousePosition = Input.mousePosition;

            if (currentMousePosition.x > lastMousePosition.x)
            {
                transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }
            else if (currentMousePosition.x < lastMousePosition.x)
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
            // Quay nhân vật xung quanh trục Y

            // Cập nhật vị trí chuột hoặc chạm màn hình cuối cùng
            lastMousePosition = currentMousePosition;
        }
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
        var backpackobj = EquipmentDataManager.Instance.GetCurrentEquippedEquipment(EquipmentType.Backpack);
        if (backpackobj != null)
        {
            backpackId = EquipmentDataManager.Instance.GetCurrentEquippedEquipment(EquipmentType.Backpack).itemTemplate.ordinalNumber;
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
        var hats = dataItemTemplate.dictItemTemplates[EquipmentType.Helmet];
        if (hats.Length == 0)
        {
            Debug.LogError($"{name} has no hats!");
            return;
        }

        if (!hatTransform)
        {
            Debug.LogError($"{name} does not have a transform to wear hat!");
            return;
        }


        if (id > hats.Length)
        {
            Debug.LogError($"{gameObject.name} has invalid hat id {id}");
            id = -1;
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
        var weapons = dataItemTemplate.dictItemTemplates[EquipmentType.Weapon];
        if (weapons.Length == 0)
        {
            Debug.LogError($"{name} has no weapons!");
            return;
        }

        if (!weaponTransform)
        {
            Debug.LogError($"{name} does not have a transform to wear weapon!");
            return;
        }


        if (id >= weapons.Length)
        {
            Debug.LogError($"{gameObject.name} has invalid weapon id {id}");
            id = -1;
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
        weapon.SetActive(true);
    }

    void ChangeArmor(int id)
    {
        var armors = dataItemTemplate.dictItemTemplates[EquipmentType.Armor];
        if (armors.Length == 0)
        {
            Debug.LogError($"{name} has no armors!");
            return;
        }

        if (!armorTransform)
        {
            Debug.LogError($"{name} does not have a transform to armor weapon!");
            return;
        }


        if (id > armors.Length)
        {
            Debug.LogError($"{gameObject.name} has invalid armors id {id}");
            id = -1;
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
        var shoes = dataItemTemplate.dictItemTemplates[EquipmentType.Boot];
        if (shoes.Length == 0)
        {
            Debug.LogError($"{name} has no shoes!");
            return;
        }

        if (!shoeTransform)
        {
            Debug.LogError($"{name} does not have a transform to shoe !");
            return;
        }


        if (id > shoes.Length)
        {
            Debug.LogError($"{gameObject.name} has invalid shoes id {id}");
            id = -1;
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
        var backpacks = dataItemTemplate.dictItemTemplates[EquipmentType.Backpack];
        if (backpacks.Length == 0)
        {
            Debug.LogError($"{name} has no backpacks!");
            return;
        }

        if (!backpackTransform)
        {
            Debug.LogError($"{name} does not have a transform to backpacks !");
            return;
        }


        if (id > backpacks.Length)
        {
            Debug.LogError($"{gameObject.name} has invalid backpacks id {id}");
            id = -1;
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
