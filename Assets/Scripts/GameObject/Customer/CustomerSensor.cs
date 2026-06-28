using UnityEngine;
using System;

[RequireComponent(typeof(CustomerBase))]
public class CustomerSensor : MonoBehaviour
{
    [SerializeField] private GameObject GameObject_InteractionUI;

    public static event Action<CustomerBase> OnStealableEnter;
    public static event Action<CustomerBase> OnStealableExit;

    private CustomerBase _customer;
    private bool _isPlayerNear = false;

    private void Awake()
    {
        _customer = GetComponent<CustomerBase>();
    }

    private void Update()
    {
        if (!_isPlayerNear) return;
        if (_customer.State == CustomerState.Hit) return;

        if (Input.GetKeyDown(KeyCode.E))
            OnInteract();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_customer.State == CustomerState.Hit) return;

        _isPlayerNear = true;
        ShowUI(true);

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

    private void OnInteract()
    {
        ShowUI(false);
        _isPlayerNear = false;

        // 이레님(낮 미니게임) 구독해서 미니게임 실행
        if (OnStealableEnter != null)
            OnStealableEnter(_customer);
    }
}