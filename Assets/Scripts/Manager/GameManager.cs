using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public enum GameState
{
    None = 0,
    Loading,
    Ready,
    Playing,
    Pause,
    Home,
    GameOver
}

public enum DayPhase
{
    None = 0,
    Day,
    Night
}

public enum EndingType
{
    None = 0,
    BadEnding,
    HappyEnding
}

public class GameManager : BaseMonoManager<GameManager>
{
    // 300f
    [SerializeField] private float _dayDuration = 50f;
    [SerializeField] private float _nightDuration = 50f;
    [SerializeField] private int _maxDayCount = 3;

    [SerializeField] private int _targetCoin = 1000;
    [SerializeField] private int _targetStolenItemCount = 6;

    private float _remainDayTime;
    private GameState _beforeGameState;

    public GameState CurrentState { get; private set; } = GameState.None;
    public DayPhase CurrentDayPhase { get; private set; } = DayPhase.None;

    public string CurrentDialogueID { get; private set; }

    public int CurrentDay => SaveManager.Instance.CurrentPlayerModel.Day;
    public int CurrentCoin => SaveManager.Instance.CurrentPlayerModel.Coin;
    public int CurrentStolenItemCount => SaveManager.Instance.CurrentPlayerModel.StolenItemCount;
    public bool IsPlaying => CurrentState == GameState.Playing;
    public bool IsPaused => CurrentState == GameState.Pause;
    public bool IsGameOver => CurrentState == GameState.GameOver;
    public float RemainDayTime => _remainDayTime;
    public float DayDuration => _dayDuration;
    public float NightDuration => _nightDuration;
    
    public float DayTimeRate 
    {
        get
        {
            if (_remainDayTime <= 0f)
                return 0f;

            if (CurrentDayPhase == DayPhase.Day)
            {
                return _remainDayTime / _dayDuration;
            }
            else if(CurrentDayPhase == DayPhase.Night)
            {
                return _remainDayTime / _nightDuration;
            }
            else
            {
                return 0f;
            }
        }
    }
    public float NightTimeRate => _nightDuration <= 0f ? 0f : (_remainDayTime / _nightDuration);

    public event Action<EndingType> OnEndingDetermined;
    public event Action<GameState> OnGameStateChanged;
    public event Action<DayPhase> OnDayPhaseChanged;
    public event Action<float> OnDayTimeChanged;
    public event Action<int> OnDayChanged;

    public event Action OnMoveHome;
    public Action<int> OnChangedStamina;

    public event Action<int> OnUseStaminaItem;
    public event Action<float> OnUseSpeedItem;

    protected override void Awake()
    {
        base.Awake();
        CurrentDialogueID = "Opening_01";
    }

    private void Start()
    {
        InitializeGame();
    }

    private void FixedUpdate()
    {
        UpdateDayTime();
    }

    private void InitializeGame()
    {
        ChangeGameState(GameState.Loading);

        _remainDayTime = _dayDuration;
        ChangeDayPhase(DayPhase.Day);

        ChangeGameState(GameState.Ready);
    }

    // Playing 상태로 전환
    public void StartGame()
    {
        if (CurrentState != GameState.Ready)
        {
            return;
        }

        Time.timeScale = 1f;
        StartDay();
        ChangeGameState(GameState.Playing);
    }

    // 낮 시간 초기화, 낮으로 전환
    public void StartDay()
    {
        _remainDayTime = _dayDuration;
        ChangeDayPhase(DayPhase.Day);
        OnDayTimeChanged?.Invoke(_remainDayTime);
    }

    public void StartNight()
    {
        _remainDayTime = _nightDuration;
        ChangeDayPhase(DayPhase.Night);
        OnDayTimeChanged?.Invoke(_remainDayTime);
    }

    // Pause(일시정지) 상태로 전환
    public void PauseGame()
    {
        if (CurrentState == GameState.Playing || CurrentState == GameState.Home)
        {
            _beforeGameState = CurrentState;
            Time.timeScale = 0f;
            ChangeGameState(GameState.Pause);
        }
    }

    // Pause > Playing 상태로 전환
    public void ResumeGame()
    {
        if (CurrentState != GameState.Pause)
        {
            return;
        }

        Time.timeScale = 1f;
        ChangeGameState(_beforeGameState);
        _beforeGameState = GameState.None;
    }

    // GameOver 상태로 전환
    public void EndGame()
    {
        if (CurrentState == GameState.GameOver)
        {
            return;
        }

        Time.timeScale = 1f;
        ChangeGameState(GameState.GameOver);
    }



