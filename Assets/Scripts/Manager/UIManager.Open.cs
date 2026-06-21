using System;
using UnityEngine;

public partial class UIManager
{
    MainMenuUIPresenter _mainMenuUIPresenter;

    public void OpenMainMenuUI()
    {
        if (_mainMenuUIPresenter == null)
        {
            _mainMenuUIPresenter = new MainMenuUIPresenter();
        }

        _activeUI.Add(_mainMenuUIPresenter.UIType_This);
        _mainMenuUIPresenter.InitUI(CreateUI<MainMenuUI>(_mainMenuUIPresenter.UIType_This));
    }

    SaveDataSlotPopupPresenter _saveDataSlotPopupPresenter;

    public void OpenSaveSlotPopup(Action action)
    {
        if(_saveDataSlotPopupPresenter == null)
        {
            _saveDataSlotPopupPresenter = new SaveDataSlotPopupPresenter();
        }

        _activeUI.Add(_saveDataSlotPopupPresenter.UIType_This);
        _saveDataSlotPopupPresenter.InitUI(CreateUI<SaveDataSlotPopup>(_saveDataSlotPopupPresenter.UIType_This));
        _saveDataSlotPopupPresenter.SetAction(action);
    }

    public void OpenInGameUI()
    {

    }

    public void OpenGameOptionPopup()
    {
    }
}
