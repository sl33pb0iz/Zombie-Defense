namespace Unicorn
{
    /// <summary>
    /// Nối scene build index với level
    /// </summary>
    public interface IDataLevel: ILevelInfo
    {
        LevelConstraint LevelConstraint { get; set; }
        void SetLevel(LevelType levelType, bool increment);
        void SetLevel(LevelType levelType, int level);
        void SetLevel(int buildIndex);
        int GetBuildIndex();
        int GetLevelReached();
        void SetLevelReached(int level);
        void IncreaseLevel();
        void Save();
    }
}