using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Prefab_NormalCustomer;
    [SerializeField] private GameObject Prefab_JerkCustomer;
    [SerializeField] private List<Transform> Transform_SpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> Transform_Waypoints = new List<Transform>();
    [SerializeField] private int Int_SpawnCount = 3;
    [SerializeField] private float Float_JerkChance = 0.3f;
    [SerializeField] private float Float_SpawnInterval = 2f; 

    private bool _isJerkSuppressed = false;

    private void Start()
    {
        SpawnCustomersAsync().Forget();
    }

    private async UniTaskVoid SpawnCustomersAsync()
    {
        for (int i = 0; i < Int_SpawnCount; i++)
        {
            SpawnCustomer();
            await UniTask.WaitForSeconds(Float_SpawnInterval, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
    }

    private void SpawnCustomer()
    {
        Transform spawnPoint = Transform_SpawnPoints[Random.Range(0, Transform_SpawnPoints.Count)];
        bool spawnJerk = !_isJerkSuppressed && Random.value < Float_JerkChance;

        if (spawnJerk)
        {
            GameObject obj = Instantiate(Prefab_JerkCustomer, spawnPoint.position, Quaternion.identity);
            JerkCustomer customer = obj.GetComponent<JerkCustomer>();
            if (customer == null) return;

            customer.Initialize(
                CustomerType.Jerk,
                CustomerRace.Human,
                2f,
                5f,
                90f,
                Transform_Waypoints
            );
        }
        else
        {
            GameObject obj = Instantiate(Prefab_NormalCustomer, spawnPoint.position, Quaternion.identity);
            NormalCustomer customer = obj.GetComponent<NormalCustomer>();
            if (customer == null) return;

            customer.Initialize(
                CustomerType.Normal,
                CustomerRace.Human,
                2f,
                5f,
                90f,
                Transform_Waypoints
            );
        }
    }

    public void ActivateJerkSuppression(float duration)
    {
        JerkSuppressionAsync(duration).Forget();
    }

    private async UniTaskVoid JerkSuppressionAsync(float duration)
    {
        _isJerkSuppressed = true;
        await UniTask.WaitForSeconds(duration, cancellationToken: this.GetCancellationTokenOnDestroy());
        _isJerkSuppressed = false;
    }
}
