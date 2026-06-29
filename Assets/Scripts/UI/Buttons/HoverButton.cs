using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int ButtonIndex;
    [SerializeField] private Image Image_This;

    public event Action<int> OnPointerEntered;
    public event Action<int> OnPointerExited;

    private void OnDestroy()
    {
        UnBindEvent();
    }

    public void SetImage(Sprite sprite)
    {
        Image_This.sprite = sprite;
    }

    private void UnBindEvent()
    {
        OnPointerEntered = null;
        OnPointerExited = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InvokePointerEntered();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InvokePointerExited();
    }

    private void InvokePointerEntered()
    {
        OnPointerEntered?.Invoke(ButtonIndex);
    }

    private void InvokePointerExited()
    {
        OnPointerExited?.Invoke(ButtonIndex);
    }
}
