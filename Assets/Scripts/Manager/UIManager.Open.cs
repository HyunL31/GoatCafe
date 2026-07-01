using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIManager
{
    InteractionPromptPresenter _interactionPromptPresenter;
    private Dictionary<Type, BasePresenter> _presenterList = new();
    private DialogueUI _dialogueUI;
    private GameResultPanel _gameResultPanel;

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
        GameManager.Instance.SetCurrentID("Opening_01");
        CursorManager.Instance.UnlockCursor();
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

    public async UniTask OpenGameResultPanel()
    {
        if (_gameResultPanel != null)
        {
            _gameResultPanel.gameObject.SetActive(true);

            if (!_activeUI.Contains(UIType.GameResultPanel))
            {
                _activeUI.Add(UIType.GameResultPanel);
            }

            return;
        }

        _gameResultPanel = await CreateUIAsync<GameResultPanel>(UIType.GameResultPanel);

        if (_gameResultPanel != null)
        {
            _gameResultPanel.gameObject.SetActive(true);
            _activeUI.Add(UIType.GameResultPanel);
        }
    }

    public void OpenInteractionPrompt(string key, string actionText, Transform target)
    {
        if (_interactionPromptPresenter == null)
        {
            _interactionPromptPresenter = new InteractionPromptPresenter();
        }

        InteractionPromptUI interactionPromptUI = CreateUI<InteractionPromptUI>(_interactionPromptPresenter.UIType_This);

        if (interactionPromptUI == null)
        {
            Debug.LogError("InteractionPromptUI 생성 실패");
            return;
        }

        _activeUI.Add(_interactionPromptPresenter.UIType_This);

        _interactionPromptPresenter.InitUI(interactionPromptUI);
        _interactionPromptPresenter.SetPrompt(key, actionText, target);
    }

    public void CloseInteractionPrompt()
    {
        if (_activeUI.Contains(UIType.InteractionPromptUI) == false)
        {
            return;
        }

        CloseUI(UIType.InteractionPromptUI);
    }
}
