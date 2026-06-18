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

    public PlayerModel PlayerModel { get; private set; } = new PlayerModel();
    public int CurrentSlotIndex { get; private set; } = 0;
    public HashSet<int> SlotIndex { get; private set; } = new HashSet<int>();

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
    public event Action<int> OnSaveSlotChanged;

    protected override void Awake()
    {
        base.Awake();

        InitSaveSlot();
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


    //// Save

    public void SaveData()
    {
        SaveManager.Instance.RequestSaveData(CurrentSlotIndex, PlayerModel);
        SlotIndex.Add(CurrentSlotIndex);
    }

    public void LoadData(int index)
    {
        PlayerModel = SaveManager.Instance.RequestLoadData(index);
    }

    public void LoadDefaultData()
    {
        PlayerModel = SaveManager.Instance.GetDefaultData();
    }

    public void SetCurrentSaveIndex(int index)
    {
        CurrentSlotIndex = index;
        OnSaveSlotChanged?.Invoke(CurrentSlotIndex);
    }

    public int GetEmptySlotIndex()
    {
        for (int i = 0; i < 100; i++)
        {
            if (!SlotIndex.Contains(i))
            {
                return i;
            }
        }

        return 0;
    }

    private void InitSaveSlot()
    {
        for (int i = 0; i < 100; i++)
        {
            if (SaveManager.Instance.HasSaveFile(i))
            {
                SlotIndex.Add(i);
            }
        }
    }

    public void LoadOrCreatePlayerData(int index)
    {
        SetCurrentSaveIndex(index);

        if (SaveManager.Instance.HasSaveFile(index))
        {
            LoadData(index);
        }
        else
        {
            LoadDefaultData();
            SaveData();
        }

        StartGame();
    }
}
