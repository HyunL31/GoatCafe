using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIManager
{
    MainMenuUIPresenter _mainMenuUIPresenter;
    SaveDataSlotPopupPresenter _saveDataSlotPopupPresenter;
    InGamePresenter _inGamePresenter;
    InGamePopupPresenter _inGamePopupPresenter;
    InteractionPromptPresenter _interactionPromptPresenter;
    DialogueUI _dialogueUI;

    private GameResultPanel _gameResultPanel;
    List<BasePresenter> _presenterList = new();

    public void OpenUI<TPresenter, TUI>() where TPresenter : BasePresenter<TPresenter, TUI>, new() where TUI : BaseUI<TUI>
    {
    }

    public void OpenMainMenuUI()
    {
        if (_mainMenuUIPresenter == null)
        {
            _mainMenuUIPresenter = new MainMenuUIPresenter();
        }

        _activeUI.Add(_mainMenuUIPresenter.UIType_This);

        _mainMenuUIPresenter.InitUI(CreateUI<MainMenuUI>(_mainMenuUIPresenter.UIType_This));
    }

    public void OpenSaveSlotPopup(Action closeMainMenuCallback)
    {
        if(_saveDataSlotPopupPresenter == null)
        {
            _saveDataSlotPopupPresenter = new SaveDataSlotPopupPresenter();
        }

        _activeUI.Add(_saveDataSlotPopupPresenter.UIType_This);

        _saveDataSlotPopupPresenter.InitUI(CreateUI<SaveDataSlotPopup>(_saveDataSlotPopupPresenter.UIType_This));
        _saveDataSlotPopupPresenter.SetAction(closeMainMenuCallback);
    }

    public void OpenInGameUI()
    {
        if(_inGamePresenter == null)
        {
            _inGamePresenter = new InGamePresenter();
        }

        _activeUI.Add(_inGamePresenter.UIType_This);

        _inGamePresenter.InitUI(CreateUI<InGameUI>(_inGamePresenter.UIType_This));
    }

    public void OpenInGamePopup(Action closeInGameUICallback)
    {
        if(_inGamePopupPresenter == null)
        {
            _inGamePopupPresenter = new InGamePopupPresenter();
        }

        _activeUI.Add(_inGamePopupPresenter.UIType_This);

        _inGamePopupPresenter.InitUI(CreateUI<InGamePopup>(_inGamePopupPresenter.UIType_This));
        _inGamePopupPresenter.SubscribeEvent(closeInGameUICallback);
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

    public void OpenGameOptionUI()
    {
    }

    public void OpenTutorialPopup()
    {

    }
}
