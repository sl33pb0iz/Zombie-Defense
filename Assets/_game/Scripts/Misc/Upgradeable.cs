using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace Unicorn
{
    public class Upgradeable : MonoBehaviour
    {
        public List<ObjectUpgrade> levels;
        int _currentlevel = 0;
        private int moneyNeedToUpgrade;
        public ParticleSystem m_ImpactUpgrade;
        GameObject _currentObj;
        public bool ObjCanUpgrade => _currentlevel < levels.Count - 1 ? true : false;
        public ObjectUpgrade GetCurrentLevels { get { return levels[_currentlevel]; } }
        public float ratioUpgrade {  get { return GetCurrentLevels.cost == 0 ? 1 : (GetCurrentLevels.cost - moneyNeedToUpgrade) / (float)(GetCurrentLevels.cost); } }

        private void Start()
        {
            SwitchObject(_currentlevel);
        }

        public void Upgrade(int cost)
        {
            if (_currentlevel < levels.Count - 1)
            {
                if (moneyNeedToUpgrade > 0 && PlayerDataManager.Instance.GetGold() - cost >= 0)
                {
                    moneyNeedToUpgrade -= cost;
                    CurrencyManager.AddGold(-cost);
                    
                }
                else if(moneyNeedToUpgrade == 0)
                {
                    _currentObj.SetActive(false);
                    _currentlevel++;
                    SwitchObject(_currentlevel);
                    if (m_ImpactUpgrade)
                    {

                        m_ImpactUpgrade.Clear();
                        m_ImpactUpgrade.Play();
                    }
                }
            }
        }

        void SwitchObject(int lvl)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (i == lvl)
                {
                    
                    _currentObj = PoolManager.Instance.ReuseObject(levels[i].objPrefab, transform.position, Quaternion.identity);
                    _currentObj.SetActive(true);
                    moneyNeedToUpgrade = levels[lvl].cost;

                }
            }
        }
    }

    [System.Serializable]
    public class ObjectUpgrade
    {
        public GameObject objPrefab;
        public int level;
        public int cost;
        public int damage;
        public int range;
    }
}
