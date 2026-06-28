using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialPopup : BaseUI<TutorialPopup>
{
    [field : SerializeField] public GameObject[] PanelList { get; private set; }
    [SerializeField] private HoverButton[] HoverButtons;

    [SerializeField] private Button Button_Background;
    [SerializeField] private Button Button_Exit;
    [SerializeField] private Button Button_Next;
    [SerializeField] private Button Button_Previous;

    [SerializeField] private HoverDescriptionPanel HoverPopup;

    public event Action OnBackgroundClicked;
    public event Action OnExitButtonClicked;
    public event Action OnNextButtonClicked;
    public event Action OnPreviousButtonClicked;

    public event Action<int> OnHoverEntered;
    public event Action<int> OnHoverExited;

    [field : SerializeField] public RectTransform RectTransform_This { get; private set; }

    private void OnEnable()
    {
        BindButtonEvents();
    }

    private void BindButtonEvents()
    {
        Button_Background.onClick.AddListener(InvokeBackgroundClicked);
        Button_Exit.onClick.AddListener(InvokeExitButtonClicked);
        Button_Next.onClick.AddListener(InvokeNextButtonClicked);
        Button_Previous.onClick.AddListener(InvokePreviousButtonClicked);

        foreach(HoverButton hoverButton in HoverButtons)
        {
            hoverButton.OnPointerEntered += InvokeHoverEntered;
            hoverButton.OnPointerExited += InvokeHoverExited;
        }
    }

    private void OnDisable()
    {
        UnBindButtonEvents();
    }

    private void UnBindButtonEvents()
    {
        Button_Background.onClick.RemoveListener(InvokeBackgroundClicked);
        Button_Exit.onClick.RemoveListener(InvokeExitButtonClicked);
        Button_Next.onClick.RemoveListener(InvokeNextButtonClicked);
        Button_Previous.onClick.RemoveListener(InvokePreviousButtonClicked);

        foreach (HoverButton hoverButton in HoverButtons)
        {
            hoverButton.OnPointerEntered -= InvokeHoverEntered;
            hoverButton.OnPointerExited -= InvokeHoverExited;
        }
    }

    public void ChangePanel(int index)
    {
        if (PanelList == null || PanelList.Length == 0)
        {
            return;
        }

        for (int i = 0; i < PanelList.Length; i++)
        {
            PanelList[i].SetActive(i == index);
        }
    }

    public void OpenHoverPopup(VideoClip videoClip)
    {
        HoverPopup.Open(videoClip);
    }

    public void CloseHoverPopup()
    {
        HoverPopup.Close();
    }

    private void InvokeBackgroundClicked()
    {
        OnBackgroundClicked?.Invoke();
    }

    private void InvokeExitButtonClicked()
    {
        OnExitButtonClicked?.Invoke();
    }

    private void InvokeNextButtonClicked()
    {
        OnNextButtonClicked?.Invoke();
    }

    private void InvokePreviousButtonClicked() 
    {
        OnPreviousButtonClicked?.Invoke();
    }

    private void InvokeHoverEntered(int i)
    {
        OnHoverEntered?.Invoke(i);
    }

    private void InvokeHoverExited(int i)
    {
        OnHoverExited?.Invoke(i);
    }
}
