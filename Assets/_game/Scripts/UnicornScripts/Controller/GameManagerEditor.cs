using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn
{
    public partial class GameManager
    {

#if UNITY_EDITOR
        private enum LevelLoadType
        {
            Normal,
            RepeatOneLevel,
            JumpToLevel
        }

        [BoxGroup("Level")] [SerializeField] private LevelLoadType levelLoadType;

        [HideIf(nameof(levelLoadType), LevelLoadType.Normal)]
        [OnValueChanged(nameof(UpdateEditorValue))]
        [BoxGroup("Level")]
        [SerializeField]
        private bool useBuildIndex;

        [HideIf("@levelLoadType == LevelLoadType.Normal || useBuildIndex")]
        [OnValueChanged(nameof(UpdateEditorValue))]
        [BoxGroup("Level")]
        [SerializeField]
        private LevelType forcedLevelType;

        [HideIf(nameof(levelLoadType), LevelLoadType.Normal)]
        [PropertyRange(nameof(minLevel), nameof(maxLevel))]
        [BoxGroup("Level")]
        [SerializeField]
        private int forcedLevel;

        private int minLevel;
        private int maxLevel;

        private int GetForcedBuildIndex(int buildIndex)
        {
            if (levelLoadType != LevelLoadType.Normal)
            {
                if (useBuildIndex)
                {
                    dataLevel.SetLevel(forcedLevel);
                    buildIndex = forcedLevel;
                }
                else
                {
                    dataLevel.SetLevel(forcedLevelType, forcedLevel);
                    forcedLevelType = dataLevel.LevelType;
                    forcedLevel = dataLevel.GetCurrentLevel();
                    buildIndex = dataLevel.GetBuildIndex();
                }

                if (levelLoadType == LevelLoadType.JumpToLevel)
                {
                    levelLoadType = LevelLoadType.Normal;
                }
            }

            return buildIndex;
        }

        [OnInspectorGUI]
        private void UpdateEditorValue()
        {
            if (levelLoadType == LevelLoadType.Normal)
            {
                return;
            }

            int levelCount = 0;
            if (useBuildIndex)
            {
                levelCount = levelConstraint.GetLevelCount();
                minLevel = levelConstraint.GetStartIndex();
            }
            else
            {
                levelCount = levelConstraint.GetLevelCount(forcedLevelType);
                minLevel = 1;
            }


            maxLevel = levelCount;
            forcedLevel = Mathf.Clamp(forcedLevel, minLevel, maxLevel);
        }
#endif
    }
}