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
    [SerializeField] private float Float_ExitBeforeNight = 7f;

    private bool _isJerkSuppressed = false;
    private bool _isExitingAll = false;
    private bool _hasSpawnedToday = false;
    private List<CustomerBase> _spawnedCustomers = new List<CustomerBase>();

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += OnStateChanged;
        GameManager.Instance.OnDayPhaseChanged += OnDayPhaseChanged;
        GameManager.Instance.OnDayTimeChanged += OnDayTimeChanged;
    }

    private void OnDayPhaseChanged(DayPhase phase)
    {
        if (phase == DayPhase.Day)
        {
            CleanUpAllCustomers();
            _isExitingAll = false;
            _hasSpawnedToday = false;

            if (GameManager.Instance.IsPlaying)
                SpawnCustomersAsync().Forget();
        }
        else if (phase == DayPhase.Night)
        {
            _isExitingAll = true;
        }
    }

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.Playing && GameManager.Instance.CurrentDayPhase == DayPhase.Day && !_hasSpawnedToday)
        {
            _hasSpawnedToday = true;
            SpawnCustomersAsync().Forget();
        }
    }

    private void OnDayTimeChanged(float reaminTime)
    {
        if (GameManager.Instance.CurrentDayPhase != DayPhase.Day) return;
        if (_isExitingAll) return;
        if (reaminTime <= Float_ExitBeforeNight)
        {
            _isExitingAll = true;
            ExitAllCustomers();
        }
    }

    private async UniTaskVoid SpawnCustomersAsync()
    {
        await UniTask.NextFrame();
        for (int i = 0; i < Int_SpawnCount; i++)
        {
            if (_isExitingAll) break;
            if (GameManager.Instance.CurrentDayPhase != DayPhase.Day) break;
            SpawnCustomer();
            await UniTask.WaitForSeconds(Float_SpawnInterval, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
    }

    private void SpawnCustomer()
    {
        if (_isExitingAll) return;

        Transform spawnPoint = Transform_SpawnPoints[Random.Range(0, Transform_SpawnPoints.Count)];
        bool spawnJerk = !_isJerkSuppressed && Random.value < Float_JerkChance;

        if (spawnJerk)
        {
            GameObject prefab = Prefabs_JerkCustomers[Random.Range(0, Prefabs_JerkCustomers.Count)];
            GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            JerkCustomer customer = obj.GetComponent<JerkCustomer>();
            if (customer == null) return;

            customer.Initialize(CustomerType.Jerk, CustomerRace.Human, 2f, 5f, 90f, Transform_Waypoints);
            _spawnedCustomers.Add(customer);
        }
        else
        {
            GameObject prefab = Prefabs_NormalCustomers[Random.Range(0, Prefabs_NormalCustomers.Count)];
            GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            NormalCustomer customer = obj.GetComponent<NormalCustomer>();
            if (customer == null) return;

            customer.Initialize(CustomerType.Normal, CustomerRace.Human, 2f, 5f, 90f, Transform_Waypoints);

            _spawnedCustomers.Add(customer);
            ExitAsync(customer).Forget();
        }
    }

    private void ExitAllCustomers()
    {
        foreach (CustomerBase customer in _spawnedCustomers)
        {
            if (customer == null) continue;
            if (customer.State == CustomerState.Hit) continue;
            if (customer.IsExiting) continue;

            JerkCustomer jerk = customer as JerkCustomer;
            if (jerk != null)
            {
                jerk.StopReactingAndExit(Transform_ExitPoint.position);
                continue;
            }
            customer.ExitTo(Transform_ExitPoint.position);
        }
        _spawnedCustomers.Clear();
    }

    private void CleanUpAllCustomers()
    {
        foreach (CustomerBase customer in _spawnedCustomers)
        {
            if (customer == null) continue;
            Destroy(customer.gameObject);
        }
        _spawnedCustomers.Clear();

        CustomerBase[] remaining = FindObjectsByType<CustomerBase>(FindObjectsSortMode.None);
        foreach (CustomerBase customer in remaining)
        {
            Destroy(customer.gameObject);
        }
    }

    private async UniTaskVoid ExitAsync(CustomerBase customer)
    {
        await UniTask.WaitForSeconds(Float_ExitTime, cancellationToken: this.GetCancellationTokenOnDestroy());
        if (customer == null) return;
        if (customer.State == CustomerState.Hit) return;
        if (customer.IsExiting) return;
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
