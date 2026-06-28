using System;
using System.Collections.Generic;

public partial class UIManager
{
    private Dictionary<Type, BasePresenter> _presenterList = new();

    public TPresenter OpenUI<TPresenter, TUI>() where TPresenter : BasePresenter<TPresenter, TUI>, new() where TUI : BaseUI<TUI>
    {
        Type type = typeof(TPresenter);
        if(_presenterList.TryGetValue(type, out BasePresenter cachedPresenter) == false)
        {
            cachedPresenter = new TPresenter();
            _presenterList.Add(type, cachedPresenter);
        }

        TPresenter presenter = cachedPresenter as TPresenter;
        _activeUI.Add(presenter.UIType_This);
        presenter.InitUI(CreateUI<TUI>(presenter.UIType_This));
        return presenter;
    }

    public void OpenMainMenuUI()
    {
        OpenUI<MainMenuUIPresenter, MainMenuUI>();
    }

    public void OpenSaveSlotPopup(Action closeMainMenuCallback)
    {
        SaveDataSlotPopupPresenter saveDataSlotPopupPresenter = OpenUI<SaveDataSlotPopupPresenter, SaveDataSlotPopup>();
        saveDataSlotPopupPresenter.InitEvent(closeMainMenuCallback);

    }

    public void OpenInGameUI()
    {
        OpenUI<InGamePresenter, InGameUI>();
    }

    public void OpenInGamePopup(Action closeInGameUICallback)
    {
        InGamePopupPresenter inGamePopupPresenter = OpenUI<InGamePopupPresenter, InGamePopup>();
        inGamePopupPresenter.InitEvent(closeInGameUICallback);
    }

    public void OpenGameOptionUI()
    {
    }

    public void OpenTutorialPopup(int index = 0)
    {
        TutorialPopupPresenter tutorialPopupPresenter = OpenUI<TutorialPopupPresenter, TutorialPopup>();

        if (index != 0)
        {
            tutorialPopupPresenter.InitPanelIndex(index);
        }
    }
}
