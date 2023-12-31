
namespace Unicorn
{
        /// <summary>
        /// Thông tin về level
        /// </summary>
        public interface ILevelInfo
        {
                LevelType LevelType { get; }
                int DisplayLevel { get; }
                int GetCurrentLevel();
                void SetCurrentLevel(int level);
        }

}