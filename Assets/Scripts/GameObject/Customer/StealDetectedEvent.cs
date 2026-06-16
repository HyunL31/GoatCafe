using System;
using UnityEngine;

public class StealDetectedEvent : MonoBehaviour
{
    public CustomerBase Detector { get; private set; }

    public StealDetectedEvent(CustomerBase detector)
    {
        Detector = detector;
    }

    public static event Action<StealDetectedEvent> OnDetected;

    public static void Invoke(StealDetectedEvent e)
    {
        if (OnDetected != null)
            OnDetected(e);
    }
}
