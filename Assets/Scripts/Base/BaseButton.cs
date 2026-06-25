using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    [SerializeField] protected Button Button_This;
    [SerializeField] protected Image Image_Button;
    [SerializeField] protected TMP_Text Text_Button;

    [SerializeField] protected bool IsText;
}