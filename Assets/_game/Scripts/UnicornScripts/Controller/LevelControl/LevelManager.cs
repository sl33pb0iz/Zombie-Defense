using UnityEngine;

namespace Unicorn
{
    /// <summary>
    /// Quản lý riêng từng level.
    /// </summary>
    /// <remarks>
    /// Inherit class này rồi đặt vào object của scene Level
    /// </remarks>
    public abstract class LevelManager : MonoBehaviour
    {
        private static LevelManager instance;
        public static LevelManager Instance => instance;

        public LevelResult Result { get; set; }

        protected virtual void Awake()
        {
            instance = this;
        }

        protected virtual void Start()
        {
            SetUpLevelEnvironment();
            GameManager.Instance.RegisterLevelManager(this);
        }

        /// <summary>
        /// Khởi tạo để nối hệ thống chung với hệ thống riêng của mỗi level. 
        /// </summary>
        protected virtual void SetUpLevelEnvironment()
        {
            ResetLevelState();
        }

        /// <summary>
        /// Làm mới level
        /// </summary>
        public virtual void ResetLevelState()
        {
            Result = LevelResult.NotDecided;
        }

        /// <summary>
        /// Khởi tạo những thứ cần thiết và bắt đầu level
        /// </summary>
        public abstract void StartLevel();


        /// <summary>
        /// Nên gọi hàm này để kết thúc level, tránh gọi EndGame ở GameManager
        /// </summary>
        /// <param name="levelResult"></param>
        protected virtual void EndGame(LevelResult levelResult)
        {
            if (Result != LevelResult.NotDecided)
            {
                Debug.LogWarning(
                    $"Level has already ended with result ${Result} but another request for result ${levelResult} is still being sent!");
                return;
            }

            Result = levelResult;
            GameManager.Instance.DelayedEndgame(levelResult);
        }

        public virtual void OnUpdate()
        {
            
        }
        protected virtual void OnDestroy() { }
    }
}
