using System;
using UnityEngine;

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

    public void StartGame()
    {
        if (CurrentState != GameState.Ready)
        {
            return;
        }

        Time.timeScale = 1f;
        ChangeGameState(GameState.Playing);
    }

    public void StartDay()
    {
        _remainDayTime = _dayDuration;
        ChangeDayPhase(DayPhase.Day);
        OnDayTimeChanged?.Invoke(_remainDayTime);
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
        {
            return;
        }

        Time.timeScale = 0f;
        ChangeGameState(GameState.Pause);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Pause)
        {
            return;
        }

        Time.timeScale = 1f;
        ChangeGameState(GameState.Playing);
    }

    public void EndGame()
    {
        if (CurrentState == GameState.GameOver)
        {
            return;
        }

        Time.timeScale = 1f;
        ChangeGameState(GameState.GameOver);
    }

    public void ChangeGameState(GameState gameState)
    {
        if (CurrentState == gameState)
        {
            return;
        }

        CurrentState = gameState;

        Debug.Log($"게임 상태 변경됨 > {CurrentState}");
        OnGameStateChanged?.Invoke(CurrentState);
    }

    public void ChangeDayPhase(DayPhase dayPhase)
    {
        if (CurrentDayPhase == dayPhase)
        {
            return;
        }

        CurrentDayPhase = dayPhase;
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
}
