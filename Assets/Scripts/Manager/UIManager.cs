using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public enum UIType : byte
{
    MainMenuUI
}

public enum UIRootType : byte
{
    Background,
    Main,
    Content,
    Popup,
    Front,
    ETC,
}

public partial class UIManager : BaseMonoManager<UIManager>
{
    [SerializeField] private Canvas Background;
    [SerializeField] private Canvas MainCanvas;
    [SerializeField] private Canvas ContentCanvas;
    [SerializeField] private Canvas PopupCanvas;
    [SerializeField] private Canvas FrontCanvas;
    [SerializeField] private Canvas ETCCanvas;

    private Dictionary<UIType, BaseUI> m_uiDic = new();

    private HashSet<UIType> m_activeUI = new();
    private HashSet<UIRootType> m_activeCanvas = new();


    public T CreateUI<T>(UIType uiType) where T : BaseUI<T>
    {
        if (m_uiDic.TryGetValue(uiType, out BaseUI baseUI))
        {
            return baseUI as T;
        }

        string address = GetAddress(uiType);

        if (string.IsNullOrEmpty(address))
        {
            return default;
        }

        Canvas canvas = GetCanvas(GetUIRootType(uiType));

        if (canvas == null)
        {
            return default;
        }

        GameObject prefab = LoadUtil.Sync.LoadPrefab(address);

        if (prefab == null)
        {
            return default;
        }

        GameObject ui = Instantiate(prefab, canvas.transform);


        T newBaseUI = ui.GetComponent<T>();

        if (newBaseUI == null)
        {
            this.LogError($"{ui.name}에 {typeof(T)} 컴포넌트가 없습니다!!");
            return default;
        }

        newBaseUI.ActiveFalse();
        m_uiDic.Add(uiType, newBaseUI);
        return newBaseUI;
    }

    public async UniTask<T> CreateUIAsync<T>(UIType uiType) where T : BaseUI<T>
    {

        if (m_uiDic.TryGetValue(uiType, out BaseUI baseUI))
        {
            return baseUI as T;
        }

        string address = GetAddress(uiType);

        if (string.IsNullOrEmpty(address))
        {
            return null;
        }

        Canvas canvas = GetCanvas(GetUIRootType(uiType));

        if (canvas == null)
        {
            return null;
        }

        GameObject prefab = await LoadUtil.Async.LoadPrefabAsync(address);

        if (prefab == null)
        {
            return null;
        }
        
        GameObject ui = Instantiate(prefab, canvas.transform);


        T newBaseUI = ui.GetComponent<T>();

        if (newBaseUI == null)
        {
            this.LogError($"{ui.name}에 {typeof(T)} 컴포넌트가 없습니다!!");
            return null;
        }

        newBaseUI.ActiveFalse();
        m_uiDic.Add(uiType, newBaseUI);
        return newBaseUI;
    }

    public void CloseUI(UIType uiType)
    {
        if (m_activeUI.Contains(uiType) == false)
        {
            this.LogWarning($"{uiType}의 UI는 열려있지 않습니다!!");
            return;
        }

        if (m_uiDic.TryGetValue(uiType, out BaseUI baseUI) == false)
        {
            this.LogError($"{uiType}의 UI를 찾을 수 없습니다!!");
            return;
        }

        baseUI.ActiveFalse();
        m_activeUI.Remove(uiType);
        m_activeCanvas.Remove(GetUIRootType(uiType));
    }

    private string GetAddress(UIType uiType)
    {
        switch (uiType)
        {
            case UIType.MainMenuUI:
                {
                    return AddressUtil.Prefab.UI.MainMenuUI;
                }
            default:
                {
                    this.LogError($"{uiType}에 알맞는 Path가 없습니다!!");
                    return null;
                }
        }
    }

    private UIRootType GetUIRootType(UIType uiType)
    {
        switch (uiType)
        {
            case UIType.MainMenuUI:
                {
                    return UIRootType.Main;
                }
            default:
                {
                    this.LogError($"{uiType}에 알맞는 UIRootType이 없습니다!!");
                    return default;
                }
        }
    }

    private Canvas GetCanvas(UIRootType uiRootType)
    {
        switch (uiRootType)
        {
            case UIRootType.Main:
                {
                    return MainCanvas;
                }
            default:
                {
                    this.LogError($"{uiRootType}에 알맞는 Canvas가 없습니다!!");
                    return null;
                }
        }
    }
}
