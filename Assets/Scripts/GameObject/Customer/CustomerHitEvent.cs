using System;
using UnityEngine;

public class CustomerHitEvent : MonoBehaviour
{
    public CustomerBase Customer {  get; private set; }
    public CustomerType Type { get; private set; }
    public int SocoreChange { get; private set; }

    public CustomerHitEvent(CustomerBase customer, CustomerType type, int socoreChange)
    {
        Customer = customer;
        Type = type;
        SocoreChange = socoreChange;
    }

    public static event Action<CustomerHitEvent> OnHitResolved;

    public static void Invoke(CustomerHitEvent e)
    {
        if (OnHitResolved != null)
            OnHitResolved(e);
    }
}
