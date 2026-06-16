using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : BaseMonoManager<StoreManager>
{
    [Header("Prefab")]
    [SerializeField] private GameObject _storeItems;

    [Header ("Store Popup")]
    [SerializeField] private GameObject _storePopup;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private Transform _contentParent;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(OnClickExitBtn);
        ItemDataBase.Instance.LoadAllItems();
        InitStorePopup();
    }

    private void InitStorePopup()
    {
        foreach (Transform child in _contentParent)
            Destroy(child.gameObject);

        foreach (var item in ItemDataBase.Instance.PermanentList)
        {
            GameObject slotObj = Instantiate(_storeItems, _contentParent);

            slotObj.GetComponent<StoreItemSlot>().Setup(item);
        }
    }

    public void OnClickExitBtn()
    {
        _storePopup.SetActive(false);

        SetCursorState(false);
    }

    public void OpenStorePopup()
    {
        UpdateStorePopup();

        _storePopup.SetActive(true);

        SetCursorState(true);
    }

    public void HandlePurchase(ItemBase itemData, Button button)
    {
        if(!itemData.Buy())
        {
            // 코인이 부족하다는 메시지 띄우기
            return;
        }


        if (itemData is PermanentItem data)  // 일단은 영구적인 효과의 아이템만 구현
        {
            UpdateStorePopup();
            button.interactable = false;
            switch (data.effectType)
            {
                case EffectType.UpgradeHealth:
                    Debug.Log("UpgradeHealth 구매됨");
                    break;
                case EffectType.SpeedUp:
                    Debug.Log("SpeedUp 구매됨");
                    break;
                case EffectType.MiniGamePointDouble:
                    Debug.Log("MiniGamePointDouble 구매됨");
                    break;
            }
        }

    }




    ////// 아래는 임시로 만든 함수 or 변수 (다른곳에서 만들어지면 지울 예정)
    
    //임시 소유 코인
    public int _coins = 999999;

    public void SpendCoins(int amount)
    {
        if(_coins >= amount)
        {
            _coins -= amount;
        }
    }

    //임시 마우스커서 활성화/비활성화 함수
    public void SetCursorState(bool state)
    {
        Cursor.visible = state;

        if (state) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
    } 

    //임시 팝업 UI 업데이트
    public void UpdateStorePopup()
    {
        _coinText.text = _coins.ToString();
    }
}
