using Cysharp.Threading.Tasks;
using UnityEngine;

public class CursorManager : BaseMonoManager<CursorManager>
{
    [SerializeField] private Transform Transform_CursorRoot;

    private RectTransform RectTransform_Cursor;
    private Animator Animator_Cursor;

    protected override void Awake()
    {
        base.Awake();
        //CreateCursor().Forget();
    }

    //private async UniTaskVoid CreateCursor()
    //{
    //    GameObject prefab = await LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.CursorUI);

    //    if (prefab == null) return;
    //    if (Transform_CursorRoot == null) return;

    //    GameObject cursorObj = Instantiate(prefab, Transform_CursorRoot);
    //    RectTransform_Cursor = cursorObj.GetComponent<RectTransform>();
    //    Animator_Cursor = cursorObj.GetComponent<Animator>();
    //}

    private void Update()
    {
        if (RectTransform_Cursor == null) return;
        if (Cursor.lockState == CursorLockMode.Locked) return;

        MoveCursor();
        CheckMouseClick();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    private void MoveCursor()
    {
        if (RectTransform_Cursor == null) return;
        RectTransform_Cursor.position = Input.mousePosition;
    }

    public void SetHover(bool isHover)
    {
        if (Animator_Cursor == null) return;
        Animator_Cursor.SetBool("IsHover", isHover);
    }

    private void CheckMouseClick()
    {
        if (Animator_Cursor == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Animator_Cursor.SetBool("IsClick", true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Animator_Cursor.SetBool("IsClick", false);
        }
    }
}