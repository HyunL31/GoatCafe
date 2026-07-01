using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseButton : BaseSoundButton
{
    [SerializeField] protected Button Button_This;
    [SerializeField] protected Image Image_Button;
    [SerializeField] protected TMP_Text Text_Button;

    [SerializeField] protected bool IsText;

    private void Awake()
    {
        Button_This.onClick.AddListener(base.PlayClickSound);
    }
}