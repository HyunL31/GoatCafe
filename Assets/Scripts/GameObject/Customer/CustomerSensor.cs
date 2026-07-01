using UnityEngine;
using System;

[RequireComponent(typeof(CustomerBase))]
public class CustomerSensor : MonoBehaviour
{
    [SerializeField] private PlayerMoving PlayerMoving;
    [SerializeField] private GameObject GameObject_InteractionUI;
    [SerializeField] private Vector3 Vector3_UIOffset = new Vector3(0f, 0f, 0f);

    public static event Action<CustomerBase> OnStealableEnter;
    public static event Action<CustomerBase> OnStealableExit;
    public static event Action<CustomerSensor> OnStealableInteract;
    public static event Action<CustomerSensor> OnCleanUpInteract;

    private CustomerBase _customer;
    private bool _isPlayerNear = false;
    private bool _isStolen = false;
    private bool _isPromptVisible = false;
    private bool _isPlayerAttacking = false;

    private Camera _camera;

    private void Awake()
    {
        _customer = GetComponent<CustomerBase>();
        //_camera = Camera.main;
        //if (GameObject_InteractionUI != null)
            //GameObject_InteractionUI.transform.localPosition = Vector3_UIOffset;
    }

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_customer.State == CustomerState.Hit)
            {
                if (GameManager.Instance.CurrentDayPhase != DayPhase.Night)
                {
                    return;
                }

                OnCleanUp();
                return;
            }

            OnInteract();
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

        if (_isStolen) return;

        if (_customer.State == CustomerState.Hit &&
            GameManager.Instance.CurrentDayPhase != DayPhase.Night) return;

        _isPlayerNear = true;
        ShowUI(true);
        if (_customer.State == CustomerState.Hit) return;
        if (OnStealableEnter != null)
            OnStealableEnter(_customer);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        bool wasNear = _isPlayerNear;
        _isPlayerNear = false;

        if (wasNear)
            ShowUI(false);

        if (OnStealableExit != null)
            OnStealableExit(_customer);
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
            string interactionText = "훔치기";

            if (_customer.State == CustomerState.Hit)
            {
                interactionText = "치우기";
            }

            UIManager.Instance.OpenInteractionPrompt(InputManager.Instance.InteractionKeyText, interactionText, transform);
        }
        else
        {
            UIManager.Instance.CloseUI(UIType.InteractionPromptUI);
        }
    }

    private void OnCleanUp()
    {
        ShowUI(false);
        _isPlayerNear = false;
        if (OnCleanUpInteract != null)
            OnCleanUpInteract(this);
    }

    private void OnInteract()
    {
        if (_isStolen)
        {
            return;
        }

        ShowUI(false);
        _isPlayerNear = false;

        if (OnStealableInteract != null)
            OnStealableInteract(this);
    }

    public void SetStolen(bool isStolen)
    {
        _isStolen = isStolen;
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

        if (_isStolen)
        {
            return;
        }

        if (_customer.State == CustomerState.Hit &&
            GameManager.Instance.CurrentDayPhase != DayPhase.Night)
        {
            ShowUI(false);
            return;
        }

        ShowUI(true);
    }
}