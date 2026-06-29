using UnityEngine;

public class PlayerInteraction : MonoBehaviour //플레이어에 붙여서 상호작용할수 있게 하는 클래스(임시)
{
    private IInteractable currentInteractable;

    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayer;

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnInteractPressed += PerformInteraction;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnInteractPressed -= PerformInteraction;
        }
    }

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
                return;
            }
        }
        currentInteractable = null;
    }

    private void PerformInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }
}