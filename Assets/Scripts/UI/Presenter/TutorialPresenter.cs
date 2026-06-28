using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

public class TutorialPopupPresenter : BasePresenter<TutorialPopupPresenter, TutorialPopup>
{
    public override UIType UIType_This { get; } = UIType.TutorialPopup;

    private TutorialPopup _tutorialPopup;

    private VideoClip[] _videoClips;
    private Vector2 _originalPos;

    private int _panelIndex;

    public override void InitUI(TutorialPopup ui)
    {
        _tutorialPopup = ui;
        _panelIndex = 0;
        _tutorialPopup.ChangePanel(_panelIndex);
        _originalPos = _tutorialPopup.RectTransform_This.anchoredPosition;

        SetUI().Forget();
    }

    public void InitPanelIndex(int index)
    {
        _panelIndex = index;
        _tutorialPopup.ChangePanel(_panelIndex);
    }

    protected override async UniTaskVoid SetUI()
    {
        await LoadAssetAsync();
        LoadData();
        SetUIData();
        SubscribeEvents();

        _tutorialPopup.ActiveTrue();
        UIEffectUtil.SetUISlideUp(_tutorialPopup.RectTransform_This, 0.5f);
    }

    protected override async UniTask LoadAssetAsync()
    {
        //_videoClips = await UniTask.WhenAll
        //    (
        //    new[]
        //    {
        //        LoadUtil.Async.LoadVideoClipAsync(""),
        //        LoadUtil.Async.LoadVideoClipAsync(""),
        //        LoadUtil.Async.LoadVideoClipAsync(""),
        //        LoadUtil.Async.LoadVideoClipAsync(""),
        //        LoadUtil.Async.LoadVideoClipAsync(""),
        //        LoadUtil.Async.LoadVideoClipAsync(""),
        //        LoadUtil.Async.LoadVideoClipAsync("")
        //    }
        //    );
    }

    protected override void LoadData()
    {

    }

    protected override void SetUIData()
    {
    }

    private void SubscribeEvents()
    {
        _tutorialPopup.OnBackgroundClicked += OnClick_Background;
        _tutorialPopup.OnExitButtonClicked += OnClick_ExitButton;
        _tutorialPopup.OnPreviousButtonClicked += OnClick_PreviousButton;
        _tutorialPopup.OnNextButtonClicked += OnClick_NextButton;

        _tutorialPopup.OnHoverEntered += On_HoverEntered;
        _tutorialPopup.OnHoverExited += On_HoverExited;
    }

    private void UnSubscribeEvents()
    {
        _tutorialPopup.OnBackgroundClicked -= OnClick_Background;
        _tutorialPopup.OnExitButtonClicked -= OnClick_ExitButton;
        _tutorialPopup.OnPreviousButtonClicked -= OnClick_PreviousButton;
        _tutorialPopup.OnNextButtonClicked -= OnClick_NextButton;

        _tutorialPopup.OnHoverEntered -= On_HoverEntered;
        _tutorialPopup.OnHoverExited -= On_HoverExited;
    }

    private void OnClick_Background()
    {
        UnSubscribeEvents();
        CloseWithAnimation();

    }

    private void OnClick_PreviousButton()
    {
        int changeIndex = _panelIndex - 1;

        if (changeIndex >= 0)
        {
            _panelIndex = changeIndex;
            _tutorialPopup.ChangePanel(_panelIndex);
        }
    }

    private void OnClick_NextButton()
    {
        int changeIndex = _panelIndex + 1;

        if (changeIndex < _tutorialPopup.PanelList.Length)
        {
            _panelIndex = changeIndex;
            _tutorialPopup.ChangePanel(_panelIndex);
        }
    }

    private void OnClick_ExitButton()
    {
        UnSubscribeEvents();
        CloseWithAnimation();
    }

    private void On_HoverEntered(int index)
    {
        _tutorialPopup.OpenHoverPopup(_videoClips[index]);
    }

    private void On_HoverExited(int index)
    {
        _tutorialPopup.CloseHoverPopup();
    }

    private void CloseWithAnimation()
    {
        UnSubscribeEvents();
        UIEffectUtil.SetUISlideDown(
            _tutorialPopup.RectTransform_This, _originalPos, 0.5f,
            () => UIManager.Instance.CloseUI(UIType.TutorialPopup));
    }
}
