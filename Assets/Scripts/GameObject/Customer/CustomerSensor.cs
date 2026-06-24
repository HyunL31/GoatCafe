using UnityEngine;
using System;

[RequireComponent(typeof(CustomerBase))]
public class CustomerSensor : MonoBehaviour
{
    public static event Action<CustomerBase> OnStealableEnter;
    public static event Action<CustomerBase> OnStealableExit;

    private CustomerBase _customer;

    private void Awake()
    {
        _customer = GetComponent<CustomerBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_customer.State == CustomerState.Hit) return;

        Debug.Log("훔치기 가능");
        if (OnStealableEnter != null)
            OnStealableEnter(_customer);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("훔치기 불가");
        if (OnStealableExit != null)
            OnStealableExit(_customer);
    }
}