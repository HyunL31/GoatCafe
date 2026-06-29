using UnityEngine;

public class LightSetting : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnDayPhaseChanged += ChangeDayPhase;
    }

    private void ChangeDayPhase(DayPhase phase)
    {
        UpdateLightSetting(phase);
    }

    private void UpdateLightSetting(DayPhase phase)
    {
        bool isDay = (phase == DayPhase.Day);
        this.gameObject.SetActive(isDay);
    }
}