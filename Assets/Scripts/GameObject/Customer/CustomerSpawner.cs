using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> Prefabs_NormalCustomers = new List<GameObject>();
    [SerializeField] private List<GameObject> Prefabs_JerkCustomers = new List<GameObject>();
    [SerializeField] private List<Transform> Transform_SpawnPoints = new List<Transform>();
    [SerializeField] private Transform Transform_ExitPoint;
    [SerializeField] private List<Transform> Transform_Waypoints = new List<Transform>();
    [SerializeField] private int Int_SpawnCount = 3;
    [SerializeField] private float Float_JerkChance = 0.3f;
    [SerializeField] private float Float_SpawnInterval = 2f;
    [SerializeField] private float Float_ExitTime = 30f; 

    private bool _isJerkSuppressed = false;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.Playing)
        {
            SpawnCustomersAsync().Forget();
        }
    }

    private async UniTaskVoid SpawnCustomersAsync()
    {
        await UniTask.NextFrame();
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
            GameObject prefab = Prefabs_JerkCustomers[Random.Range(0, Prefabs_JerkCustomers.Count)];
            GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            JerkCustomer customer = obj.GetComponent<JerkCustomer>();
            if (customer == null) return;

            customer.Initialize(CustomerType.Jerk, CustomerRace.Human, 2f, 5f, 90f, Transform_Waypoints);
        }
        else
        {
            GameObject prefab = Prefabs_NormalCustomers[Random.Range(0, Prefabs_NormalCustomers.Count)];
            GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            NormalCustomer customer = obj.GetComponent<NormalCustomer>();
            if (customer == null) return;

            customer.Initialize(CustomerType.Normal, CustomerRace.Human, 2f, 5f, 90f, Transform_Waypoints);

            ExitAsync(customer).Forget();
        }
    }

    private async UniTaskVoid ExitAsync(CustomerBase customer)
    {
        await UniTask.WaitForSeconds(Float_ExitTime, cancellationToken: this.GetCancellationTokenOnDestroy());
        if (customer == null) return;
        if (customer.State == CustomerState.Hit) return;
        customer.ExitTo(Transform_ExitPoint.position);
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
