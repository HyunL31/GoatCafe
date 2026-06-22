using UnityEngine;
using UnityEngine.UI;

public class SaveUI : BaseUI
{
    [SerializeField] private Button Button_Close;
    [SerializeField] private Transform Transform_SlotParent;
    [SerializeField] private GameObject Prefab_SaveSlot;
    [SerializeField] private GameObject GameObject_InfoText;

    private void Awake()
    {
        Button_Close.onClick.AddListener(OnClickClose);

        SaveManager.Instance.OnSaveClear += ClearSaveSlot;
    }

    private void OnEnable()
    {
        foreach (Transform child in Transform_SlotParent)
        {
            Destroy(child.gameObject);
        }

        RefreshSlot();
    }

    private void RefreshSlot()
    {
        ClearSaveSlot();

        foreach (string slotName in SaveManager.Instance.SlotIndex)
        {
            GameObject prefab = Instantiate(Prefab_SaveSlot, Transform_SlotParent);
            SaveSlot saveSlot = prefab.GetComponent<SaveSlot>();
            saveSlot.InitSlot(slotName);
        }
    }

    private void ClearSaveSlot()
    {
        if (SaveManager.Instance.SlotIndex.Count == 0)
        {
            GameObject_InfoText.SetActive(true);
        }
        else
        {
            GameObject_InfoText.SetActive(false);
        }
    }

    private void OnClickClose()
    {
        // UIExtension 이후
    }
}