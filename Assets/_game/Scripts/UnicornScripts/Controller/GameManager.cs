using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Spicyy.System;
using System.Diagnostics;

namespace Unicorn
{
    /// <summary>
    /// Quản lý load scene và game state
    /// </summary>
    public partial class GameManager : SerializedMonoBehaviour
    {
        public static GameManager Instance;

        [Space]
        [BoxGroup("Level")]
        [SerializeField] private LevelConstraint levelConstraint;

        [FoldoutGroup("Persistant Component", false)]
        [SerializeField] private UiController uiController;
        [FoldoutGroup("Persistant Component")]
        [SerializeField] private CameraController mainCamera;

        [FoldoutGroup("Persistant Component")]
        [SerializeField] private IapController iap;

        private LevelManager currentLevelManager;
        private TycoonManager tycoonManager; 

        private IDataLevel dataLevel;

        [HideInInspector] public UnityAction GamePaused;
        [HideInInspector] public UnityAction GameResumed;
        [HideInInspector] public UnityAction GameContinue;

        public bool IsLevelLoading { get; private set; }
        public bool IsTycoonLoading {  get; private set;  }

        public ILevelInfo DataLevel => dataLevel;
        public int CurrentLevel => DataLevel.GetCurrentLevel();
        public GameFSM GameStateController { get; private set; }
        public PlayerDataManager PlayerDataManager => PlayerDataManager.Instance;
        public CameraController MainCamera => mainCamera;
        public UiController UiController => uiController;
        public LevelManager LevelManager
        {
            get => currentLevelManager;
            private set => currentLevelManager = value;
        }

        public TycoonManager TycoonManager
        {
            get => tycoonManager;
            private set => tycoonManager = value; 
        }

        public IapController IapController => iap;
        public Profile Profile { get; private set; }

        private void Awake()
        {
#if !UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif
               
            Instance = this;
            GameStateController = new GameFSM(this);
            Profile = new Profile();

            DOTween.Init().SetCapacity(200, 125);
#if FINAL_BUILD
            //Debug.unityLogger.logEnabled = false;
#endif

            dataLevel = PlayerDataManager.GetDataLevel(levelConstraint);
            dataLevel.LevelConstraint = levelConstraint;
        }

        private void Start()
        {

            //UnicornAdManager.Init();
            UiController.Init();
            LoadLevel();

        }

        public void ChangeLevel(int level)
        {
            EventManager.Clear();
            int buildIndex = level + 1;
            
            bool isBuildIndexValid = buildIndex > gameObject.scene.buildIndex
                                     && buildIndex < SceneManager.sceneCountInBuildSettings;
            if (!isBuildIndexValid)
            {
                UnityEngine.Debug.LogError("No valid scene is found! \nFailed build index: " + buildIndex);
                GameStateController.ChangeState(GameState.LOBBY);
                return;
            }

            IsLevelLoading = true;
            dataLevel.SetCurrentLevel(level);
            if (CurrentLevel != 0 && SceneManager.sceneCount != 1)
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));

            SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            uiController.OpenLoading(true);
            uiController.OpenUiMainLobby();
           

        }

        /// <summary>
        /// Load level mới và xóa level hiện tại
        /// </summary>
        public void LoadLevel()
        {
            EventManager.Clear();
            Events.ResetStatAllEvent();
            int buildIndex = dataLevel.GetBuildIndex();

    /*#if UNITY_EDITOR
            buildIndex = GetForcedBuildIndex(buildIndex);
#endif*/

            bool isBuildIndexValid = buildIndex > gameObject.scene.buildIndex 
                                     && buildIndex < SceneManager.sceneCountInBuildSettings;

            if (!isBuildIndexValid)
            {
                GameStateController.ChangeState(GameState.LOBBY);
                return;
            }

            IsLevelLoading = true;
            if (CurrentLevel != 0 && SceneManager.sceneCount != 1)
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));

            SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            uiController.OpenLoading(true);
        }

        public void LoadTycoonScene(int tycoonindex)
        {
            int buildIndex = tycoonindex;

            IsLevelLoading = true;
            if (CurrentLevel != 0 && SceneManager.sceneCount != 1)
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));

            SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            uiController.OpenLoading(true);
        }
    
        /// <summary>
        /// Đưa game về state Lobby và khởi tạo lại các giá trị cần thiết cho mỗi level mới.
        /// <remarks>
        /// LevelManager khi đã load xong thì PHẢI gọi hàm này.
        /// </remarks>
        /// </summary>
        /// <param name="levelManager"></param>
        public void RegisterLevelManager(LevelManager levelManager)
        {
            LevelManager = levelManager;
            GameStateController.ChangeState(GameState.LOBBY);
            uiController.OpenLoading(false);
            IsLevelLoading = false;
        }

        /// <summary>
        /// Đưa game về state Tycoon và khởi tạo lại các giá trị cần thiết .
        /// <remarks>
        /// TycoonManager khi đã load xong thì PHẢI gọi hàm này.
        /// </remarks>
        /// </summary>
        /// <param name="tycoonManager"></param>

        public void RegisterTycoonScene(TycoonManager tycoonManager)
        {
            TycoonManager = tycoonManager;
            GameStateController.ChangeState(GameState.TYCOON);
            uiController.OpenLoading(false);
        }

        /// <summary>
        /// Bắt đầu level, đưa game vào state <see cref="GameState.IN_GAME"/>
        /// </summary>
        public void StartLevel()
        {
            Analytics.LogTapToPlay();
            GameStateController.ChangeState(GameState.IN_GAME);
        }
        
        /// <summary>
        /// Kết thúc game sau một khoảng thời gian
        /// </summary>
        /// <param name="result"></param>
        /// <param name="delayTime"></param>
        public void DelayedEndgame(LevelResult result, float delayTime = 0.5f)
        {
            StartCoroutine(DelayedEndgameCoroutine(result, delayTime));
        }
        
        private IEnumerator DelayedEndgameCoroutine(LevelResult result, float delayTime)
        {
            yield return Yielders.Get(delayTime);
            EndLevel(result);
        }

        /// <summary>
        /// Kết thúc game
        /// </summary>
        /// <param name="result"></param>
        public void EndLevel(LevelResult result)
        {
            GameStateController.ChangeState(GameState.END_GAME);

            if (result == LevelResult.Win )
            {
                if(PlayerDataManager.GetUnlockedLevel(dataLevel.GetCurrentLevel() + 1) == false)
                {
                    IncreaseLevel();
                }
            }
         
        }

        /// <summary>
        /// Tăng level
        /// </summary>
        public void IncreaseLevel()
        {
            dataLevel.SetLevelReached(dataLevel.GetCurrentLevel());
            dataLevel.IncreaseLevel();
        }
        /// <summary>
        /// Hồi sinh
        /// </summary>
        public void Revive()
        {
            LevelManager.ResetLevelState();
            GameStateController.ChangeState(GameState.IN_GAME);
            // TODO: Revive code
        }

        public void Pause()
        {
            Time.timeScale = 0;
            GamePaused?.Invoke();
        }

        public void Resume()
        {
            Time.timeScale = 1;
        }

        private void Update()
        {
            if (!IsLevelLoading)
                GameStateController.Update();
        }

        private void FixedUpdate()
        {
            if (!IsLevelLoading)
                GameStateController.FixedUpdate();
        }

        private void LateUpdate()
        {
            if (!IsLevelLoading)
                GameStateController.LateUpdate();
        }
    }
}

