using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class PlayerBodyCollisionHandler : MonoBehaviour
    {
        public bool canUpgradeTurrret;
        [HideInInspector] public CapsuleCollider m_Collider; 
        [HideInInspector] public Vector3 _turretUpgradePosition;
        [HideInInspector] public UpgradeHandler activeTurretUpgrade;
        public bool NearNode;

        private void Start()
        {
            m_Collider = GetComponent<CapsuleCollider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Node")){
                NearNode = true;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Node"))
            {
                PayMoneyToUpgrade();
            }
        }

        public void PayMoneyToUpgrade()
        {
            if (canUpgradeTurrret && activeTurretUpgrade)
            {
                if (PlayerStateMachine.Instance.timeDelayCountDown > 0) PlayerStateMachine.Instance.timeDelayCountDown -= Time.deltaTime;
                else
                {
                    if (PlayerDataManager.Instance.GetGold() - PlayerStateMachine.Instance.m_CurrencyManager._goldPaidPerTime > 0)
                    {
                        GameObject gold = PoolManager.Instance.ReuseObject(PlayerStateMachine.Instance.m_CurrencyManager._goldPrefab, 
                            PlayerStateMachine.Instance.transform.position, Quaternion.identity);
                        gold.SetActive(true);
                        PlayerStateMachine.Instance.m_CurrencyManager.MoneyPaidToUpgrade(PlayerStateMachine.Instance.m_CurrencyManager._goldPaidPerTime, 
                            gold, PlayerStateMachine.Instance.m_bodyCollision._turretUpgradePosition + new Vector3(0, 2, 0), 
                            PlayerStateMachine.Instance.m_bodyCollision.activeTurretUpgrade);
                        PlayerStateMachine.Instance.timeDelayCountDown = PlayerStateMachine.Instance.reloadPaid;
                    }
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Node")){
                NearNode = false;
            }
        }
    }
}
