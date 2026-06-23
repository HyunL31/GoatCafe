using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private Button Button_Discard;
    [SerializeField] private Button Button_Confirm;

    private string _slotID;

    private void Awake()
    {
        Button_Discard.onClick.AddListener(OnClickDiscard);
        Button_Confirm.onClick.AddListener(OnClickConfirm);
    }

    public void InitSlot(string slotID)
    {
        _slotID = slotID;
    }

    private void OnClickConfirm()
    {
        SaveManager.Instance.LoadOrCreatePlayerData(_slotID);
    }

    private void OnClickDiscard()
    {
        string path = Path.Combine(Application.persistentDataPath, $"GOAT{_slotID}.json");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        SaveManager.Instance.SlotIndex.Remove(_slotID);
        SaveManager.Instance.OnSaveClear?.Invoke();

        Destroy(gameObject);
    }
}