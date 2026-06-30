using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class InteractionPromptPresenter : BasePresenter<InteractionPromptPresenter, InteractionPromptUI>
{
    public override UIType UIType_This { get; } = UIType.InteractionPromptUI;

    private InteractionPromptUI _interactionPromptUI;

    //private Sprite _sprite_background;
    //private Sprite _sprite_keyBox;
    private TMP_FontAsset _fontAsset_promptFont;

    private string _key;
    private string _actionText;
    private Transform _target;

    public override void InitUI(InteractionPromptUI ui)
    {
        _interactionPromptUI = ui;

        SetUI().Forget();
    }

    public void SetPrompt(string key, string actionText, Transform target)
    {
        Debug.Log($"Prompt : {key} / {actionText}");
        _key = key;
        _actionText = actionText;
        _target = target;

        if (_interactionPromptUI != null)
        {
            _interactionPromptUI.Open(_key, _actionText, _target);
        }
    }

    protected async override UniTaskVoid SetUI()
    {
        await LoadAssetAsync();
        LoadData();
        SetUIData();

        _interactionPromptUI.Open(_key, _actionText, _target);
    }

    protected async override UniTask LoadAssetAsync()
    {
        //var (backgroundSprite, keyBoxSprite, fontAsset) = await UniTask.WhenAll
        //(
        //    LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InteractionPrompt.Background),
        //    LoadUtil.Async.LoadSpriteAsync(AddressUtil.Sprite.UI.InteractionPrompt.KeyBox),
        //    LoadUtil.Async.LoadFontAssetAsync(AddressUtil.Font.BaseFont)
        //);

        //_sprite_background = backgroundSprite;
        //_sprite_keyBox = keyBoxSprite;
        //_fontAsset_promptFont = fontAsset;
    }

    protected override void LoadData()
    {
    }

    protected override void SetUIData()
    {
        //_interactionPromptUI.SetPromptData(_sprite_background, _sprite_keyBox, _fontAsset_promptFont);
    }

    public void ClosePrompt()
    {
        if (_interactionPromptUI == null)
        {
            return;
        }

        _interactionPromptUI.Close();
    }
}