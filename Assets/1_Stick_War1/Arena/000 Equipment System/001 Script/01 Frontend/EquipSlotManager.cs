using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unicorn;
using Unicorn.UI;
using UnityEngine.Events;
using Spicyy.System;

namespace Snowyy.EquipmentSystem
{
    public class EquipSlotManager : MonoBehaviour
    {
        [Title("TEXT", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] private TextMeshProUGUI txtTotalAtk;
        [SerializeField] private TextMeshProUGUI txtTotalHp;
        [SerializeField] private TextMeshProUGUI txtUsername;

        [Title("HOLDERS", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] private ItemEquippedSlot[] arrItemEquippedSlots;
        [SerializeField] private EquipmentType[] arrEquipmentTypeAtk;
        [SerializeField] private EquipmentType[] arrEquipmentTypeHp;

        [Title("BUTTON", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] private Button btnChangeName;

        private PlayerDataManager playerDataManager => PlayerDataManager.Instance;

        private OnRefreshDisplay onRefreshDisplay;

        private void Awake()
        {
            onRefreshDisplay += DisplayTotalStats;
        }

        private void OnEnable()
        {
            GameManager.Instance.UiController.UiPopupUsername.ChangeUsernameAction += DisplayUsername;
            btnChangeName.onClick.AddListener(OnChangeUsername);
        }

        private void OnDisable()
        {
            GameManager.Instance.UiController.UiPopupUsername.ChangeUsernameAction -= DisplayUsername;
            btnChangeName.onClick.RemoveListener(OnChangeUsername);
        }

        private void OnChangeUsername()
        {
            PlayerDataManager.Instance.SetChangeableUsername(true);
            GameManager.Instance.UiController.OpenUiUsername();
            PlayerDataManager.Instance.SetChangeableUsername(false);
        }

        public void Init()
        {
            for (int i = 0; i < arrItemEquippedSlots.Length; i++)
            {
                arrItemEquippedSlots[i].Init(EquipmentDataManager.Instance.GetCurrentEquippedEquipment((EquipmentType)i));
            }
            onRefreshDisplay?.Invoke();
        }

        public void InitEquip(EquipmentType type)
        {
            arrItemEquippedSlots[(int)type].Init(EquipmentDataManager.Instance.GetCurrentEquippedEquipment(type));
            onRefreshDisplay?.Invoke();
        }

        public void InitUnequip(EquipmentType type)
        {
            arrItemEquippedSlots[(int)type].Init();
            onRefreshDisplay?.Invoke();
        }

        public void DisplayTotalStats()
        {
            DisplayUsername();
            DisplayTotalAtk();
            DisplayTotalHp();
        }

        private void DisplayUsername()
        {
            txtUsername.text = PlayerDataManager.Instance.GetUsername();
        }

        private void DisplayTotalAtk()
        {
            PlayerDataManager playerdata = PlayerDataManager.Instance;
            var currentLevel = playerdata.DataPlayerStat.GetPlayerStatLevel(PlayerUpgradeStatEvent.UpgradeStatType.Attack);
            int totalAtk = (int)playerdata.DataPlayerStat.GetPlayerStatAtLevel(PlayerUpgradeStatEvent.UpgradeStatType.Attack, currentLevel);
            for (int i = 0; i < arrEquipmentTypeAtk.Length; i++)
            {
                totalAtk += arrItemEquippedSlots[(int)arrEquipmentTypeAtk[i]].GetStat();
            }
            txtTotalAtk.text = $"{totalAtk}";

            playerDataManager.DataPlayerStat.SetTotalATK(totalAtk);
            Debug.Log(playerDataManager.DataPlayerStat.GetTotalATK());
        }

        private void DisplayTotalHp()
        {
            PlayerDataManager playerdata = PlayerDataManager.Instance;
            var currentLevel = playerdata.DataPlayerStat.GetPlayerStatLevel(PlayerUpgradeStatEvent.UpgradeStatType.Health);
            int totalHp = (int)playerdata.DataPlayerStat.GetPlayerStatAtLevel(PlayerUpgradeStatEvent.UpgradeStatType.Health, currentLevel); ;
            for (int i = 0; i < arrEquipmentTypeHp.Length; i++)
            {
                totalHp += arrItemEquippedSlots[(int)arrEquipmentTypeHp[i]].GetStat();
            }
            txtTotalHp.text = $"{totalHp}";

            playerDataManager.DataPlayerStat.SetTotalHealth(totalHp);

        }
    }
}
