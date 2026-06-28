using UnityEngine;
using System;

[RequireComponent(typeof(CustomerBase))]
public class CustomerSensor : MonoBehaviour
{
    [SerializeField] private GameObject GameObject_InteractionUI;
    [SerializeField] private Vector3 Vector3_UIOffset = new Vector3(0f, 0f, 0f);

    public static event Action<CustomerBase> OnStealableEnter;
    public static event Action<CustomerBase> OnStealableExit;
    public static event Action<CustomerBase> OnStealableInteract;
    public static event Action<CustomerBase> OnCleanUpInteract;

    private CustomerBase _customer;
    private bool _isPlayerNear = false;

    private Camera _camera;

    private void Awake()
    {
        _customer = GetComponent<CustomerBase>();
        _camera = Camera.main;
        if (GameObject_InteractionUI != null)
            GameObject_InteractionUI.transform.localPosition = Vector3_UIOffset;
    }

    private void Update()
    {
        if (GameObject_InteractionUI != null && GameObject_InteractionUI.activeSelf)
        {
            GameObject_InteractionUI.transform.LookAt(
                GameObject_InteractionUI.transform.position + _camera.transform.rotation * Vector3.forward,
                _camera.transform.rotation * Vector3.up
            );
        }

        if (!_isPlayerNear) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_customer.State == CustomerState.Hit)
            {
                if (GameManager.Instance.CurrentDayPhase != DayPhase.Night) return;
                OnCleanUp();
            }
            else
                OnInteract();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

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

        _isPlayerNear = false;
        ShowUI(false);

        if (OnStealableExit != null)
            OnStealableExit(_customer);
    }

    private void ShowUI(bool show)
    {
        if (GameObject_InteractionUI == null) return;
        GameObject_InteractionUI.SetActive(show);
    }

    private void OnCleanUp()
    {
        ShowUI(false);
        _isPlayerNear = false;
        if (OnCleanUpInteract != null)
            OnCleanUpInteract(_customer);
    }

    private void OnInteract()
    {
        ShowUI(false);
        _isPlayerNear = false;

        // 이레님(낮 미니게임) 구독해서 미니게임 실행
        if (OnStealableInteract != null)
            OnStealableInteract(_customer);
    }
}