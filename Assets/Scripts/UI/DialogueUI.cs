using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : BaseUI<DialogueUI>
{
    public UIType UIType_This { get; } = UIType.DialogueUI;

    [SerializeField] private Image Image_Background;
    [SerializeField] private Button Button_Dialogue;
    [SerializeField] private TextMeshProUGUI Text_Dialogue;
    [SerializeField] private TextMeshProUGUI Text_Speaker;
    [SerializeField] private Image Image_Speaker;
    [SerializeField] private Image Image_NextArrow;

    private bool _isTyping = false;
    private float _typingWaitTime = 0.03f;
    private CancellationTokenSource _typingToken;
    private Dictionary<string, DialogueData> _dialogues;

    private void Awake()
    {
        Button_Dialogue.onClick.AddListener(OnClickDialogue);

        _dialogues = GameDataManager.Instance.DialogueDataList;
    }

    private void OnEnable()
    {
        GameManager.Instance.PauseGame();
        ShowDialogue(GetCurrentID());
    }

    private void OnDisable()
    {
        if (GameManager.Instance.CurrentEndingType == EndingType.None)
        {
            GameManager.Instance.ResumeGame();
        }

        CancelTypingRoutine();
    }

    private void OnClickDialogue()
    {
        if (_isTyping)
        {
            _isTyping = false;
        }
        else
        {
            MoveToNextDialogue(GetCurrentID());
        }
    }

    private void ShowDialogue(string id)
    {
        if (string.IsNullOrEmpty(_dialogues[id].Speaker))
        {
            Image_Speaker.gameObject.SetActive(false);
        }
        else
        {
            Image_Speaker.gameObject.SetActive(true);

            string speaker = _dialogues[id].Speaker;
            Text_Speaker.text = speaker;
        }

        CancelTypingRoutine();
        _typingToken = new CancellationTokenSource();

        Typing(id, _typingToken.Token).Forget();

        SetBackgroundImage(id).Forget();
    }

    private void MoveToNextDialogue(string id)
    {
        string nextID = _dialogues[id].NextID;

        if (nextID == "Open")
        {
            UIManager.Instance.CloseDialogueUI();
            GameManager.Instance.StartGame();
            return;
        }

        if (nextID == "GameClear")
        {
            UIManager.Instance.CloseDialogueUI();
            GameManager.Instance.ClearGame();
            return;
        }

        if (nextID == "GameOver")
        {
            UIManager.Instance.CloseDialogueUI();
            GameManager.Instance.HandleEndingDialogueFinished();
            return;
        }

        GameManager.Instance.SetCurrentID(nextID);

        ShowDialogue(GetCurrentID());
    }

    private async UniTaskVoid Typing(string id, CancellationToken token)
    {
        _isTyping = true;

        string content = _dialogues[id].Content;
        Text_Dialogue.text = content;
        Text_Dialogue.maxVisibleCharacters = 0;
        Image_NextArrow.gameObject.SetActive(false);

        if (_typingWaitTime > 0)
        {
            for (int i = 0; i < content.Length; i++)
            {
                if (!_isTyping)
                {
                    break;
                }

                Text_Dialogue.maxVisibleCharacters = i;

                await UniTask.Delay(TimeSpan.FromSeconds(_typingWaitTime), cancellationToken: token);
            }
        }

        Text_Dialogue.maxVisibleCharacters = content.Length;

        _isTyping = false;
        Image_NextArrow.gameObject.SetActive(true);
    }

    private void CancelTypingRoutine()
    {
        if (_typingToken != null)
        {
            _typingToken.Cancel();
            _typingToken.Dispose();
            _typingToken = null;
        }
    }

    private string GetCurrentID()
    {
        return GameManager.Instance.CurrentDialogueID;
    }

    private async UniTask SetBackgroundImage(string id)
    {
        string background = _dialogues[id].Background;

        if (string.IsNullOrEmpty(background))
        {
            return;
        }

        Image_Background.sprite = await LoadUtil.Async.LoadSpriteAsync($"Image/{background}");
    }
}
 