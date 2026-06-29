using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class GameOptionPopupPresenter : BasePresenter<GameOptionPopupPresenter, GameOptionPopup>
{
    public override UIType UIType_This { get; } = UIType.GameOptionUI;

    private GameOptionPopup _gameOptionUI;

    private List<(int width, int height)> _resolutionList;
    private Dictionary<(int width, int height), List<RefreshRate>> _refreshList;

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
        _refreshList = new();

        foreach (Resolution resolution in Screen.resolutions)
        {
            (int width, int height) resolutionKey = (resolution.width, resolution.height);

            if(_refreshList.TryGetValue(resolutionKey, out List<RefreshRate> refreshRateList) == false)
            {
                refreshRateList = new();
                _refreshList[resolutionKey] = refreshRateList;
                _resolutionList.Add(resolutionKey);
            }

            refreshRateList.Add(resolution.refreshRateRatio);
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
        SetRefreshRateDropdown(_selectedResolution);
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

    private void SetRefreshRateDropdown(int resolutionIndex)
    {
        (int width, int height) = _resolutionList[resolutionIndex];
        List<RefreshRate> refreshRates = _refreshList[(width, height)];

        List<string> refreshRateOption = new();
        int currentIndex = 0;

        for(int i = 0; i < refreshRates.Count; i++)
        {
            refreshRateOption.Add($"{Mathf.RoundToInt((float)refreshRates[i].value)}Hz");

            if (refreshRates[i].value == Screen.currentResolution.refreshRateRatio.value)
            {
                currentIndex = i;
            }
        }

        _gameOptionUI.SetRefreshRateDropdown(refreshRateOption, currentIndex);
    }

    private void SubscribeEvents()
    {
        _gameOptionUI.OnSFXSliderChanged += On_ChangeSFXVolume;
        _gameOptionUI.OnBGMSliderChanged += On_ChangeBGMVolume;

        _gameOptionUI.OnFullScreenToggleChanged += On_ChangeFullScreen;

        _gameOptionUI.OnResolutionChanged += On_ChangeResolution;
        _gameOptionUI.OnRefreshRateChanged += On_ChangeRefreshRate;

        _gameOptionUI.OnBackgroundButtonClicked += OnClick_Background;
        _gameOptionUI.OnExitButtonClicked += OnClick_Exit;
    }

    private void UnsubscribeEvent()
    {
        _gameOptionUI.OnSFXSliderChanged -= On_ChangeSFXVolume;
        _gameOptionUI.OnBGMSliderChanged -= On_ChangeBGMVolume;

        _gameOptionUI.OnFullScreenToggleChanged -= On_ChangeFullScreen;

        _gameOptionUI.OnResolutionChanged -= On_ChangeResolution;
        _gameOptionUI.OnRefreshRateChanged -= On_ChangeRefreshRate;

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
        SetRefreshRateDropdown(resolutionIndex);
        SetScreenResolution();
    }

    private void On_ChangeRefreshRate(int refreshRateIndex)
    {
        SetScreenResolution(refreshRateIndex);
    }

    private void SetScreenResolution(int rateIndex = 0)
    {
        (int width, int height) = _resolutionList[_selectedResolution];
        RefreshRate rate = _refreshList[(width, height)][rateIndex];
        Screen.SetResolution(width, height, Screen.fullScreenMode, rate);
    }

    private void OnClick_Background()
    {
        UnsubscribeEvent();
        UIManager.Instance.CloseUI(UIType_This);
    }

    private void OnClick_Exit()
    {
        UnsubscribeEvent();
        UIManager.Instance.CloseUI(UIType_This);
        GameManager.Instance.ResumeGame();
    }
}