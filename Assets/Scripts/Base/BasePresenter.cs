using Cysharp.Threading.Tasks;
using UnityEngine;

public class BasePresenter
{

}

public abstract class BasePresenter<TUIModel, TPresenter, TUIView> : BasePresenter where TUIModel : UIDataModel where TPresenter : BasePresenter<TUIModel, TPresenter, TUIView> where TUIView : BaseUI<TUIView>
{
    public abstract UIType UIType_This { get; }

    protected TUIModel Model { get; private set; }

    protected TUIView View { get; private set; }

    public virtual void InitUI(TUIModel model, TUIView view)
    {
        Model = model;
        View = view;
        SetUI().Forget();
    }

    protected abstract UniTaskVoid SetUI();

    protected abstract UniTask LoadAndSetAssetAsync();

    protected abstract void LoadUIData();

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
