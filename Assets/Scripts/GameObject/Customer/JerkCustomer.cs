using UnityEngine;
using Cysharp.Threading.Tasks;

public class JerkCustomer : CustomerBase
{
    [SerializeField] private int Int_ScoreValue = 100;
    [SerializeField] private float Float_MinReactTime = 3f;
    [SerializeField] private float Float_MaxReactTime = 10f;

    protected override void OnInitialized()
    {
        WaitAndStartReactingAsync().Forget();
    }

    protected override void OnStateChanged(CustomerState newState)
    {
        if (newState == CustomerState.Reacting)
        {
            _agent.isStopped = true;
            if (_anim != null) _anim.SetTrigger(HashReact);
            StartReacting();
            return;
        }
        base.OnStateChanged(newState);
    }

    private async UniTaskVoid WaitAndStartReactingAsync()
    {
        float waitTime = UnityEngine.Random.Range(Float_MinReactTime, Float_MaxReactTime);
        await UniTask.WaitForSeconds(waitTime, cancellationToken: this.GetCancellationTokenOnDestroy());
        if (State == CustomerState.Hit) return;
        if (IsExiting) return;
        SetState(CustomerState.Reacting);
    }

    private void StartReacting()
    {
        switch (Race)
        {
            case CustomerRace.Human:
                DoHumanJerk();
                break;
            case CustomerRace.Animal:
                DoAnimalJerk();
                break;
            case CustomerRace.Alien:
                DoAlienJerk();
                break;
        }
    }

    private void DoHumanJerk() { Debug.Log("고함지르기"); }
    private void DoAnimalJerk() { Debug.Log("물건 엎기"); }
    private void DoAlienJerk() { Debug.Log("이상한 행동"); }

    public void StopReactingAndExit(Vector3 exitPosition)
    {
        if (State == CustomerState.Hit) return;
        // Walking 상태 전환 없이 바로 IsExiting 처리
        IsExiting = true;
        _agent.isStopped = false;
        if (_anim != null) _anim.SetTrigger("Walk"); // Reacting 애니메이션 중단
        _agent.SetDestination(exitPosition);
        WaitAndDestroyAsync().Forget();
    }

    public void StopReacting()
    {
        if (State != CustomerState.Reacting) return;
        SetState(CustomerState.Walking);
    }

    protected override int GetScoreChange()
    {
        return Int_ScoreValue;
    }
}