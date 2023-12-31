using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Unicorn
{
    /// <summary>
    /// Hỗ trợ cho <see cref="UnicornDataLevel"/>, viết lại nếu cần.
    /// </summary>
    [Serializable]
    public class LevelTypeInfo
    {
        [JsonProperty] private LevelType levelType;
        [JsonProperty] private int currentLevel = 1;
        [JsonProperty] private int maxLevel = int.MinValue;

        private int levelReached; 

        [JsonIgnore]
        public LevelType LevelType
        {
            get => levelType;
            set => levelType = value;
        }
        
        [JsonIgnore]
        public int CurrentLevel
        {
            get => currentLevel;
            set => currentLevel = value;
        }

        [JsonIgnore]
        public int MaxLevel => maxLevel;

        public int LevelReached { get { return levelReached; } set { levelReached = value; } }

        public LevelTypeInfo(LevelType levelType)
        {
            this.levelType = levelType;
        }
        public int IncreaseLevel(LevelConstraint levelConstraint)
        {
            var levelCount = levelConstraint.GetLevelCount(levelType);
            currentLevel++;
            maxLevel = Mathf.Max(maxLevel, currentLevel);
            
            if (maxLevel <= levelCount) return currentLevel;
            
            currentLevel = 1;
            maxLevel = 1;
            return currentLevel;
        }

        public void SetLevel(int level, LevelConstraint levelConstraint)
        {
            var levelCount = levelConstraint.GetLevelCount(levelType);
            currentLevel = Mathf.Clamp(level, 1, levelCount);
        }
    }
}