using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Unicorn
{
    public  class CurrencyManager : MonoBehaviour
    {
        public GameObject _gemPrefab;
        public GameObject _goldPrefab; 
        public int _goldPaidPerTime;
        public static int GetGold()
        {
            return PlayerDataManager.Instance.GetGold();
        }

        public static int GetGem()
        {
            return PlayerDataManager.Instance.GetGem(); 
        }

        public static void AddGold(int goldBonus)
        {
            var playerdata = GameManager.Instance.PlayerDataManager;
            int _count = GetGold() + goldBonus;
            PlayerDataManager.Instance.SetGold(_count);

            if (playerdata.actionUITop != null)
            {
                playerdata.actionUITop(TypeItem.Coin);
            }
        }

        public static void AddGem(int gemBonus)
        {
            var playerdata = GameManager.Instance.PlayerDataManager;
            int _count = GetGem() + gemBonus;
            PlayerDataManager.Instance.SetGem(_count);
            if (playerdata.actionUITop != null)
            {
                playerdata.actionUITop(TypeItem.Gem);
            }
        }

        public void MoneyPaidToUpgrade(int amount, GameObject moneyPaid, Vector3 paidDestination, UpgradeHandler turretNeedUpgrade )
        {
            if(PlayerDataManager.Instance.GetGold() - amount > 0 )
            {
                moneyPaid.transform.DOMove(paidDestination, 0.5f).OnComplete(() => 
                { 
                    turretNeedUpgrade.m_Upgradeable.Upgrade(_goldPaidPerTime); 
                    moneyPaid.SetActive(false); 
                }
                );

                moneyPaid.transform.DOScale(transform.localScale * 1f, 1f);
            }
        }

    }
}
