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

public class GameManager : BaseMonoManager<GameManager>
{
    public GameState CurrentState { get; private set; } = GameState.None;

    public bool IsPlaying => CurrentState == GameState.Playing;
    public bool IsPaused => CurrentState == GameState.Pause;
    public bool IsGameOver => CurrentState == GameState.GameOver;

    public event Action<GameState> OnGameStateChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        ChangeGameState(GameState.Loading);

        // TODO : 매니저 초기화 예정

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
}
