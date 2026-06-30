using UnityEngine;

public class PlayerInteraction : MonoBehaviour //플레이어에 붙여서 상호작용할수 있게 하는 클래스(임시)
{
    private IInteractable currentInteractable;
    private IInteractionPromptProvider _currentPromptProvider;

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

                if (hit.collider.TryGetComponent(out IInteractionPromptProvider promptProvider))
                {
                    ShowInteractionPrompt(promptProvider);
                }

                return;
            }
        }
        currentInteractable = null;
        HideInteractionPrompt();
    }

    private void PerformInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void ShowInteractionPrompt(IInteractionPromptProvider promptProvider)
    {
        Debug.Log($"상호작용 프롬프트 실행됨 {promptProvider.InteractionText}");

        if (_currentPromptProvider == promptProvider)
        {
            return;
        }

        _currentPromptProvider = promptProvider;

        UIManager.Instance.OpenInteractionPrompt(InputManager.Instance.InteractionKeyText, promptProvider.InteractionText, promptProvider.PromptTarget);
    }

    private void HideInteractionPrompt()
    {
        if (_currentPromptProvider == null)
        {
            return;
        }

        _currentPromptProvider = null;


        if (UIManager.Instance.IsActiveUI(UIType.InteractionPromptUI))
        {
            UIManager.Instance.CloseUI(UIType.InteractionPromptUI);
        }
    }

}