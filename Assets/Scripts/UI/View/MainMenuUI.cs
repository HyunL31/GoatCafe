using UnityEngine;
using System;
using TMPro;

public class MainMenuUI : BaseUI<MainMenuUI>
{
    [SerializeField] private Transform Transform_StartButton;
    [SerializeField] private Transform Transform_GameOptionButton;
    [SerializeField] private Transform Transform_ExitGameButton;

    private NormalButton _startButton;
    private NormalButton _gameOptionButton;
    private NormalButton _exitGameButton;

    public void SetStartButton(GameObject startButtonPrefab, Sprite startButtonSprite, string startButtonText, TMP_FontAsset startButtonFont, Action startButtonCallback)
    {
        if(_startButton != null)
        {
            return;
        }

        NormalButton startButton = CreateButton(startButtonPrefab, Transform_StartButton, startButtonSprite, startButtonText, startButtonFont, startButtonCallback);
        _startButton = startButton;
    }

    public void SetGameOptionButton(GameObject GameOptionButtonPrefab, Sprite GameOptionButtonSprite, string gameOptionButtonText, TMP_FontAsset gameOptionButtonFont, Action gaemOptionButtonCallback)
    {
        if(_gameOptionButton != null)
        {
            return;
        }

        NormalButton gameOptionButton = CreateButton(GameOptionButtonPrefab, Transform_GameOptionButton, GameOptionButtonSprite, gameOptionButtonText, gameOptionButtonFont, gaemOptionButtonCallback);
        _gameOptionButton = gameOptionButton;
    }

    public void SetExitGameButton(GameObject exitGameButtonPrefab, Sprite exitGameButtonSprite, string exitGameButtonText, TMP_FontAsset exitGameButtonFont, Action exitGameButtonCallBack)
    {
        if(_exitGameButton != null)
        {
            return;
        }

        NormalButton exitGameButton = CreateButton(exitGameButtonPrefab, Transform_ExitGameButton, exitGameButtonSprite, exitGameButtonText, exitGameButtonFont, exitGameButtonCallBack);
        _exitGameButton = exitGameButton;
    }

    private NormalButton CreateButton(GameObject buttonPrefab, Transform buttonTransform, Sprite buttonSprite, string buttonText, TMP_FontAsset buttonFont, Action buttonCallback)
    {
        if (this.InstantiateAndGetComponent(buttonPrefab, buttonTransform, out NormalButton normalButton) == false)
        {
            return null;
        }

        if (normalButton.SetButtonData(buttonSprite, buttonText, buttonFont, buttonCallback) == false)
        {
            Destroy(normalButton.gameObject);
            return null;
        }

        return normalButton;
    }
}
