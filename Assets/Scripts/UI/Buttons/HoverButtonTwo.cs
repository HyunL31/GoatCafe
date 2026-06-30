using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class GameOptionTabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform RectTransform_This;
    [SerializeField] private Image Image_Target;
    [SerializeField] private Image Image_Background;

    public event Action<int> OnButtonSelected;

    public event Action OnPointerEntered;
    public event Action OnPointerClicked;
    public event Action OnPointerExited;

    private float _exitRatio = 0.6f;

    private Vector2 _enteredVector2;
    private Vector2 _exitedVector2;

    private int _buttonIndex;
    private bool _isClicked;

    private void Awake()
    {
        float height = RectTransform_This.rect.height;
        _enteredVector2 = new Vector2(0, 0);
        _exitedVector2 = new Vector2(0, -height * _exitRatio);

    }

    private void OnDestroy()
    {
        UnBindEvent();
    }

    public void SetButtonIndex(int buttonIndex)
    {
        _buttonIndex = buttonIndex;
    }

    public void SetImage(Sprite targetSprite, Sprite backgroundSprite)
    {
        Image_Target.sprite = targetSprite;
        Image_Background.sprite = backgroundSprite;
    }

    public void Select()
    {
        _isClicked = true;
        RectTransform_This.anchoredPosition = _enteredVector2;
    }

    public void BindPointerEvent(Action onPointerEnteredCallback = null, Action onPointerClickedCallback = null, Action onPointerExitedCallback = null)
    {
        OnPointerEntered = onPointerEnteredCallback;
        OnPointerClicked = onPointerClickedCallback;
        OnPointerExited = onPointerExitedCallback;
    }

    public void BindButtonSelectEvent(Action<int> onButtonSelected)
    {
        OnButtonSelected = onButtonSelected;
    }

    private void UnBindEvent()
    {
        OnPointerEntered = null;
        OnPointerClicked = null;
        OnPointerExited = null;
        OnButtonSelected = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InvokePointerEntered();
        ChangeRectTransformToEnteredVector2();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isClicked = true;
        InvokePointerClicked();
        InvokerButtonSelected();
    }

    public void OnPointerDeselected()
    {
        _isClicked = false;
        ChangeRectTransformToExitedVector2();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InvokePointerExited();
        ChangeRectTransformToExitedVector2();
    }

    private void ChangeRectTransformToEnteredVector2()
    {
        RectTransform_This.anchoredPosition = _enteredVector2;
    }

    private void ChangeRectTransformToExitedVector2()
    {
        if (_isClicked == false)
        {
            RectTransform_This.anchoredPosition = _exitedVector2;
        }
    }

    private void InvokerButtonSelected()
    {
        OnButtonSelected?.Invoke(_buttonIndex);
    }

    private void InvokePointerEntered()
    {
        OnPointerEntered?.Invoke();
    }

    private void InvokePointerClicked()
    {
        OnPointerClicked?.Invoke();
    }

    private void InvokePointerExited()
    {
        OnPointerExited?.Invoke();
    }
}
