using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class TrashSpawner : MonoBehaviour
{
    public static TrashSpawner Instance { get; private set; }

    [SerializeField] private List<GameObject> Prefabs_Trash = new List<GameObject>();  
    [SerializeField] private List<Transform> Transform_SpawnPoints = new List<Transform>(); 
    [SerializeField] private float Float_SpawnInterval = 30f;
    [SerializeField] private int Int_MaxTrash = 10;     

    private List<GameObject> _spawnedTrash = new List<GameObject>();
    private List<Transform> _availableSpawnPoints;
    private bool _isSpawning = false;

    private void Start()
    {
        _availableSpawnPoints = new List<Transform>(Transform_SpawnPoints);
        GameManager.Instance.OnCleanSpawn += CleanUpAllTrash;
        GameManager.Instance.OnDayPhaseChanged += OnDayPhaseChanged;
        GameManager.Instance.OnGameStateChanged += OnStateChanged;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.Playing && GameManager.Instance.CurrentDayPhase == DayPhase.Day)
        {
            _isSpawning = true;
            SpawnTrashLoopAsync().Forget();
        }
    }

    private void OnDayPhaseChanged(DayPhase phase)
    {
        if (phase == DayPhase.Day)
        {
            CleanUpAllTrash();
            _isSpawning = true;
            if (!GameManager.Instance.IsPlaying) return;
            SpawnTrashLoopAsync().Forget();
        }
        else if (phase == DayPhase.Night)
        {
            _isSpawning = false;
        }
    }

    private async UniTaskVoid SpawnTrashLoopAsync()
    {
        await UniTask.WaitForSeconds(3f, cancellationToken: this.GetCancellationTokenOnDestroy());
        while (_isSpawning)
        {
            await UniTask.WaitForSeconds(Float_SpawnInterval, cancellationToken: this.GetCancellationTokenOnDestroy());
            if (!_isSpawning) break;
            if (GameManager.Instance.CurrentDayPhase != DayPhase.Day) break;
            if (_spawnedTrash.Count >= Int_MaxTrash) continue;
            if (_availableSpawnPoints.Count == 0) continue;
            SpawnTrash();
        }
    }

    private void SpawnTrash()
    {
        int pointIndex = Random.Range(0, _availableSpawnPoints.Count);
        Transform spawnPoint = _availableSpawnPoints[pointIndex];
        _availableSpawnPoints.RemoveAt(pointIndex);

        GameObject prefab = Prefabs_Trash[Random.Range(0, Prefabs_Trash.Count)];
        GameObject trash = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        _spawnedTrash.Add(trash);
    }

    private void CleanUpAllTrash()
    {
        foreach (GameObject trash in _spawnedTrash)
        {
            if (trash == null) continue;
            Destroy(trash);
        }
        _spawnedTrash.Clear();
        _availableSpawnPoints = new List<Transform>(Transform_SpawnPoints);
    }

    public void RemoveTrash(GameObject trash)
    {
        if (_spawnedTrash.Contains(trash))
        {
            _spawnedTrash.Remove(trash);
            foreach (Transform point in Transform_SpawnPoints)
            {
                if (Vector3.Distance(point.position, trash.transform.position) < 0.1f)
                {
                    _availableSpawnPoints.Add(point);
                    break;
                }
            }
        }
        Destroy(trash);
    }
}
