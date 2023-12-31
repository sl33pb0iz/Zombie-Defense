using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unicorn
{
    public class Node : MonoBehaviour
    {
        public ShopTurretBuild m_ShopTurret; 

        private GameObject tower;
        public GameObject Tower { get { return tower; } }
        
        private PlayerBuildManager buildManager;
        void Start()
        {
            tower = null;
            buildManager = PlayerStateMachine.Instance.m_BuildManager;
        }

        private void OnEnable()
        {
            m_ShopTurret.ActiveShopTurretUI(false);
        }

        public void BuildTurret()
        {
            if (tower != null)
            {
                return;
            }
            TurretBlueprint turretToBuild = buildManager.GetTurretToBuild();
            CurrencyManager.AddGold(-turretToBuild.cost);
            tower = PoolManager.Instance.ReuseObject(turretToBuild.turretPrefab, transform.position + new Vector3(0, 4f, 0), Quaternion.identity);
            tower.SetActive(true);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject == PlayerStateMachine.Instance.m_bodyCollision.gameObject)
            {
                if (tower == null)
                {
                    buildManager.SetActiveNode(this);
                    m_ShopTurret.ActiveShopTurretUI(true);
                }
                else m_ShopTurret.ActiveShopTurretUI(false);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
           
            if (collision.gameObject == PlayerStateMachine.Instance.m_bodyCollision.gameObject)
            {
                m_ShopTurret.ActiveShopTurretUI(false);
                buildManager.SetActiveNode(null);
            }
        }



    }
}
