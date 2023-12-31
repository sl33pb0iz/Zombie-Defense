using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spicyy.System;
using Unicorn;

namespace Spicyy.AI
{
    public class AIManager : MonoBehaviour
    {
        public static AIManager Instance;
        [HideInInspector] public List<AI> AI;
        [HideInInspector] public List<GameObject> Enemy;

        public int NumberOfAITotal { get; private set; }

        public int NumberOfEnemyRemaining { get; private set; }

        public void Awake()
        {
            Instance = this;
        }

        public void RegisterAI(AI ai)
        {
            AI.Add(ai);
            NumberOfAITotal++;
        }

        public void UnregisterAI(AI ai)
        {

            AI.Remove(ai);
            NumberOfAITotal--;
        }

        public void RegisterEnemy(GameObject m_Enemy)
        {
            Enemy.Add(m_Enemy);
            NumberOfEnemyRemaining++;
            Events.EnemySpawnEvent.enemiesSpawned++;
            EventManager.Broadcast(Events.EnemySpawnEvent);
           
        }

        public void UnRegisterEnemy(GameObject m_Enemy)
        {
            Enemy.Remove(m_Enemy);
            NumberOfEnemyRemaining--;
            EventManager.Broadcast(Events.EnemyDeathEvent);
            if(NumberOfEnemyRemaining == 0 && Events.NewStageStartEvent.allEnemiesSpawn)
            {
                if (Events.InitLevelEvent.wavesCount == Events.NewStageStartEvent.currStage + 1)
                {
                    EventManager.Broadcast(Events.LevelWinEvent);
                }
                else
                {
                    EventManager.Broadcast(Events.NewStageStartEvent);
                }
            }
            
        }
    }
}
