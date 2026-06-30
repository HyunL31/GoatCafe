using Cysharp.Threading.Tasks;
using UnityEngine;

public class BasePresenter
{

}

public abstract class BasePresenter<TUIModel, TPresenter, TUIView> : BasePresenter where TUIModel : UIDataBase where TPresenter : BasePresenter<TUIModel, TPresenter, TUIView> where TUIView : BaseUI<TUIView>
{
    public abstract UIType UIType_This { get; }

    public abstract void InitUI(TUIModel uiModel, TUIView ui);

    protected abstract UniTaskVoid SetUI();

    protected abstract UniTask LoadAssetAsync();

    protected abstract void LoadUIData();

    protected abstract void SubscribeEvents();

    protected abstract void UnsubscribeEvents();

    protected void Log(string text)
    {
        Debug.Log($"{this} : " + text);
    }

    protected void LogWarning(string text)
    {
        Debug.LogWarning($"{this} : " + text);
    }

    protected void LogError(string text)
    {
        Debug.LogError($"{this} : " + text);
    }

}
