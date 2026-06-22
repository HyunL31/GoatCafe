using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private Button Button_Discard;

    private string _slotID;
    private PlayerModel _slotModel;

    private void Awake()
    {
        Button_Discard.onClick.AddListener(OnClickDiscard);
    }

    public void InitSlot(string slotID)
    {
        _slotID = slotID;
        _slotModel = SaveManager.Instance.RequestLoadData(slotID);
    }

    private void OnClickConfirm()
    {
        // 게임 시작
    }

    private void OnClickDiscard()
    {
        string path = Path.Combine(Application.persistentDataPath, $"GOAT{_slotID}.json");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        GameManager.Instance.SlotIndex.Remove(_slotID);
        SaveManager.Instance.OnSaveClear?.Invoke();

        Destroy(gameObject);
    }
}