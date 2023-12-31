using System;
using System.Collections.Generic;
using Unicorn.Examples;
using UniRx.Triggers;
using UnityEngine;

namespace Unicorn.Examples
{
    public class ShopSkinChangerSample: SkinChangerRoblox
    {
        [SerializeField] private Transform transPets;

        private Dictionary<int, Pet> pets = new Dictionary<int, Pet>();
        private static readonly int Action = Animator.StringToHash("Action");

        public override void ChangePet(int id)
        {
            base.ChangePet(id);
            if (id < 0)
                return;

            Pet.CanMove = false;
            Transform petTransform = Pet.transform;
            petTransform.SetParent(transPets);
            petTransform.localPosition = Vector3.zero;
            petTransform.localRotation = Quaternion.identity;
        }

        private void OnEnable()
        {
            MakePetDoActionRepeating();
            Init();
        }

        public void MakePetDoActionRepeating()
        {
            CancelInvoke();
            InvokeRepeating(nameof(MakePetDoAction), 10f, 5f);
        }

        public void MakePetDoAction()
        {
            if (Pet)
            {
                Pet.Animator.SetTrigger(Action);
            }
        }
    }
}