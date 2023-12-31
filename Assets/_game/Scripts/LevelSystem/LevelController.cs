using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unicorn.UI;
using Unicorn.Utilities;
using Spicyy.System;
using UnityEngine.Events;
using System;
using System.Reflection;
using System.Linq;

namespace Unicorn
{
    public class LevelController : LevelManager
    {
        public enum LevelState
        {
            NotDecided,
            Win,
            Lose,
        }

        private LevelState levelState;

        private PlayerStateMachine playerStateMachine;
        private WaveSpawnerManager waveSpawnerManager;

        private int level;
        private bool isFirstTouch = false;
        private float timerStage;



        
        protected override void Awake()
        {
            base.Awake();
            playerStateMachine = FindObjectOfType<PlayerStateMachine>();
            waveSpawnerManager = FindAnyObjectByType<WaveSpawnerManager>();
            level = SceneManager.GetSceneAt(1).buildIndex - 1;


        }
        private void OnEnable()
        {
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
            EventManager.AddListener<LevelWinEvent>(OnPlayerWin);
            isFirstTouch = false;

        }
        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
            EventManager.RemoveListener<LevelWinEvent>(OnPlayerWin);
        }

        void OnPlayerDeath(PlayerDeathEvent evt) => LoseGame();
        void OnPlayerWin(LevelWinEvent evt) => WinGame();

        protected override void Start()
        {
            base.Start();
            EventManager.Broadcast(Events.InitLevelEvent);
            PlayerDataManager.Instance.SetUnlockedLevel(level);
            Time.timeScale = 1f;
        }

        public override void StartLevel()
        {
            EventManager.Broadcast(Events.StartLevelEvent);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            UpdateCurrentLevelState();
        }

        void ShowInter()
        {
            if (timerStage >= 50)
            {
                timerStage = 0;
                EventManager.Broadcast(Events.ShowInterMidGameEvent);
            }
            else timerStage += Time.deltaTime;
        }

        void UpdateCurrentLevelState()
        {
            if (!isFirstTouch && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
            {
                isFirstTouch = true;
                EventManager.Broadcast(Events.FirstTouchEvent);
            }
            switch (levelState)
            {
                case LevelState.NotDecided:
                    ShowInter();
                    if (waveSpawnerManager.gameObject.activeSelf && isFirstTouch)
                    {
                        waveSpawnerManager.UpdateWaveSpawner();
                    }
                    playerStateMachine.OnUpdate();
                    break;
                case LevelState.Win:
                    timerStage = 0;
                    break;
                case LevelState.Lose:
                    timerStage = 0;
                    break;
            }
        }

        #region level state transition 

        void LoseGame()
        {
            if (levelState == LevelState.NotDecided) levelState = LevelState.Lose;
            EndGame(LevelResult.Lose);
        }

        void WinGame()
        {
            if (levelState == LevelState.NotDecided) levelState = LevelState.Win;
            EndGame(LevelResult.Win);
        }

        #endregion
        protected override void EndGame(LevelResult levelResult)
        {
            base.EndGame(levelResult);
        }

        public override void ResetLevelState()
        {
            base.ResetLevelState();
            if (levelState == LevelState.Lose)
            {
                levelState = LevelState.NotDecided;
                EventManager.Broadcast(Events.PlayerReviveEvent);
                Time.timeScale = 1f;
            }
        }
    }
}
