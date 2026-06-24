using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private Transform Transform_CafePoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveManager.Instance.SaveData();
            other.transform.position = Transform_CafePoint.position;

            GameManager.Instance.NextDay();
        }
    }
}
