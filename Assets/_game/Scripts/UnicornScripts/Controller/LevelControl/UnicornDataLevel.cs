using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Unicorn
{
    /// <summary>
    /// Example về implementation của IDataLevel, dùng luôn cũng được.
    /// </summary>
    [Serializable]
    public class UnicornDataLevel : IDataLevel, ILevelInfo
    {
        [JsonProperty] private LevelType levelType = LevelType.Normal;
        [JsonProperty] private int displayLevel = 1;
        [JsonProperty] private Dictionary<LevelType, LevelTypeInfo> levelTypesIndex;

        private List<int> loopLevels = new List<int>();
        private LevelConstraint levelConstraint;

        public LevelConstraint LevelConstraint
        {
            get
            {
                if (levelConstraint == null)
                {
                    Debug.LogError(nameof(levelConstraint) + " is not set, using default values!");
                    levelConstraint = new LevelConstraint();
                }

                return levelConstraint;
            }
            set => levelConstraint = value;
        }

        public LevelType LevelType => levelType;
        public int DisplayLevel => displayLevel;

        [JsonIgnore]
        private Dictionary<LevelType, LevelTypeInfo> LevelTypesIndex
        {
            get
            {
                if (levelTypesIndex.Count < Enum.GetValues(typeof(LevelType)).Length)
                {
                    foreach (LevelType levelType in Enum.GetValues(typeof(LevelType)))
                    {
                        if (levelTypesIndex.ContainsKey(levelType)) continue;
                        levelTypesIndex.Add(levelType, new LevelTypeInfo(levelType));
                    }
                }
                return levelTypesIndex;
            }
            set => levelTypesIndex = value;
        }

        public UnicornDataLevel(LevelConstraint levelConstraint)
        {
            levelTypesIndex = new Dictionary<LevelType, LevelTypeInfo>();
            foreach (LevelType value in Enum.GetValues(typeof(LevelType)))
            {
                levelTypesIndex.Add(value, new LevelTypeInfo(value));
            }
        }
    
        public override string ToString()
        {
            return $"level: {GetCurrentLevel()}, " +
                   $"{nameof(levelType)}: {levelType}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levelType"></param>
        /// <param name="increment">Nếu <code>true</code> thì sẽ chuyển sang <see cref="LevelType"/> và tăng level</param>
        public void SetLevel(LevelType levelType, bool increment = false)
        {
            if (increment)
            {
                LevelTypesIndex[levelType].IncreaseLevel(LevelConstraint);
            }
            this.levelType = levelType;
            Save();
        }
        
        /// <summary>
        /// Chuyển sang <see cref="LevelType"/> và nhảy tới level
        /// </summary>
        /// <param name="levelType"></param>
        /// <param name="level"></param>
        public void SetLevel(LevelType levelType, int level)
        {
            LevelTypesIndex[levelType].SetLevel(level, LevelConstraint);
            this.levelType = levelType;
            Save();
        }

        public void SetLevel(int buildIndex)
        {
            var levelType = levelConstraint.GetLevelTypeFromBuildIndex(buildIndex);
            var levelIndex = levelConstraint.GetLevelIndexFromBuildIndex(buildIndex);
            SetLevel(levelType, levelIndex);
        }

        public int GetBuildIndex()
        {
            int startIndex = LevelConstraint.GetStartIndex(levelType);
            if (startIndex < 1)
            {
                levelType = GetNextLevelType();
                startIndex = LevelConstraint.GetStartIndex(levelType);

                if (startIndex < 1)
                {
                    // No valid scene found!
                    return -1;
                }
            }
        
            int level = GetCurrentLevel();

            return startIndex + level - 1;
        }

        public int GetCurrentLevel() => LevelTypesIndex[levelType].CurrentLevel;

        public void SetCurrentLevel(int level) { LevelTypesIndex[levelType].CurrentLevel = level; }
        public int GetLevelReached() { return LevelTypesIndex[levelType].LevelReached; }
        public void SetLevelReached(int level) { LevelTypesIndex[levelType].LevelReached = level; }

        public void IncreaseLevel()
        {
            displayLevel++;
            
            LevelTypesIndex[levelType].IncreaseLevel(LevelConstraint);

            levelType = GetNextLevelType();        
            Save();
        }


        private LevelType GetNextLevelType()
        {
            int levelTypeIntValue = (int) levelType;
            if (++levelTypeIntValue >= Enum.GetValues(typeof(LevelType)).Length)
            {
                levelTypeIntValue = 0;
            }

            return (LevelType) levelTypeIntValue;
        }

        public void Save()
        {
            PlayerDataManager.Instance.SetDataLevel(this);
        }
    
    }
}