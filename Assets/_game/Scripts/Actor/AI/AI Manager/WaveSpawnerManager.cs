using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using Unicorn;
using Spicyy.System;
using Spicyy.AI;
using System.Linq;
using System.Threading.Tasks;

public class WaveSpawnerManager : MonoBehaviour
{
    public List<Wave> waves;

    private float waveWaiting = 0;
    private int currWave = -1;
    private new Camera camera;

    private List<GameObject> enemiesToSpawn = new List<GameObject>();
    private AIManager aiManager;

    public void Start()
    {
        aiManager = AIManager.Instance;
        camera = Camera.main;
    }

    private void OnEnable()
    {
        EventManager.AddListener<InitLevelEvent>(OnInitLevel, 0);
        EventManager.AddListener<NewStageStartEvent>(OnNewStageStart);
        EventManager.AddListener<FirstTouchEvent>(OnFirstTouch);
    }

    /// <summary>
    /// Khởi tạo level mới
    /// </summary>
    /// <param name="evt"> Set up số waves sẽ có cho biến static wavesCount dùng cho các component khác gọi tới</param>
    private void OnInitLevel(InitLevelEvent evt)
    {
        evt.wavesCount = waves.Count;
    }
    /// <summary>
    /// Observer pattern bắt sự kiện người chơi chạm vào màn hình, khi đó mới bắt đầu stage đầu tiên
    /// </summary>
    private void OnFirstTouch(FirstTouchEvent evt) => EventManager.Broadcast(Events.NewStageStartEvent);

    /// <summary>
    /// Observer pattern bắt sự kiện bắt đầu một waves mới, set up các UI và các component khác 
    /// </summary>
    /// <param name="evt"></param>
    private void OnNewStageStart(NewStageStartEvent evt)
    {
        if (currWave < waves.Count - 1)
        {
            currWave++;
            evt.currStage = currWave;
            evt.allEnemiesSpawn = false;
            evt.enemiesTotal = enemiesToSpawn.Count;
            Events.EnemySpawnEvent.enemiesSpawned = 0;
            waveWaiting = waves[currWave].waveWaitingTime;
            SoundManager.Instance.PlayWarningSound();
        }
    }

    public void UpdateWaveSpawner()
    {
        CheckInitialEnemy();
    }

    /// <summary>
    /// Kiểm tra xem khi nào thì bắt đầu một wave quái mới
    /// </summary>
    void CheckInitialEnemy()
    {
        if (waveWaiting < 0 && waves[currWave].waveValue > 0 && waves[currWave].hasSpawn == false)
        {
            waves[currWave].hasSpawn = true;
            GenerateEnemies();
        }
        else waveWaiting -= Time.deltaTime;
    }

    /// <summary>
    /// Hoi sinh quai
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemies()
    {
        foreach (GameObject enemyPrefab in enemiesToSpawn)
        {
            // hạn chế số lượng quái gây giật lag game
            while (aiManager.NumberOfEnemyRemaining >= 70)
            {
                yield return null;
            }

            Vector3 position = GetRandomOnMap();
            position.y += enemyPrefab.transform.position.y;
            GameObject enemy = PoolManager.Instance.ReuseObject(enemyPrefab, position, transform.rotation);

            enemy.SetActive(true);
            yield return new WaitForSeconds(1f);
            yield return Yielders.Get(1 / waves[currWave].waveRate);
        }
        Events.NewStageStartEvent.allEnemiesSpawn = true;
        enemiesToSpawn.Clear();
    }

    /// <summary>
    /// Tao danh sach cac quai se xuat hien trong wave
    /// </summary>
    public void GenerateEnemies()
    {
        var generatedEnemies = GenerateEnemiesListAsync();

        foreach (EnemyData enemyData in generatedEnemies)
        {
            enemiesToSpawn.Add(enemyData.enemyPrefab);
        }

        Events.StartSpawnEnemyEvent.enemiesTotal = enemiesToSpawn.Count;
        EventManager.Broadcast(Events.StartSpawnEnemyEvent);
        StartCoroutine(SpawnEnemies());

    }

    /// <summary>
    /// Xu ly bat dong bo sinh quai, tranh gay lag game
    /// </summary>
    /// <returns></returns>
    public IEnumerable<EnemyData> GenerateEnemiesListAsync()
    {
        while (waves[currWave].waveValue > 0)
        {
            EnemyData selectedEnemy = SelectRandomEnemy();
            if (selectedEnemy == null)
            {
                yield break;
            }
            waves[currWave].waveValue -= selectedEnemy.cost;
            yield return selectedEnemy;
        }
    }

    /// <summary>
    /// Lay mot con quai ngau nhien trong danh sach nhung quai se xuat hien o wave
    /// </summary>
    /// <returns></returns>
    private EnemyData SelectRandomEnemy()
    {
        List<EnemyData> availableEnemies = waves[currWave].enemies;
        float totalWeight = 0f;

        // Tính tổng trọng số của tất cả enemy có sẵn
        foreach (EnemyData enemyData in availableEnemies)
        {
            totalWeight += 1f / enemyData.cost;
        }

        if (totalWeight <= 0f)
        {
            return null;
        }

        float randomValue = Random.Range(0f, totalWeight);

        foreach (EnemyData enemyData in availableEnemies)
        {
            if (randomValue < totalWeight)
            {
                return enemyData;
            }
        }
        return null;
    }

    /// <summary>
    /// Lay vi tri sinh quai
    /// </summary>
    /// <returns></returns>
    Vector3 GetRandomOnMap()
    {
        Vector3 randomPoint = GetRandomPointOnNavMesh();
        int trial = 0;

        while (IsPointVisible(randomPoint))
        {
            randomPoint = GetRandomPointOnNavMesh();
            trial++;

            if (trial > 100)
            {
                break;
            }
        }

        return randomPoint;
    }

    /// <summary>
    /// Lay mot vi tri ngau nhien tren navmesh
    /// </summary>
    /// <returns></returns>
    Vector3 GetRandomPointOnNavMesh()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        int randomIndex = Random.Range(0, navMeshData.indices.Length / 3); // Chia cho 3 vì mỗi mặt tam giác có 3 đỉnh
        int triangleIndex = randomIndex * 3;

        Vector3 vertex1 = navMeshData.vertices[navMeshData.indices[triangleIndex]];
        Vector3 vertex2 = navMeshData.vertices[navMeshData.indices[triangleIndex + 1]];
        Vector3 vertex3 = navMeshData.vertices[navMeshData.indices[triangleIndex + 2]];

        Vector3 randomPoint = Vector3.Lerp(vertex1, vertex2, Random.value);
        randomPoint = Vector3.Lerp(randomPoint, vertex3, Random.value);

        return randomPoint;
    }

    /// <summary>
    /// Kiem tra vi tri ngau nhien trong navmesh co xuat hien tren man hinh nguoi choi khong
    /// </summary>
    /// <param name="point">Điểm tham số đầu vào, dựa trên navmesh point tính được</param>
    /// <returns></returns>
    bool IsPointVisible(Vector3 point)
    {
        Vector3 viewportPoint = camera.WorldToViewportPoint(point);
        return (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0);
    }

}
[System.Serializable]
public class EnemyData
{
    public GameObject enemyPrefab;
    public int cost;
}

[System.Serializable]
public class Wave
{
    [BoxGroup("Parameter")] public int waveValue;
    [BoxGroup("Parameter")] public float waveWaitingTime;
    [BoxGroup("Parameter")] public float waveRate;


    [BoxGroup("Enemies")] public List<EnemyData> enemies = new List<EnemyData>();

    [HideInInspector] public bool hasSpawn = false;
}


