using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : BaseUI<MainMenuUI>
{
    [SerializeField] private Transform Transform_StartButton;
    [SerializeField] private Transform Transform_GameOptionButton;
    [SerializeField] private Transform Transform_ExitGameButton;

    private NormalButton _startButton;
    private NormalButton _gameOptionButton;
    private NormalButton _exitGameButton;

    public void SetButtonAsset(GameObject buttonPrefab, Sprite startGameButtonSprite, Sprite gameOptionButtonSprite, Sprite exitGameButtonSprite)
    {
        CreateButtons(buttonPrefab);

        if (_startButton != null)
        {
            _startButton.SetButtonData(buttonSprite: startGameButtonSprite);
        }

        if (_gameOptionButton != null)
        {
            _gameOptionButton.SetButtonData(buttonSprite: gameOptionButtonSprite);
        }

        if (_exitGameButton != null)
        {
            _exitGameButton.SetButtonData(buttonSprite: exitGameButtonSprite);
        }
    }

    public void SetButtonData(string startGameButtonText, string gameOptionButtonText, string exitGameButtonText)
    {
        if (_startButton != null)
        {
            _startButton.SetButtonData(buttonText: startGameButtonText);
        }

        if (_gameOptionButton != null)
        {
            _gameOptionButton.SetButtonData(buttonText: gameOptionButtonText);
        }

        if (_exitGameButton != null)
        {
            _exitGameButton.SetButtonData(buttonText: exitGameButtonText);
        }
    }

    public void SetButtonEvent(Action startButtonEvent, Action gameOptionButtonEvent, Action exitGameButtonEvent)
    {
        if(_startButton != null)
        {
            _startButton.ResetButtonEvent();
            _startButton.OnButtonClicked += startButtonEvent;
        }

        if(_gameOptionButton != null)
        {
            _gameOptionButton.ResetButtonEvent();
            _gameOptionButton.OnButtonClicked += gameOptionButtonEvent;
        }

        if(_exitGameButton != null)
        {
            _exitGameButton.ResetButtonEvent();
            _exitGameButton.OnButtonClicked += exitGameButtonEvent;
        }
    }

    private void CreateButtons(GameObject buttonPrefab)
    {
        CreateButton(ref _startButton, buttonPrefab, Transform_StartButton);
        CreateButton(ref _gameOptionButton, buttonPrefab, Transform_GameOptionButton);
        CreateButton(ref _exitGameButton, buttonPrefab, Transform_ExitGameButton);
    }

    private void CreateButton(ref NormalButton button, GameObject buttonPrefab, Transform buttonTransform)
    {
        if(button != null)
        {
            return;
        }

        if(this.InstantiateAndGetComponent(buttonPrefab, buttonTransform, out NormalButton normalButton) == false)
        {
            return;
        }

        button = normalButton;
    }
}