    public void ReadyGame()
    {
        Time.timeScale = 0f;
        ChangeGameState(GameState.Ready);
    }

    private void ChangeGameState(GameState gameState)
    {
        if (CurrentState == gameState)
        {
            return;
        }

        CurrentState = gameState;

        Debug.Log($"게임 상태 변경됨 > {CurrentState}");
        OnGameStateChanged?.Invoke(CurrentState);
    }

    private void ChangeDayPhase(DayPhase dayPhase)
    {
        if (CurrentDayPhase == dayPhase)
        {
            return;
        }

        CurrentDayPhase = dayPhase;

        Debug.Log($"낮밤 변경됨 > {CurrentDayPhase}");
        OnDayPhaseChanged?.Invoke(CurrentDayPhase);
    }

    private void FinishNight()
    {
        ChangeDayPhase(DayPhase.None);
        ChangeGameState(GameState.Home);

        OnMoveHome?.Invoke();
    }

    private void UpdateDayTime()
    {
        if (!IsPlaying)
        {
            return;
        }

        if (CurrentDayPhase == DayPhase.None)
        {
            return;
        }

        _remainDayTime -= Time.fixedDeltaTime;
        _remainDayTime = Mathf.Max(_remainDayTime, 0f);

        OnDayTimeChanged?.Invoke(_remainDayTime);

        if (_remainDayTime <= 0f)
        {
            if (CurrentDayPhase == DayPhase.Day)
            {
                StartNight();
            }
            else if (CurrentDayPhase == DayPhase.Night)
            {
                FinishNight();
            }
        }
    }

    public void NextDay()
    {
        if (CurrentDay >= _maxDayCount)
        {
            DetermineEnding();
            EndGame();
            return;
        }

        SaveManager.Instance.CurrentPlayerModel.Day++;
        SaveManager.Instance.SaveData();

        OnDayChanged?.Invoke(SaveManager.Instance.CurrentPlayerModel.Day);

        Time.timeScale = 1f;
        ChangeGameState(GameState.Playing);
        StartDay();
    }

    public EndingType GetEndingType()
    {
        bool isCoinSuccess = CurrentCoin >= _targetCoin;
        bool isStolenSuccess = CurrentStolenItemCount >= _targetStolenItemCount;

        if (isCoinSuccess && isStolenSuccess)
        {
            SaveManager.Instance.CurrentPlayerModel.Ending = "Happy_Ending";
            CurrentDialogueID = "Happy_Ending_01";
            return EndingType.HappyEnding;
        }

        SaveManager.Instance.CurrentPlayerModel.Ending = "Bad_Ending";
        CurrentDialogueID = "Bad_Ending_01";
        return EndingType.BadEnding;
    }

    public void DetermineEnding()
    {
        EndingType endingType = GetEndingType();

        Debug.Log($"엔딩 > {endingType}");

        OnEndingDetermined?.Invoke(endingType);
        SaveManager.Instance.SaveData();
        UIManager.Instance.OpenDialogueUI().Forget();
    }

    public void SetCurrentID(string nextID)
    {
        CurrentDialogueID = nextID;
    }

    // ======== StoreManager 연락부분 (마음에 안드시거나 event로 하고싶으시면 바꾸셔도됩니다) ========

    private bool isPointDouble = false;   // true일때 보너스 점수 계산을 2배로

    private float ex_dayDuration = 300f;  // 낮/밤 지속시간, 위에 구현되있는건 알지만 예시로 적어둔것

    public void PointDoubleItemPurchased()  // StoreManager쪽에서 아이템 구매시 실행
    {
        isPointDouble = true;
    }

    public void BonusDayDurationItemPurchased(float bonus)  // StoreManager쪽에서 아이템 구매시 실행
    {
        ex_dayDuration = ex_dayDuration + bonus;
    }

    private int PointCalculate(int basePoint, int miniGamePoint)  // 점수계산 부분 예시로 만든겁니다
    {
        int result = basePoint + miniGamePoint;

        if(isPointDouble) // DoublePoint 아이템이 구매되었으면, 보너스 점수 배율 적용
        {
            result = GameUtil.GetBonusScoreByRate(result, 2.0f);
        }

        return result;
    }

    public void GoatStaminaItemUsed(int value)
    {
        OnUseStaminaItem.Invoke(value);
    }

    public void GoatSpeedBoostPurchased(float value)
    {
        OnUseSpeedItem.Invoke(value);
    }
}
