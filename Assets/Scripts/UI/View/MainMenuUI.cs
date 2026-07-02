using UnityEngine;
using System;

public class MainMenuUIView : BaseUI<MainMenuUIView>
{
    [SerializeField] private Transform Transform_StartButton;
    [SerializeField] private Transform Transform_GameOptionButton;
    [SerializeField] private Transform Transform_ExitGameButton;

    private NormalButton _startButton;
    private NormalButton _gameOptionButton;
    private NormalButton _exitGameButton;

    public bool SetButtonAsset(GameObject buttonPrefab, Sprite startGameButtonSprite, Sprite gameOptionButtonSprite, Sprite exitGameButtonSprite)
    {
        CreateButtons(buttonPrefab);

        bool isButtonCreate = true;
        int nullButtonCount = 0;

        if (_startButton == null)
        {
            this.LogError("StartButton이 생성되지 않았습니다!!");
            isButtonCreate = false;
            nullButtonCount++;
        }

        if (_gameOptionButton == null)
        {
            this.LogError("GameOptionButton이 생성되지 않았습니다!!");
            isButtonCreate = false;
            nullButtonCount++;
        }

        if (_exitGameButton == null)
        {
            this.LogError("ExitGameButton이 생성되지 않았습니다!!");
            isButtonCreate = false;
            nullButtonCount++;
        }

        if (isButtonCreate == false)
        {
            this.LogError($"총 {nullButtonCount}개의 버튼이 생성에 실패했습니다!!");
            return false;
        }

        _startButton.SetButtonData(buttonSprite: startGameButtonSprite);
        _gameOptionButton.SetButtonData(buttonSprite: gameOptionButtonSprite);
        _exitGameButton.SetButtonData(buttonSprite: exitGameButtonSprite);

        return true;
    }

    private void CreateButtons(GameObject buttonPrefab)
    {
        CreateButton(ref _startButton, buttonPrefab, Transform_StartButton);
        CreateButton(ref _gameOptionButton, buttonPrefab, Transform_GameOptionButton);
        CreateButton(ref _exitGameButton, buttonPrefab, Transform_ExitGameButton);
    }

    private void CreateButton(ref NormalButton button, GameObject buttonPrefab, Transform buttonTransform)
    {
        if (button != null)
        {
            this.Log("이미 버튼이 생성되어 있는 상태입니다!!");
            return;
        }

        NormalButton normalButton = this.InstantiateAndGetComponent<NormalButton>(buttonPrefab, buttonTransform);   
        
        if(normalButton == null)
        {
            this.LogError("버튼 생성에 실패했습니다!!");
            return;
        }

        button = normalButton;
    }

    public void SetButtonData(string startGameButtonText, string gameOptionButtonText, string exitGameButtonText)
    {
        _startButton.SetButtonData(buttonText: startGameButtonText);
        _gameOptionButton.SetButtonData(buttonText: gameOptionButtonText);
        _exitGameButton.SetButtonData(buttonText: exitGameButtonText);
    }

    public void SubscribeButtonEvent(Action startButtonEvent, Action gameOptionButtonEvent, Action exitGameButtonEvent)
    {
        _startButton.OnButtonClicked += startButtonEvent;
        _gameOptionButton.OnButtonClicked += gameOptionButtonEvent;
        _exitGameButton.OnButtonClicked += exitGameButtonEvent;
    }

    public void UnsubscribeButtonEvent()
    {
        _startButton.ResetButtonEvent();
        _gameOptionButton.ResetButtonEvent();
        _exitGameButton.ResetButtonEvent();
    }
}
