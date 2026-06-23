using System;
using UnityEngine;
using System.Collections.Generic;

public enum GameState
{
    None = 0,
    Loading,
    Ready,
    Playing,
    Pause,
    GameOver
}

public enum DayPhase
{
    None = 0,
    Day,
    Night
}

public class GameManager : BaseMonoManager<GameManager>
{
    [SerializeField] private float _dayDuration = 300f;

    private float _remainDayTime;

    public GameState CurrentState { get; private set; } = GameState.None;
    public DayPhase CurrentDayPhase { get; private set; } = DayPhase.None;

    public bool IsPlaying => CurrentState == GameState.Playing;
    public bool IsPaused => CurrentState == GameState.Pause;
    public bool IsGameOver => CurrentState == GameState.GameOver;

    public float RemainDayTime => _remainDayTime;
    public float DayDuration => _dayDuration;
    public float DayTimeRate => _dayDuration <= 0f ? 0f : _remainDayTime / _dayDuration;

    public event Action<GameState> OnGameStateChanged;
    public event Action<DayPhase> OnDayPhaseChanged;
    public event Action<float> OnDayTimeChanged;

    public event Action<int> OnUseStaminaItem;
    public event Action<int, int> OnUseSpeedItem;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitializeGame();
    }

    private void Update()
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

    // Pause(일시정지) 상태로 전환
    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
        {
            return;
        }

        Time.timeScale = 0f;
        ChangeGameState(GameState.Pause);
    }

    // Pause > Playing 상태로 전환
    public void ResumeGame()
    {
        if (CurrentState != GameState.Pause)
        {
            return;
        }

        Time.timeScale = 1f;
        ChangeGameState(GameState.Playing);
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

    private void UpdateDayTime()
    {
        if (!IsPlaying)
        {
            return;
        }

        if (CurrentDayPhase != DayPhase.Day)
        {
            return;
        }

        _remainDayTime -= Time.deltaTime;
        _remainDayTime = Mathf.Max(_remainDayTime, 0f);

        OnDayTimeChanged?.Invoke(_remainDayTime);

        if (_remainDayTime <= 0f)
        {
            ChangeDayPhase(DayPhase.Night);
        }
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
}
