using System;
using System.Reflection;
using UnityEngine;

namespace Spicyy.System
{
    public static class Events
    {
        public static InitLevelEvent InitLevelEvent = new InitLevelEvent();
        public static FirstTouchEvent FirstTouchEvent = new FirstTouchEvent(); 
        public static StartLevelEvent StartLevelEvent = new StartLevelEvent();
        public static PlayerDamagedEvent PlayerDamagedEvent = new PlayerDamagedEvent();
        public static PlayerVibratedEvent PlayerVibratedEvent = new PlayerVibratedEvent();
        public static PlayerChangeEquipmentEvent PlayerChangeEquipmentEvent = new PlayerChangeEquipmentEvent();
        public static PlayerDeathEvent PlayerDeathEvent = new PlayerDeathEvent();
        public static PlayerReviveEvent PlayerReviveEvent = new PlayerReviveEvent();
        public static PlayerUpgradeStatEvent PlayerUpgradeStatEvent = new PlayerUpgradeStatEvent(); 
        public static PlayerCastSkillEvent PlayerCastSkillEvent = new PlayerCastSkillEvent(); 
        public static EnemyDeathEvent EnemyDeathEvent = new EnemyDeathEvent();
        public static EnemySpawnEvent EnemySpawnEvent = new EnemySpawnEvent();
        public static LevelWinEvent LevelWinEvent = new LevelWinEvent();
        public static NewStageStartEvent NewStageStartEvent = new NewStageStartEvent();
        public static ShowInterMidGameEvent ShowInterMidGameEvent = new ShowInterMidGameEvent();
        public static StartSpawnEnemyEvent StartSpawnEnemyEvent = new StartSpawnEnemyEvent();

        public static void ResetStatAllEvent()
        {
            Type eventType = typeof(GameEvent);
            FieldInfo[] eventFields = eventType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in eventFields)
            {
                GameEvent eventInstance = (GameEvent)field.GetValue(null);
                eventInstance.ResetAll();
            }

        }
    }


    public class FirstTouchEvent : GameEvent 
    {
        public override void ResetAll()
        {
        }
    }

    public class InitLevelEvent : GameEvent
    {
        public int wavesCount;


        private const int wavesCountDefault = 0;

        public override void ResetAll()
        {
            wavesCount = wavesCountDefault;
        }
    }

    public class StartLevelEvent : GameEvent
    {
        public override void ResetAll()
        {
        }
    }

    public class StartSpawnEnemyEvent : GameEvent
    {
        public int enemiesTotal;

        private const int enemiesTotalDefault = 0; 
        public override void ResetAll()
        {
            enemiesTotal = enemiesTotalDefault;
        }
    }

    public class EnemySpawnEvent : GameEvent
    {
        public int enemiesSpawned;
        private const int enemiesSpawnedTotalDefault = 0; 
        public override void ResetAll()
        {
            enemiesSpawned = enemiesSpawnedTotalDefault;
        }
    }

    public class EnemyDeathEvent : GameEvent
    {
        public override void ResetAll()
        {
        }
    }

    public class PlayerDamagedEvent : GameEvent
    {
        public override void ResetAll()
        {
        }
    }

    public class PlayerUpgradeStatEvent : GameEvent
    {
        public enum UpgradeStatType
        {
            Health, 
            Defense, 
            Attack,
        }
        public UpgradeStatType upgradeType;
        public override void ResetAll()
        {
        }
    }

    public class PlayerChangeEquipmentEvent : GameEvent
    {
        public override void ResetAll()
        {
        }
    }

    public class PlayerVibratedEvent : GameEvent
    {
        public override void ResetAll()
        {
        }
    }

    public class PlayerCastSkillEvent : GameEvent
    {
        public enum ActivatedSkillType
        {
            Rocket = 0,
            Meteorite = 1,
            Lightning = 2, 
            Tornado = 3,
            Invincible = 4,
        }

        public ActivatedSkillType skillType;
        public override void ResetAll()
        {
        }
    }

    public class PlayerDeathEvent : GameEvent
    {
        public override void ResetAll()
        {
        }
    }

    public class LevelWinEvent : GameEvent
    {
        public override void ResetAll()
        {
        }
    }

    public class PlayerReviveEvent : GameEvent
    {
        public override void ResetAll()
        {
        }
    }

    public class NewStageStartEvent : GameEvent
    {
        public bool allEnemiesSpawn = false; 
        public int currStage;
        public int enemiesTotal;

        public const bool allEnemiesSpawnDefault = false;
        public const int currStageDefault = 0;
        public const int enemiesTotalDefault = 0;

        public override void ResetAll()
        {
            allEnemiesSpawn = allEnemiesSpawnDefault;
            currStage = currStageDefault;
            enemiesTotal = enemiesTotalDefault;
        }
    }

    public class ShowInterMidGameEvent : GameEvent
    {
        public override void ResetAll()
        {
        }
    }

    public class PlayerPickUpEvent : GameEvent
    {
        public override void ResetAll()
        {
        }
    }
}






