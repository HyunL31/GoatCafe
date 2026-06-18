using UnityEngine;

public class ButtonSoundComponent : MonoBehaviour
{
    [SerializeField] private BaseButton BaseButton;



    private void OnEnable()
    {
        SubscribeEvent();
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }

    private void SubscribeEvent()
    {
        BaseButton.OnButtonClicked += PlaySound;
    }

    private void UnsubscribeEvent()
    {
        BaseButton.OnButtonClicked -= PlaySound;
    }

    private void PlaySound()
    {
        //대충 음악 재생하는 로직
    }
}
