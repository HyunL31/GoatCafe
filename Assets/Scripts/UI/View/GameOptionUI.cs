using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameOptionPopup : BaseUI<GameOptionPopup>
{
    [SerializeField] private GameOptionTabButton[] GameOptionTabButtons;

    [SerializeField] private GameObject[] GameObject_MenuList;

    [SerializeField] private Slider Slider_SFXSound;
    [SerializeField] private TMP_Text Text_SFXSound;
    
    [SerializeField] private Slider Slider_BGMSound;
    [SerializeField] private TMP_Text Text_BGMSound;

    [SerializeField] private Toggle Toggle_FullScreen;

    [SerializeField] private TMP_Dropdown Dropdown_Resolution;
    [SerializeField] private TMP_Dropdown Dropdown_RefreshRate;

    [SerializeField] private Button Button_Background;
    [SerializeField] private Button Button_ExitUI;

    private int _selectedButtonIndex;

    public event Action<float> OnSFXSliderChanged;
    public event Action<float> OnBGMSliderChanged;

    public event Action<bool> OnFullScreenToggleChanged;

    public event Action<int> OnResolutionChanged;
    public event Action<int> OnRefreshRateChanged;

    public event Action OnBackgroundButtonClicked;
    public event Action OnExitButtonClicked;

    private void OnEnable()
    {
        BindSliderEvent();
        BindToggleEvent();
        BindButtonEvent();
        BindDropdownEvent();
    }

    private void BindSliderEvent()
    {
        Slider_SFXSound.onValueChanged.AddListener(InvokeSFXSliderChanged);
        Slider_BGMSound.onValueChanged.AddListener(InvokeBGMSliderChanged);
    }

    private void BindToggleEvent()
    {
        Toggle_FullScreen.onValueChanged.AddListener(InvokeFullScreenToggleChanged);
    }

    private void BindButtonEvent()
    {
        Button_Background.onClick.AddListener(InvokeBackgroundButton);
        Button_ExitUI.onClick.AddListener(InvokeExitButtonClicked);
    }

    private void BindDropdownEvent()
    {
        Dropdown_Resolution.onValueChanged.AddListener(InvokeResolutionChanged);
        Dropdown_RefreshRate.onValueChanged.AddListener(InvokeRefreshRateChanged);
    }

    private void OnDisable()
    {
        UnBindSliderEvent();
        UnBindToggleEvent();
        UnBindButtonEvent();
        UnBindDropdownEvent();
    }

    private void UnBindSliderEvent()
    {
        Slider_SFXSound.onValueChanged.RemoveListener(InvokeSFXSliderChanged);
        Slider_BGMSound.onValueChanged.RemoveListener(InvokeBGMSliderChanged);
    }

    private void UnBindToggleEvent()
    {
        Toggle_FullScreen.onValueChanged.RemoveListener(InvokeFullScreenToggleChanged);
    }

    private void UnBindButtonEvent()
    {
        Button_Background.onClick.RemoveListener(InvokeBackgroundButton);
        Button_ExitUI.onClick.RemoveListener(InvokeExitButtonClicked);
    }

    private void UnBindDropdownEvent()
    {
        Dropdown_Resolution.onValueChanged.RemoveListener(InvokeResolutionChanged);
        Dropdown_RefreshRate.onValueChanged.RemoveListener(InvokeRefreshRateChanged);
    }

    public void SetTabButtons()
    {
        for (int i = 0; i < GameOptionTabButtons.Length; i++)
        {
            GameOptionTabButtons[i].SetButtonIndex(i);

            GameOptionTabButtons[i].BindButtonSelectEvent(SelectTabButton);
        }

        SelectTabButton(0);
    }

    public void SetUIBaseSetting(float sfxVolume, float bgmVolume, bool isFullScreen)
    {
        Slider_SFXSound.SetValueWithoutNotify(sfxVolume);
        SetSFXText(sfxVolume);

        Slider_BGMSound.SetValueWithoutNotify(bgmVolume);
        SetBGMText(bgmVolume);

        Toggle_FullScreen.SetIsOnWithoutNotify(isFullScreen);
    }
    public void SetResolutionDropdown(List<string> resolutionList, int index)
    {
        Dropdown_Resolution.ClearOptions();
        Dropdown_Resolution.AddOptions(resolutionList);
        Dropdown_Resolution.SetValueWithoutNotify(index);
        Dropdown_Resolution.RefreshShownValue();
    }

    public void SetRefreshRateDropdown(List<string> refreshRateList, int index)
    {
        Dropdown_RefreshRate.ClearOptions();
        Dropdown_RefreshRate.AddOptions(refreshRateList);
        Dropdown_RefreshRate.SetValueWithoutNotify(index);
        Dropdown_RefreshRate.RefreshShownValue();
    }

    private void SelectTabButton(int index)
    {
        for (int i = 0; i < GameOptionTabButtons.Length; i++)
        {
            if (i == index)
            {
                GameOptionTabButtons[i].Select();
            }
            else
            {
                GameOptionTabButtons[i].OnPointerDeselected();
            }
        }

        _selectedButtonIndex = index;
        OpenMenu(index);
    }

    private void OpenMenu(int index)
    {
        if (GameObject_MenuList == null || GameObject_MenuList.Length == 0) return;

        for (int i = 0; i < GameObject_MenuList.Length; i++)
        {
            GameObject_MenuList[i].SetActive(i == index);
        }
    }
    private void SetSFXText(float volume)
    {
        Text_SFXSound.text = volume.ToString();
    }

    private void SetBGMText(float volume)
    {
        Text_BGMSound.text = volume.ToString();
    }

    private void InvokeSFXSliderChanged(float volume)
    {
        OnSFXSliderChanged?.Invoke(volume);
        SetSFXText(volume);
    }

    private void InvokeBGMSliderChanged(float volume)
    {
        OnBGMSliderChanged?.Invoke(volume);
        SetBGMText(volume);
    }

    private void InvokeFullScreenToggleChanged(bool isFullScreen)
    {
        OnFullScreenToggleChanged?.Invoke(isFullScreen);
    }

    private void InvokeBackgroundButton()
    {
        OnBackgroundButtonClicked?.Invoke();
    }

    private void InvokeExitButtonClicked()
    {
        OnExitButtonClicked?.Invoke();
    }

    private void InvokeResolutionChanged(int index)
    {
        OnResolutionChanged?.Invoke(index);
    }

    private void InvokeRefreshRateChanged(int index)
    {
        OnRefreshRateChanged?.Invoke(index);
    }
}
