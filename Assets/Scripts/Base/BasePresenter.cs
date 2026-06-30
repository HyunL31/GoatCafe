using Cysharp.Threading.Tasks;
using UnityEngine;

public class BasePresenter
{

}

public abstract class BasePresenter<TPresenter, TUI> : BasePresenter where TPresenter : BasePresenter<TPresenter, TUI> where TUI : BaseUI<TUI>
{
    public abstract UIType UIType_This { get; }

    public abstract void InitUI(TUI ui);

    protected async virtual UniTaskVoid SetUI()
    {
        return;
    }

    protected async virtual UniTask LoadAssetAsync()
    {
        return;
    }

    protected abstract void LoadData();

    protected abstract void SetUIData();

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
