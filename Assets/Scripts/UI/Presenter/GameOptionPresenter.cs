using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class GameOptionPopupPresenter : BasePresenter<GameOptionPopupPresenter, GameOptionPopup>
{
    public override UIType UIType_This { get; } = UIType.GameOptionUI;

    private GameOptionPopup _gameOptionUI;

    private List<(int width, int height)> _resolutionList;

    private int[] _frameRate = { 30, 60, 120, 144, -1 };

    private int _selectedResolution;

    public override void InitUI(GameOptionPopup ui)
    {
        _gameOptionUI = ui;
        SetUI().Forget();
    }
    protected async override UniTaskVoid SetUI()
    {
        await LoadAssetAsync();
        LoadData();
        SetUIData();

        _gameOptionUI.ActiveTrue();
    }

    protected async override UniTask LoadAssetAsync()
    {
    }

    protected override void LoadData()
    {
        _resolutionList = new();
        var seen = new HashSet<(int, int)>();

        foreach (Resolution resolution in Screen.resolutions)
        {
            var key = (resolution.width, resolution.height);
            if (seen.Add(key))
                _resolutionList.Add(key);
        }
    }


    protected override void SetUIData()
    {
        _gameOptionUI.SetTabButtons();

        float sfxVolume = SoundManager.Instance.GetSFXVolume() * 10;
        float bgmVolume = SoundManager.Instance.GetBGMVolume() * 10;

        bool isFullScreen = Screen.fullScreen;

        SubscribeEvents();
        _gameOptionUI.SetUIBaseSetting(sfxVolume, bgmVolume, isFullScreen);

        SetResolutionList();
        SetFrameRateDropdown();
    }

    private void SetResolutionList()
    {
        List<string> resolutionOption = new();

        _selectedResolution = 0;

        for (int i = 0; i < _resolutionList.Count; i++)
        {
            (int width, int height) = _resolutionList[i];

            resolutionOption.Add($"{width} x {height}");

            if (width == Screen.width && height == Screen.height)
            {
                _selectedResolution = i;
            }
        }

        _gameOptionUI.SetResolutionDropdown(resolutionOption, _selectedResolution);
    }

    private void SetFrameRateDropdown()
    {
        List<string> options = new();
        int currentIndex = 0;

        for (int i = 0; i < _frameRate.Length; i++)
        {
            int fps = _frameRate[i];
            options.Add(fps == -1 ? "무제한" : $"{fps} FPS");

            if (fps == Application.targetFrameRate)
                currentIndex = i;
        }

        _gameOptionUI.SetFrameRateDropdown(options, currentIndex);
    }

    private void SubscribeEvents()
    {
        _gameOptionUI.OnSFXSliderChanged += On_ChangeSFXVolume;
        _gameOptionUI.OnBGMSliderChanged += On_ChangeBGMVolume;

        _gameOptionUI.OnFullScreenToggleChanged += On_ChangeFullScreen;

        _gameOptionUI.OnResolutionChanged += On_ChangeResolution;
        _gameOptionUI.OnFrameRateChanged += On_ChangeFrameRate;

        _gameOptionUI.OnBackgroundButtonClicked += OnClick_Background;
        _gameOptionUI.OnExitButtonClicked += OnClick_Exit;
    }

    private void UnsubscribeEvent()
    {
        _gameOptionUI.OnSFXSliderChanged -= On_ChangeSFXVolume;
        _gameOptionUI.OnBGMSliderChanged -= On_ChangeBGMVolume;

        _gameOptionUI.OnFullScreenToggleChanged -= On_ChangeFullScreen;

        _gameOptionUI.OnResolutionChanged -= On_ChangeResolution;
        _gameOptionUI.OnFrameRateChanged -= On_ChangeFrameRate;

        _gameOptionUI.OnBackgroundButtonClicked -= OnClick_Background;
        _gameOptionUI.OnExitButtonClicked -= OnClick_Exit;
    }

    private void On_ChangeSFXVolume(float volume)
    {
        SoundManager.Instance.SetSFXVolume(volume * 0.1f);
    }

    private void On_ChangeBGMVolume(float volume)
    {
        SoundManager.Instance.SetBGMVolume(volume * 0.1f);
    }

    private void On_ChangeFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    private void On_ChangeResolution(int resolutionIndex)
    {
        _selectedResolution = resolutionIndex;
        (int width, int height) = _resolutionList[resolutionIndex];
        Screen.SetResolution(width, height, Screen.fullScreenMode);
    }

    private void On_ChangeFrameRate(int index)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _frameRate[index];
    }

    private void OnClick_Background()
    {
        UnsubscribeEvent();
        UIManager.Instance.CloseUI(UIType_This);
        GameManager.Instance.ResumeGame();
    }

    private void OnClick_Exit()
    {
        UnsubscribeEvent();
        UIManager.Instance.CloseUI(UIType_This);
        GameManager.Instance.ResumeGame();
    }
}