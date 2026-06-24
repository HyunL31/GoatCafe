using UnityEngine;
using System;

public class Trash : MonoBehaviour
{
    public static event Action<Trash> OnTrashEnter;
    public static event Action<Trash> OnTrashExit;

    public static event Action<Trash> OnTrashInteract;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (OnTrashEnter != null)
            OnTrashEnter(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (OnTrashExit != null)
            OnTrashExit(this);
    }

    public void Interact()
    {
        if (OnTrashInteract != null)
            OnTrashInteract(this);
    }
}
