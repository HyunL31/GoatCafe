using UnityEngine;
using UnityEngine.EventSystems;
public enum TrashType
{
    Paper,
    Plastic
}


public class TrashItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
// IBeginDragHandler -> 드래그 시작 감지
// IDragHandler -> 드래그 중 감지
// IEndDragHandler -> 드래그 끝 감지
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    [SerializeField] private TrashType trashtype;
    public TrashType TrashType
    {
        get { return trashtype; }
        set { trashtype = value; }
    }


    [SerializeField] private ZoneType currentzone;
    public ZoneType CurrentZone
    {
        get { return currentzone; }
        set { currentzone = value; }
    }


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작할 때 실행
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;

        Debug.Log($"{gameObject.name} 드래그 시작 / 타입: {TrashType}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 중일 때 계속 실행
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Debug.Log($"{gameObject.name} 드래그 중");
        // eventData.delta -> 이번 프레임에 마우스가 움직인 거리
        // canvas.scaleFactor -> 캔버스 보정
        // rectTransform.anchoredPosition -> 값 만큼 이동 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 끝났을 때 실행
        canvasGroup.blocksRaycasts = true;
        Debug.Log($"{gameObject.name} 드래그 종료");
    }

}