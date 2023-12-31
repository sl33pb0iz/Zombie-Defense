using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Snowyy;
using Arena;

namespace Unicorn
{
    public class ShopTurretBuild : MonoBehaviour
    {
        public TurretBlueprint[] TurretData;

        private void Awake()
        {
            SurvivorShopDataManager.Instance.SetItemTowerUnlock(TowerTypeShop.Machine, true);
            SurvivorShopDataManager.Instance.SetItemTowerUnlock(TowerTypeShop.TeslaCoil, true);
         
        }

        private void OnEnable()
        {
            foreach(TowerTypeShop type in Enum.GetValues(typeof(TowerTypeShop)))
            {
                if (SurvivorShopDataManager.Instance.GetItemTowerUnlocked(type))
                {
                    TurretData[(int)type].button.gameObject.SetActive(true);
                }
                else
                {
                    TurretData[(int)type].button.gameObject.SetActive(false);
                }
            }
        }

        public void ActiveShopTurretUI(bool value)
        {
            gameObject.SetActive(value); 
        }

        public void PurchaseCanonPrefab()
        {
            PlayerStateMachine.Instance.m_BuildManager.BuildTurretOn(TurretData[0]);
            SoundManager.Instance.PlaySoundButton();
        }

        public void PurchaseTeslaCoil()
        {
            PlayerStateMachine.Instance.m_BuildManager.BuildTurretOn(TurretData[1]);
            SoundManager.Instance.PlaySoundButton();
        }

        public void PurchasePlasma()
        {
            PlayerStateMachine.Instance.m_BuildManager.BuildTurretOn(TurretData[4]);
            SoundManager.Instance.PlaySoundButton();
        }

        public void PurchaseRocketPlatform()
        {
            PlayerStateMachine.Instance.m_BuildManager.BuildTurretOn(TurretData[3]);
            SoundManager.Instance.PlaySoundButton();
        }

        public void PurchaseFlamethrower()
        {
            PlayerStateMachine.Instance.m_BuildManager.BuildTurretOn(TurretData[2]);
            SoundManager.Instance.PlaySoundButton();
        }
    }

    [System.Serializable]
    public class TurretBlueprint
    {
        public string turretName;
        public GameObject turretPrefab;
        public int cost;
        public GameObject button; 
    }
}
