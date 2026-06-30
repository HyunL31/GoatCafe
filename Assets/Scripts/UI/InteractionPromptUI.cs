using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPromptUI : BaseUI<InteractionPromptUI>
{
    [SerializeField] private RectTransform RectTransform_This;
    [SerializeField] private Image Image_Background;
    [SerializeField] private Image Image_KeyBox;
    [SerializeField] private TMP_Text Text_Key;
    [SerializeField] private TMP_Text Text_Action;

    private Transform _target;

    public void SetPromptData(Sprite backgroundSprite, Sprite keyBoxSprite, TMP_FontAsset fontAsset)
    {
        if (backgroundSprite != null)
        {
            Image_Background.sprite = backgroundSprite;
        }

        if (keyBoxSprite != null)
        {
            Image_KeyBox.sprite = keyBoxSprite;
        }

        if (fontAsset != null)
        {
            Text_Key.font = fontAsset;
            Text_Action.font = fontAsset;
        }
    }

    public void Open(string key, string actionText, Transform target)
    {
        _target = target;

        Text_Key.text = key;
        Text_Action.text = actionText;

        this.ActiveTrue();
        UpdatePosition();
    }

    public void Close()
    {
        _target = null;
        this.ActiveFalse();
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (_target == null)
        {
            return;
        }

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_target.position + Vector3.up * 1.5f);

        RectTransform_This.position = screenPosition;
    }
}