using UnityEngine;
using UnityEngine.EventSystems;

public enum ZoneType
{
    Start,
    Paper,
    Plastic
}

public class RecycleZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private ZoneType zoneType;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop 실행됨");

        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject == null)
        {
            Debug.Log("pointerDrag가 null임");
            return;
        }

        TrashItem trashItem = droppedObject.GetComponent<TrashItem>();

        if (trashItem == null)
        {
            Debug.Log("TrashItem 컴포넌트 못 찾음");
            return;
        }

        trashItem.CurrentZone = zoneType;

        Debug.Log("CurrentZone 변경됨: " + trashItem.CurrentZone);
    }
}