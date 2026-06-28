using UnityEngine;
using System;

public class Trash : MonoBehaviour
{
    [SerializeField] private GameObject GameObject_InteractionUI;

    public static event Action<Trash> OnTrashEnter;
    public static event Action<Trash> OnTrashExit;
    public static event Action<Trash> OnTrashInteract;

    private bool _isPlayerNear = false;

    private void Update()
    {
        if (!_isPlayerNear) return;
        if (GameManager.Instance.CurrentDayPhase != DayPhase.Night) return;
        if (Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance.CurrentDayPhase != DayPhase.Night) return;
        _isPlayerNear = true;
        ShowUI(true);
        if (OnTrashEnter != null)
            OnTrashEnter(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _isPlayerNear = false;
        ShowUI(false);
        if (OnTrashExit != null)
            OnTrashExit(this);
    }

    private void ShowUI(bool show)
    {
        if (GameObject_InteractionUI == null) return;
        GameObject_InteractionUI.SetActive(show);
    }

    public void Interact()
    {
        ShowUI(false);
        _isPlayerNear = false;
        if (OnTrashInteract != null)
            OnTrashInteract(this);
    }
}