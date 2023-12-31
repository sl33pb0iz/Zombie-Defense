namespace Unicorn
{
    /// <summary>
    /// Example về implementation của <see cref="ILevelLoader"/>, dùng luôn cũng được.
    /// </summary>
    public class UnicornLevelLoader: ILevelLoader
    {
        public void LoadLevel(IDataLevel dataLevel)
        {
            
        }
    }

    /// <summary>
    /// Interface load level
    /// </summary>
    public interface ILevelLoader
    {
        public void LoadLevel(IDataLevel dataLevel);
    }
}