using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;

namespace Unicorn
{
    public class Health : MonoBehaviour
    {
        [Tooltip("Maximum amount of health")] public float maxHealth = 10f;

        private float bonusDefense = 0;

        public bool canPopUp;
        public bool canInvincible;
        public bool canRevive;

        [ShowIfGroup("canPopUp")]
        [FoldoutGroup("canPopUp/Pop up Damage")] public GameObject DamagePopupPrefab;
        [FoldoutGroup("canPopUp/Pop up Damage")] public float popUpTime;
        [FoldoutGroup("canPopUp/Pop up Damage")] public Transform popUpPosition;

        [ShowIfGroup("canInvincible")]
        [FoldoutGroup("canInvincible/Invincible")] public GameObject InvincibleFX;
        [FoldoutGroup("canInvincible/Invincible")] public float InvincibleTime;

        [ShowIfGroup("canRevive")]
        [FoldoutGroup("canRevive/Revive")] public GameObject ReviveFX;


        public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public float CurrentHealth { get; set; }
        public bool Invincible { get; set; }
        public bool CanPickup() => CurrentHealth <= maxHealth;
        public float GetRatio() => CurrentHealth / maxHealth;
        public float Defense { get { return bonusDefense; } set { bonusDefense = value; } }

        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction<float> OnHealed;
        public UnityAction OnDie;

        private bool m_IsDead; 
     
        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            CurrentHealth = maxHealth;
            m_IsDead = false;
        }

        public void Heal(float healAmount)
        {
            float healthBefore = CurrentHealth;
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

            // call OnHeal action
            float trueHealAmount = CurrentHealth - healthBefore;
            if (trueHealAmount > 0f)
            {
                OnHealed?.Invoke(trueHealAmount);
            }

        }

        public void TakeDamage(float damage, GameObject damageSource)
        {
            if (Invincible) return;
            if (m_IsDead) return;
            float healthBefore = CurrentHealth;
            damage -= damage * bonusDefense / 1000;
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

            if (canPopUp)
            {
                CreatePopUp(popUpPosition.position, damage);
            }

            // call OnDamage action
            float trueDamageAmount = healthBefore - CurrentHealth;
            if (trueDamageAmount > 0f)
            {
                OnDamaged?.Invoke(trueDamageAmount, damageSource);

            }

            HandleDeath();
        }

        public void CreatePopUp(Vector3 position, float text)
        {
            var popup = PoolManager.Instance.ReuseObject(DamagePopupPrefab, position, Quaternion.identity);
            var temp = popup.transform.GetChild(0).GetComponent<TextMeshPro>();

            int damage = Mathf.RoundToInt(text);
            temp.text = damage.ToString();
            popup.SetActive(true);
            StartCoroutine(PopUpExist());

            IEnumerator PopUpExist()
            {
                yield return new WaitForSeconds(popUpTime);
                popup.SetActive(false);
            }
        }

        public void Kill()
        {
            CurrentHealth = 0f;

            // call OnDamage action
            OnDamaged?.Invoke(maxHealth, null);


            HandleDeath();
        }

        void HandleDeath()
        {
            if (m_IsDead)
                return;

            // call OnDie action
            if (CurrentHealth <= 0f)
            {
                m_IsDead = true;
                OnDie?.Invoke();
            }
        }

        public void HandleRevive()
        {
            CurrentHealth = maxHealth;
            m_IsDead = false;
            if (ReviveFX)
            {
                GameObject reviveFX = Instantiate(ReviveFX, transform.position, ReviveFX.transform.rotation);
                Destroy(reviveFX, 1f);
            }
            StartCoroutine(IEInvincibleTime(InvincibleTime));
        }

        public IEnumerator IEInvincibleTime(float number)
        {
            Invincible = true;
            if (InvincibleFX)
            {
                InvincibleFX.SetActive(true);
            }
            yield return Yielders.Get(number);
            if (InvincibleFX)
            {
                InvincibleFX.SetActive(false);
            }
            Invincible = false; 
        }
    }
}