using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using Spicyy.System;
using Unicorn.Utilities;
using UnityEngine.SceneManagement;

namespace Unicorn
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [FoldoutGroup("Parameter")] private UltimateJoystick joystick;
        [FoldoutGroup("Parameter")] public WaveUISystem m_WaveUiSystem;
        [FoldoutGroup("Parameter")] public PopUpInterStartStage m_StageInter;
        [FoldoutGroup("Parameter")] public GameObject parentUiInLevel; 
        [FoldoutGroup("Parameter")] public TutHand tutHand;

        private readonly string levelDescription = "KILL ALL ZOMBIES";
        private int level;

        private void Awake()
        {
            Instance = this;
            joystick = FindObjectOfType<UltimateJoystick>();
            level = SceneManager.GetSceneAt(1).buildIndex - 1;
        }

        private void OnEnable()
        {
            EventManager.AddListener<InitLevelEvent>(OnInitLevel, 1);
            EventManager.AddListener<StartLevelEvent>(OnStartLevel);
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
            EventManager.AddListener<LevelWinEvent>(OnPlayerWin);
            EventManager.AddListener<PlayerReviveEvent>(OnPlayerRevive);
            EventManager.AddListener<ShowInterMidGameEvent>(OnShowMidGameInter);
        }

        void OnInitLevel(InitLevelEvent evt)
        {
            parentUiInLevel.SetActive(false);
            ActiveJoystick(false);
            LevelInfo(level);
            m_WaveUiSystem.m_WaveProgressBar.OnInitLevel(evt);
        }

        void OnStartLevel(StartLevelEvent evt)
        {
            parentUiInLevel.SetActive(true);
            ActiveJoystick(true);
            if (level == 1)
            {
                tutHand.gameObject.SetActive(true);
            }
        }

        void OnPlayerDeath(PlayerDeathEvent evt)
        {
            parentUiInLevel.SetActive(false);
            ResetJoystick();
            ActiveJoystick(false);
        }

        void OnPlayerWin(LevelWinEvent evt)
        {
            parentUiInLevel.SetActive(false);
            ResetJoystick();
            ActiveJoystick(false);
        }

        void OnPlayerRevive(PlayerReviveEvent evt)
        {
            parentUiInLevel.SetActive(true);
            ResetJoystick();
            ActiveJoystick(true); 
        }

        public void OnShowMidGameInter(ShowInterMidGameEvent evt)
        {
            m_StageInter.TogglePanel(Helper.inter_mid_game, () =>
             {
                 GameManager.Instance.Profile.AddGem(50, Helper.inter_mid_game);

             });
        }

        public void LevelInfo(int level)
        {
            GameManager.Instance.UiController.UiMainLobby.txtCurentLevel.text = "LEVEL " + level.ToString();
            GameManager.Instance.UiController.UiMainLobby.txtLevelInfo.text = levelDescription.ToString();
        }

        public void ActiveJoystick(bool value)
        {
            joystick.gameObject.SetActive(value);
        }

        public void ResetJoystick()
        {
            joystick.ResetJS();
        }

        public void ActiveWaveUISystem(bool value)
        {
            m_WaveUiSystem.gameObject.SetActive(value);
        }
    }
}
