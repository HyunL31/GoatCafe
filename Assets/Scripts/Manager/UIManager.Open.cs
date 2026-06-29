using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public partial class UIManager
{
    private Dictionary<Type, BasePresenter> _presenterList = new();
    private DialogueUI _dialogueUI;

    public TPresenter OpenUI<TPresenter, TUI>() where TPresenter : BasePresenter<TPresenter, TUI>, new() where TUI : BaseUI<TUI>
    {
        Type type = typeof(TPresenter);
        if (_presenterList.TryGetValue(type, out BasePresenter cachedPresenter) == false)
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
        OpenUI<GameOptionPopupPresenter, GameOptionPopup>();
    }

    public void OpenTutorialPopup(int index = 0)
    {
        TutorialPopupPresenter tutorialPopupPresenter = OpenUI<TutorialPopupPresenter, TutorialPopup>();

        if (index != 0)
        {
            tutorialPopupPresenter.InitPanelIndex(index);
        }
    }

    public void OpenInfoPopup()
    {
        InfoPopupPresenter infoPopupPresenter = OpenUI<InfoPopupPresenter, InfoPopup>();
    }


    public async UniTask OpenDialogueUI()
    {
        if (_dialogueUI != null)
        {
            _dialogueUI.gameObject.SetActive(true);

            if (!_activeUI.Contains(UIType.DialogueUI))
            {
                _activeUI.Add(UIType.DialogueUI);
            }

            return;
        }

        _dialogueUI = await CreateUIAsync<DialogueUI>(UIType.DialogueUI);

        if (_dialogueUI != null)
        {
            _dialogueUI.gameObject.SetActive(true);
            _activeUI.Add(UIType.DialogueUI);
        }
    }

    public void CloseDialogueUI()
    {
        if (_dialogueUI != null)
        {
            _dialogueUI.gameObject.SetActive(false);

            if (_activeUI.Contains(UIType.DialogueUI))
            {
                _activeUI.Remove(UIType.DialogueUI);
            }
        }
    }

}
