using UnityEngine;
using UnityEngine.EventSystems;

public enum ZoneType
{
    Paper,
    Plastic
}

public class RecycleZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private ZoneType zoneType;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject == null)
        {
            return;
        }

        TrashItem trashItem = droppedObject.GetComponent<TrashItem>();

        if (trashItem == null)
        {
            return;
        }

        trashItem.CurrentZone = zoneType;

        Debug.Log($"{droppedObject.name}이 {zoneType}에 드롭됨");
    }
}