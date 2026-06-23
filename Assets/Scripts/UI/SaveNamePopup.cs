using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveNamePopup : BaseUI
{
    [SerializeField] Button Button_Close;
    [SerializeField] Button Button_Confirm;
    [SerializeField] TMP_InputField InputField_Name;

    private void Awake()
    {
        Button_Close.onClick.AddListener(OnClickClose);
        Button_Confirm.onClick.AddListener(OnClickConfirm);
    }

    private void OnEnable()
    {
        InputField_Name.text = "";
    }

    private void OnClickClose()
    {

    }

    private void OnClickConfirm()
    {
        //SaveManager.Instance.LoadOrCreatePlayerData(InputField_Name.text);
    }
}
