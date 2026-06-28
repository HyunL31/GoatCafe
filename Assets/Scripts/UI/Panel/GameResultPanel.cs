using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

public class GameResultPanel : BaseUI<GameResultPanel>
{
    [SerializeField] private TMP_Text Text_Result;

    [SerializeField] private string _gameClearText = "GAME CLEAR";
    [SerializeField] private string _gameOverText = "GAME OVER";

    [SerializeField] private Color _gameClearColor = Color.blue;
    [SerializeField] private Color _gameOverColor = Color.red;

    [SerializeField] private float _showDuration = 10f;

    private bool _isShowing;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameState gameState)
    {
        if (_isShowing)
        {
            return;
        }

        if (gameState == GameState.GameClear)
        {
            ShowResultAsync(_gameClearText, _gameClearColor).Forget();
            return;
        }

        if (gameState == GameState.GameOver)
        {
            ShowResultAsync(_gameOverText, _gameOverColor).Forget();
        }
    }

    private async UniTaskVoid ShowResultAsync(string resultText, Color resultColor)
    {
        _isShowing = true;

        Text_Result.text = resultText;
        Text_Result.color = resultColor;

        this.ActiveTrue();

        await UniTask.Delay(TimeSpan.FromSeconds(_showDuration), ignoreTimeScale: true);

        this.ActiveFalse();

        _isShowing = false;

        GameManager.Instance.ReturnTitle();
    }
}