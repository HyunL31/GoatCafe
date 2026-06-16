using UnityEngine;

public class Store_Area : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StoreManager.Instance.OpenStorePopup();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StoreManager.Instance.OnClickExitBtn();
        }
    }
}
