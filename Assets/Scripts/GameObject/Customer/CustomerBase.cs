using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;


public class CustomerHitEvent
{
    public CustomerBase Customer { get; private set; }
    public CustomerType Type { get; private set; }
    public int ScoreChange { get; private set; }

    public CustomerHitEvent(CustomerBase customer, CustomerType type, int scoreChange)
    {
        Customer = customer;
        Type = type;
        ScoreChange = scoreChange;
    }

    public static event Action<CustomerHitEvent> OnHitResolved;

    public static void Invoke(CustomerHitEvent e)
    {
        if (OnHitResolved != null)
        {
            OnHitResolved(e);
        }
    }
}

public class StealDetectedEvent
{
    public CustomerBase Detector { get; private set; }

    public StealDetectedEvent(CustomerBase detector)
    {
        Detector = detector;
    }


    public static event Action<StealDetectedEvent> OnDetected;

    public static void Invoke(StealDetectedEvent e)
    {
        if (OnDetected != null)
        {
            OnDetected(e);
        }
    }
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public abstract class CustomerBase : MonoBehaviour, IHittable, IStealable
{
    public CustomerType Type { get; private set; }
    public CustomerRace Race { get; private set; }
    public CustomerState State { get; private set; }
    public bool IsExiting { get;  set; }

    protected NavMeshAgent _agent;
    protected List<Transform> _waypoints = new List<Transform>();
    private int _waypointIndex;
    private bool _isWaiting = false;

    private float _detectionRange;
    private float _detectionAngle;

    private List<ItemData> _inventory = new List<ItemData>();

    protected Animator _anim;

    private static readonly int HashSpeed = Animator.StringToHash("Speed");
    private static readonly int HashHit = Animator.StringToHash("Hit");
    protected static readonly int HashReact = Animator.StringToHash("React");
    protected static readonly int HashExitReact = Animator.StringToHash("ExitReact");


    public void Initialize(CustomerType type, CustomerRace race, float moveSpeed, float detectionRange, float detectionAngle, List<Transform> waypointList)
    {
        Type = type;
        Race = race;

        _detectionRange = detectionRange;
        _detectionAngle = detectionAngle;
        _waypoints = waypointList;

        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _agent.speed = moveSpeed;

        SetState(CustomerState.Walking);
        OnInitialized();
    }

    public void SetInventory(List<ItemData> items)
    {
        _inventory = new List<ItemData>(items);
    }

    protected void SetState(CustomerState newState)
    {
        if (State == newState) return;
        State = newState;
        OnStateChanged(newState);
    }

    protected virtual void OnStateChanged(CustomerState newState)
    {
        switch (newState)
        {
            case CustomerState.Walking:
                _agent.isStopped = false;
                MoveToNextWaypoint();
                break;
            case CustomerState.Idle:
                _agent.isStopped = true;
                if (_anim != null) _anim.SetFloat(HashSpeed, 0f);
                break;
            case CustomerState.Hit:
                _agent.isStopped = true;
                if (_anim != null)  _anim.SetTrigger(HashHit);
                GetComponent<Collider>().enabled = false;

                VFXManager.Instance.PlayVFX(AddressUtil.Prefab.VFX.MagicPoof, transform.position, new Vector3(0, 0f, 0), 1.4f).Forget();
                break;
            case CustomerState.Detected:
                _agent.isStopped = true;
                if (_anim != null)  _anim.SetTrigger(HashReact);
                WaitAndReturnToWalkAsync(2f, CustomerState.Detected).Forget();
                break;
        }
    }

    private void MoveToNextWaypoint()
    {
        if (_waypoints == null || _waypoints.Count == 0) return;

        _waypointIndex = UnityEngine.Random.Range(0, _waypoints.Count);
        _agent.SetDestination(_waypoints[_waypointIndex].position);

        MoveTimeoutAsync().Forget();
    }

    protected virtual void Update()
    {
        if (IsExiting)
        {
            if (_anim != null) _anim.SetFloat(HashSpeed, _agent.velocity.magnitude);
            return;
        }
        if (State != CustomerState.Walking) return;

        if (_anim != null)  _anim.SetFloat(HashSpeed, _agent.velocity.magnitude);

        if (!_isWaiting && !_agent.pathPending && _agent.remainingDistance <= 0.5f)
            IdleThenMoveAsync().Forget();
    }

    private async UniTaskVoid IdleThenMoveAsync()
    {
        _isWaiting = true;
        SetState(CustomerState.Idle);
        await UniTask.WaitForSeconds(UnityEngine.Random.Range(5f, 8f), cancellationToken: this.GetCancellationTokenOnDestroy());
        _isWaiting = false;
        if (IsExiting) return;
        if (State == CustomerState.Idle)
            SetState(CustomerState.Walking);
    }

    private async UniTaskVoid WaitAndReturnToWalkAsync(float seconds, CustomerState waitingState)
    {
        await UniTask.WaitForSeconds(seconds, cancellationToken: this.GetCancellationTokenOnDestroy());
        if (State == waitingState)
            SetState(CustomerState.Walking);
    }

    private async UniTaskVoid MoveTimeoutAsync()
    {
        await UniTask.WaitForSeconds(10f, cancellationToken: this.GetCancellationTokenOnDestroy());

        if (IsExiting) return;

        if (State == CustomerState.Walking && _isWaiting == false)
        {
            MoveToNextWaypoint();
        }
    }

    public void ExitTo(Vector3 exitPosition)
    {
        if (State == CustomerState.Hit) return;
        IsExiting = true;
        _agent.isStopped = false;
        _agent.SetDestination(exitPosition);
        WaitAndDestroyAsync().Forget();
    }

    protected async UniTaskVoid WaitAndDestroyAsync()
    {
        await UniTask.NextFrame();
        await UniTask.WaitUntil(
            () => IsExiting && !_agent.pathPending && _agent.remainingDistance <= 0.1f,
            cancellationToken: this.GetCancellationTokenOnDestroy()
        );
        Destroy(gameObject);
    }

    //ihittable
    public void OnHit() 
    {
        if (State == CustomerState.Hit) return;
        SetState(CustomerState.Hit);
    }

    //istealable
    public List<ItemData> GetStealableItems()
    {
        return new List<ItemData>(_inventory);
    }

    public void OnItemStolen(ItemData item)
    {
        _inventory.Remove(item);
    }

    //도둑질 부분 (감지)
    public bool TryDetectGoat(Transform goat)
    {
        if (State == CustomerState.Hit) return false;

        Vector3 dir = goat.position - transform.position;
        if (dir.magnitude > _detectionRange) return false;

        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > _detectionAngle * 0.5f) return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, dir.normalized, out hit, _detectionRange))
        {
            if (hit.transform != goat) return false;
        }

        SetState(CustomerState.Detected);
        StealDetectedEvent.Invoke(new StealDetectedEvent(this));
        return true;
    }

    protected abstract int GetScoreChange();

    protected virtual void OnInitialized() { }
}

