using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : BaseUI
{
    [SerializeField] private Button Button_Close;
    [SerializeField] private Transform Transform_SlotParent;
    [SerializeField] private GameObject GameObject_InfoText;

    private HashSet<int> _slotID = new HashSet<int>();

    private void Awake()
    {
        Button_Close.onClick.AddListener(OnClickClose);

        SaveManager.Instance.OnSaveClear = ClearSaveSlot;
    }

    private void OnEnable()
    {
        foreach (Transform child in Transform_SlotParent)
        {
            Destroy(child.gameObject);
        }

        _slotID.Clear();

        RefreshSlot().Forget();
    }

    private async UniTask RefreshSlot()
    {
        ClearSaveSlot();

        foreach (int i in GameManager.Instance.SlotIndex)
        {
            if (_slotID.Contains(i))
            {
                continue;
            }

            GameObject prefab = await LoadUtil.Async.LoadPrefabAsync("Prefabs/UI/SaveSlot");
            SaveSlot saveSlot = prefab.GetComponent<SaveSlot>();
            saveSlot.InitSlot(i);

            _slotID.Add(i);
        }
    }

    private void ClearSaveSlot()
    {
        if (GameManager.Instance.SlotIndex.Count == 0)
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