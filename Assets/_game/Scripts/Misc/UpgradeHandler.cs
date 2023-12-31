using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace Unicorn
{
    public class UpgradeHandler : MonoBehaviour
    {
        [HideInInspector] public float timeCountDownUpgrade;
        private readonly float delayBeforeUpgrade = 0.5f;
        public Upgradeable m_Upgradeable;

        private float currentRatioUpgradeProgress;
        [HideInInspector] public bool _canUpgrade;

        [BoxGroup("Show Data")] public string itemName;
        [BoxGroup("Show Data")] public TextMeshProUGUI m_ItemName;
        [BoxGroup("Show Data")] public GameObject m_Index;
        [BoxGroup("Show Data")] public GameObject m_UpgradeBar;
        [BoxGroup("Show Data")] public TextMeshProUGUI m_IndexLevel;
        [BoxGroup("Show Data")] public TextMeshProUGUI m_IndexDamage;
        [BoxGroup("Show Data")] public TextMeshProUGUI m_IndexRange;
        [BoxGroup("Show Data")] public TextMeshProUGUI m_UpgradeCost;
        [BoxGroup("Show Data")] public Image upgradeProgressBar;
        [BoxGroup("Show Data")] public Image imageShow;
        [BoxGroup("Show Data")] public Sprite[] iconTurret;

        [Header("VFX")]
        public GameObject m_PaidUpgradeVFX;

        private void OnEnable()
        {
            timeCountDownUpgrade = delayBeforeUpgrade;
            upgradeProgressBar.fillAmount = m_Upgradeable.ratioUpgrade;
        }

        void ChangeIndex()
        {
            m_ItemName.text = itemName;
            Vector3 direction = Camera.main.transform.forward;
            m_Index.transform.forward = direction;
            m_IndexLevel.text = "LEVEL " + m_Upgradeable.GetCurrentLevels.level.ToString();
            currentRatioUpgradeProgress = m_Upgradeable.ratioUpgrade;
            upgradeProgressBar.fillAmount = Mathf.Lerp(upgradeProgressBar.fillAmount, currentRatioUpgradeProgress, Time.deltaTime * 10);
            m_IndexDamage.text = m_Upgradeable.GetCurrentLevels.damage.ToString();
            m_IndexRange.text = m_Upgradeable.GetCurrentLevels.range.ToString();
            if (m_Upgradeable.GetCurrentLevels.cost > 0)
            {
                m_UpgradeCost.text = m_Upgradeable.GetCurrentLevels.cost.ToString();
            }
            else m_UpgradeCost.text = "MAX";
            if(m_Upgradeable.GetCurrentLevels.level == 1)
            {
                imageShow.sprite = iconTurret[0];
            }
            if (m_Upgradeable.GetCurrentLevels.level == 2)
            {
                imageShow.sprite = iconTurret[1];
            }
            if (m_Upgradeable.GetCurrentLevels.level == 3)
            {
                imageShow.sprite = iconTurret[2];
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == PlayerStateMachine.Instance.m_bodyCollision.gameObject)
            {
                timeCountDownUpgrade = delayBeforeUpgrade;
                m_Index.SetActive(true);
                m_UpgradeBar.SetActive(true);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject == PlayerStateMachine.Instance.m_bodyCollision.gameObject )
            {
                ChangeIndex();
                timeCountDownUpgrade -= Time.deltaTime;
                if (timeCountDownUpgrade <= 0)
                {
                    _canUpgrade = m_Upgradeable.ObjCanUpgrade;
                    {
                        PlayerStateMachine.Instance.m_bodyCollision.canUpgradeTurrret = _canUpgrade;
                        PlayerStateMachine.Instance.m_bodyCollision.activeTurretUpgrade = this;
                        PlayerStateMachine.Instance.m_bodyCollision._turretUpgradePosition = this.gameObject.transform.position;
                    }
                    if ((m_PaidUpgradeVFX))
                    {
                        m_PaidUpgradeVFX.SetActive(true);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                if (other.gameObject == PlayerStateMachine.Instance.m_bodyCollision.gameObject)
                {
                    m_Index.SetActive(false);
                    m_UpgradeBar.SetActive(false);
                    if (m_PaidUpgradeVFX)
                    {
                        m_PaidUpgradeVFX.SetActive(false);
                    }
                    {
                        PlayerStateMachine.Instance.m_bodyCollision.canUpgradeTurrret = false;
                        PlayerStateMachine.Instance.m_bodyCollision.activeTurretUpgrade = null;
                    }
                }
                timeCountDownUpgrade = delayBeforeUpgrade;
                _canUpgrade = false;
            }
        }
    }
}