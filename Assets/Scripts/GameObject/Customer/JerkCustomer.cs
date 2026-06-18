using UnityEngine;
using Cysharp.Threading.Tasks;

public class JerkCustomer : CustomerBase
{
    [SerializeField] private int Int_ScoreValue = 100;       // 박치기 처치 시 점수
    [SerializeField] private float Float_MinReactTime = 5f;  // 진상짓 발동 최소 대기 시간
    [SerializeField] private float Float_MaxReactTime = 10f; // 진상짓 발동 최대 대기 시간

    protected override void OnInitialized()
    {
        WaitAndStartReactingAsync().Forget();
    }

    private async UniTaskVoid WaitAndStartReactingAsync()
    {
        float waitTime = UnityEngine.Random.Range(Float_MinReactTime, Float_MaxReactTime);
        await UniTask.WaitForSeconds(waitTime, cancellationToken: this.GetCancellationTokenOnDestroy());

        if (State == CustomerState.Hit) return;
        SetState(CustomerState.Reacting);
        StartReacting();
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

    private void DoHumanJerk()
    {
        Debug.Log("고함지르기");
    }

    private void DoAnimalJerk()
    {
        Debug.Log("물건 엎기");
    }

    private void DoAlienJerk()
    {
        Debug.Log("이상한 행동");
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
