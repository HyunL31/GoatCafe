using UnityEngine;

[RequireComponent(typeof(CustomerBase))]
public class CustomerSensor : MonoBehaviour
{
    [SerializeField] private Transform Transform_Goat;

    private CustomerBase _customer;

    private void Awake()
    {
        _customer = GetComponent<CustomerBase>();
    }

    private void Update()
    {
        if (Transform_Goat == null) return;
        _customer.TryDetectGoat(Transform_Goat);
    }

    public void SetGoatTransform(Transform goatTransform)
    {
        Transform_Goat = goatTransform;
    }
}
