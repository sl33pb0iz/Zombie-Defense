using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
using Logger = Unicorn.Utilities.Logger;

namespace Unicorn.Examples
{
    public class SkinChangerRoblox : SkinChanger<TypeEquipment, int>
    {
        [SerializeField] private bool canHavePet = true;
        [SerializeField] protected DataTextureSkin dataTextureSkin;
        [SerializeField] private Renderer[] renderers;
        
        [FoldoutGroup("Pet")] [SerializeField] private Vector3 petOffset;
        [FoldoutGroup("Pet")] [SerializeField] private float petMaxDistance = 5;

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
        private Pet pet;

        public event Action<SkinChangerRoblox, Pet> OnNewPetSpawned;

        public int TypeHat { get; private set; }

        public int TypeWeapon { get; private set;  }

        public int TypeArmor { get; private set; }

        public int TypeShoe { get; private set;  }

        public int TypeBackpack { get; private set; }

        public int Number { get; private set; } = -1;

        public int PetId { get; private set; } = -1;

        public Pet Pet => pet;

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
            //InitPet();
            InitWeapon();
        }

        private void Update()
        {
            RotatePreview(); 
        }

        private float rotationSpeed = 360f;  // Tốc độ quay
        private Vector3 lastMousePosition;  // Vị trí chuột cuối cùng
        private Vector3 currentMousePosition;
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

                var hatId = PlayerDataManager.Instance.GetIdEquipSkin(TypeEquipment.Hat);
                ChangeHat(hatId);

        }

        private void InitPet()
        {
          
        }
        void InitWeapon()
        {
           
                var weaponId = PlayerDataManager.Instance.GetIdEquipSkin(TypeEquipment.Weapon);
                ChangeWeapon(weaponId);
         
        }

        void InitArmor()
        {
         
                var armorId = PlayerDataManager.Instance.GetIdEquipSkin(TypeEquipment.Armor);
                ChangeArmor(armorId);
        
        }

        void InitShoe()
        {
           
                var shoeId = PlayerDataManager.Instance.GetIdEquipSkin(TypeEquipment.Shoe);
                ChangeShoe(shoeId);
          
        }

        void InitBackpack()
        {
          
                var backpackId = PlayerDataManager.Instance.GetIdEquipSkin(TypeEquipment.Backpack);
                ChangeBackpack(backpackId);
            
        }

        public override void Change(TypeEquipment typeEquipment, int id)
        {
            switch (typeEquipment)
            {
                case TypeEquipment.Hat:
                    ChangeHat(id);
                    break;
                case TypeEquipment.Pet:
                    ChangePet(id);
                    break;
                case TypeEquipment.Weapon:
                    ChangeWeapon(id);
                    break;
                case TypeEquipment.Armor:
                    ChangeArmor(id);
                    break;
                case TypeEquipment.Shoe:
                    ChangeShoe(id);
                    break;
                case TypeEquipment.Backpack:
                    ChangeBackpack(id);
                    break; 
                case TypeEquipment.Tower:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeEquipment), typeEquipment, null);
            }
        }

        public virtual void ChangeHat(int id)
        {
            var hats = dataTextureSkin.Hats;
            if (hats.Length == 0)
            {
                Logger.LogError($"{name} has no hats!");
                return;
            }

            if (!hatTransform)
            {
                Logger.LogWarning($"{name} does not have a transform to wear hat!");
                return;
            }


            if (id >= hats.Length)
            {
                Logger.LogError($"{gameObject.name} has invalid hat id {id}");
                id = -1;
            }

            if (hat != null)
            {
                hat.SetActive(false);
            }

            TypeHat = id;

            if (id < 0)
            {

                    return;
                ;
            }
            hat = hatTransform.GetChild(id).gameObject;
            hat.SetActive(true);
        }

        public virtual void ChangePet(int id)
        {
            
        }
        void ChangeWeapon(int id)
        {
            var weapons = dataTextureSkin.Weapons;
            if (weapons.Length == 0)
            {
                Logger.LogError($"{name} has no weapons!");
                return;
            }

            if (!weaponTransform)
            {
                Logger.LogWarning($"{name} does not have a transform to wear weapon!");
                return;
            }


            if (id >= weapons.Length)
            {
                Logger.LogError($"{gameObject.name} has invalid weapon id {id}");
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
                ;
            }

            weapon = weaponTransform.GetChild(id).gameObject;
            weapon.SetActive(true);
        }

        void ChangeArmor(int id)
        {
            var armors = dataTextureSkin.Armors;
            if (armors.Length == 0)
            {
                Logger.LogError($"{name} has no armors!");
                return;
            }

            if (!armorTransform)
            {
                Logger.LogWarning($"{name} does not have a transform to armor weapon!");
                return;
            }


            if (id >= armors.Length)
            {
                Logger.LogError($"{gameObject.name} has invalid armors id {id}");
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
            var shoes = dataTextureSkin.Shoes;
            if (shoes.Length == 0)
            {
                Logger.LogError($"{name} has no shoes!");
                return;
            }

            if (!shoeTransform)
            {
                Logger.LogWarning($"{name} does not have a transform to shoe !");
                return;
            }


            if (id >= shoes.Length)
            {
                Logger.LogError($"{gameObject.name} has invalid shoes id {id}");
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
            var backpacks = dataTextureSkin.Backpacks;
            if (backpacks.Length == 0)
            {
                Logger.LogError($"{name} has no backpacks!");
                return;
            }

            if (!backpackTransform)
            {
                Logger.LogWarning($"{name} does not have a transform to backpacks !");
                return;
            }


            if (id >= backpacks.Length)
            {
                Logger.LogError($"{gameObject.name} has invalid backpacks id {id}");
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
   
}