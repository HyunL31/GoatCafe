using UnityEngine;
using System;

public class Trash : MonoBehaviour
{
    [SerializeField] private PlayerMoving PlayerMoving;
    [SerializeField] private GameObject GameObject_InteractionUI;
    [SerializeField] private Vector3 Vector3_UIOffset = new Vector3(0f, 0.5f, 0f);

    public static event Action<Trash> OnTrashEnter;
    public static event Action<Trash> OnTrashExit;
    public static event Action<Trash> OnTrashInteract;

    private bool _isPlayerNear = false;
    private bool _isPromptVisible = false;
    private bool _isPlayerAttacking = false;

    private Camera _camera;

    //private void Awake()
    //{
    //    _camera = Camera.main;
    //    if (GameObject_InteractionUI != null)
    //        GameObject_InteractionUI.transform.localPosition = Vector3_UIOffset;
    //}

    private void OnEnable()
    {
        if (PlayerMoving != null)
        {
            PlayerMoving.OnAttackStateChanged += HandleAttackStateChanged;
        }
    }

    private void OnDisable()
    {
        if (PlayerMoving != null)
        {
            PlayerMoving.OnAttackStateChanged -= HandleAttackStateChanged;
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            ShowUI(false);
            return;
        }

        if (!_isPlayerNear)
        {
            return;
        }

        if (_isPlayerAttacking)
        {
            return;
        }

        if (GameManager.Instance.CurrentDayPhase != DayPhase.Night)
        {
            ShowUI(false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerMoving != null && PlayerMoving.IsAttacking)
        {
            ShowUI(false);
            return;
        }

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
        if (_isPromptVisible == show)
        {
            return;
        }

        _isPromptVisible = show;

        if (show)
        {
            UIManager.Instance.OpenInteractionPrompt(InputManager.Instance.InteractionKeyText, "치우기", transform);
        }
        else
        {
            UIManager.Instance.CloseUI(UIType.InteractionPromptUI);
        }
    }

    public void Interact()
    {
        ShowUI(false);
        _isPlayerNear = false;
        if (OnTrashInteract != null)
            OnTrashInteract(this);
    }

    private void HandleAttackStateChanged(bool isAttacking)
    {
        _isPlayerAttacking = isAttacking;

        if (_isPlayerAttacking)
        {
            ShowUI(false);
        }
        else
        {
            RefreshPrompt();
        }
    }

    private void RefreshPrompt()
    {
        if (!_isPlayerNear)
        {
            return;
        }

        if (GameManager.Instance.CurrentDayPhase != DayPhase.Night)
        {
            ShowUI(false);
            return;
        }

        ShowUI(true);
    }
}