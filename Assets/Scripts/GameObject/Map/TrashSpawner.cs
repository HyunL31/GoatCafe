using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class TrashSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> Prefabs_Trash = new List<GameObject>();  
    [SerializeField] private List<Transform> Transform_SpawnPoints = new List<Transform>(); 
    [SerializeField] private float Float_SpawnInterval = 30f;
    [SerializeField] private int Int_MaxTrash = 10;     

    private List<GameObject> _spawnedTrash = new List<GameObject>();
    private List<Transform> _availableSpawnPoints;                  

    private void Start()
    {
        _availableSpawnPoints = new List<Transform>(Transform_SpawnPoints);
        SpawnTrashLoopAsync().Forget();
    }

    private async UniTaskVoid SpawnTrashLoopAsync()
    {
        while (true)
        {
            await UniTask.WaitForSeconds(Float_SpawnInterval, cancellationToken: this.GetCancellationTokenOnDestroy());

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
