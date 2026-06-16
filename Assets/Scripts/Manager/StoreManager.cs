using UnityEngine;
using UnityEngine.UI;

public class StoreManager : BaseMonoManager<StoreManager>
{
    [Header ("Store Popup")]
    [SerializeField] private GameObject _storePopup;
    [SerializeField] private Button _exitButton;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(OnClickExitBtn);
    }

    public void OnClickExitBtn()
    {
        _storePopup.SetActive(false);

        SetCursorState(false);
    }

    public void OpenStorePopup()
    {
        _storePopup.SetActive(true);

        SetCursorState(true);
    }



    ////// 아래는 임시로 만든 함수 (다른곳에서 만들어지면 지울 예정)

    //임시 마우스커서 활성화/비활성화 함수
    public void SetCursorState(bool state)
    {
        Cursor.visible = state;

        if (state) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
    } 
}
