using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveDataButton : BaseButton
{
    [SerializeField] private Image Image_Background;

    [SerializeField] private TMP_Text Text_DataName;

    [SerializeField] private TMP_Text Text_DayTitle;
    [SerializeField] private TMP_Text Text_GoldTitle;
    [SerializeField] private TMP_Text Text_StaminaTitle;

    [SerializeField] private TMP_Text Text_DayData;
    [SerializeField] private TMP_Text Text_GoldData;
    [SerializeField] private TMP_Text Text_StaminaData;

    private void Awake()
    {
        OnSetDataEnded += SetData;
    }

    private void OnDestroy()
    {
        OnSetDataEnded -= SetData;
    }

    private void SetData()
    {
        TMP_FontAsset font = Text_Button.font;

        Text_DataName.font = font;

        Text_DayTitle.font = font;
        Text_GoldTitle.font = font;
        Text_StaminaTitle.font = font;

        Text_DayData.font = font;
        Text_GoldData.font = font;
        Text_StaminaData.font = font;
    }
    
    public void SetSaveData(PlayerModel playerModel)
    {
        Text_DataName.text = "";
        Text_DayData.text = playerModel.Day.ToString();
        Text_GoldData.text = playerModel.Gold.ToString();
        Text_StaminaData.text = playerModel.Stamina.ToString();
    }

    
}
