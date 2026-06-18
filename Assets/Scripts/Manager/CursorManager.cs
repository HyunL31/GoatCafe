using Cysharp.Threading.Tasks;
using UnityEngine;

public class CursorManager : BaseMonoManager<CursorManager>
{
    private RectTransform RectTransform_Cursor;
    private Animator Animator_Cursor;

    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = false;
        CreateCursor().Forget();
    }

    private async UniTaskVoid CreateCursor()
    {
        GameObject prefab = await LoadUtil.Async.LoadPrefabAsync(AddressUtil.Prefab.UI.CursorUI);
        if (prefab == null) return;

        GameObject cursorObj = Instantiate(prefab);
        Animator_Cursor = cursorObj.GetComponentInChildren<Animator>();

        if (Animator_Cursor != null)
        {
            RectTransform_Cursor = Animator_Cursor.GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        if (Cursor.visible == true)
            Cursor.visible = false;

        if (RectTransform_Cursor == null) return;
        if (Cursor.lockState == CursorLockMode.Locked) return;

        MoveCursor();
        CheckMouseClick();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (RectTransform_Cursor != null)
            RectTransform_Cursor.gameObject.SetActive(false);
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        if (RectTransform_Cursor != null)
            RectTransform_Cursor.gameObject.SetActive(true);
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