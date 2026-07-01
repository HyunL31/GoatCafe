using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseSoundButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClickSound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
        if (CursorManager.Instance != null)
            CursorManager.Instance.SetHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CursorManager.Instance != null)
            CursorManager.Instance.SetHover(false);
    }

    private void PlayButtonSound(string soundKey)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(soundKey).Forget();
        }
        else
        {
            this.LogWarning("사운드 매니저를 찾을 수 없습니다!!");
        }
    }

    public void PlayClickSound()
    {
        int randomIndex = Random.Range(1, 5);
        string randomKey = $"Audio/SFX/UISound/Click_{randomIndex}";
        PlayButtonSound(randomKey);
    }

    private void PlayHoverSound()
    {
        int randomIndex = Random.Range(1, 3);
        string randomKey = $"Audio/SFX/UISound/Hover_{randomIndex}";
        PlayButtonSound(randomKey);
    }
}