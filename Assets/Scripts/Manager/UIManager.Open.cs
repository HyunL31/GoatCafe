using System;
using UnityEngine;

public partial class UIManager
{
    MainMenuUIPresenter _mainMenuUIPresenter;

    public void OpenMainMenu()
    {
        if (_mainMenuUIPresenter == null)
        {
            _mainMenuUIPresenter = new MainMenuUIPresenter();
        }

        _activeUI.Add(_mainMenuUIPresenter.UIType_This);
        _mainMenuUIPresenter.InitUI(CreateUI<MainUI>(_mainMenuUIPresenter.UIType_This));
    }

    SaveSlotPopupPresenter _saveSlotPopupPresenter;

    public void OpenSaveSlotPopup(Action action)
    {
        if(_saveSlotPopupPresenter == null)
        {
            _saveSlotPopupPresenter = new SaveSlotPopupPresenter();
        }

        _activeUI.Add(_saveSlotPopupPresenter.UIType_This);
        _saveSlotPopupPresenter.InitUI(CreateUI<MainUI>(_saveSlotPopupPresenter.UIType_This));
    }

    public void OpenInGameUI()
    {

    }

    public void OpenGameOptionPopup()
    {
    }
}
