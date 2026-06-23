using UnityEngine;
using UnityEngine.EventSystems;

public class ItemIconHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    // 부모인 StoreItemSlot이 가진 팝업을 참조하게 함
    private StoreItemSlot parentSlot;

    private void Awake()
    {
        // 부모의 StoreItemSlot 스크립트를 가져옴
        parentSlot = GetComponentInParent<StoreItemSlot>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StoreManager.Instance.SetitemDescPopup(true, parentSlot.itemBaseData);
        StoreManager.Instance.UpdateBuffPopupPosition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StoreManager.Instance.SetitemDescPopup(false, parentSlot.itemBaseData);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        StoreManager.Instance.UpdateBuffPopupPosition();
    }
}
