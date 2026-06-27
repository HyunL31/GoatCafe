using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private Transform Transform_CafePoint;
    [SerializeField] private Transform Transform_PlayerMoving;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveManager.Instance.SaveData();
            Transform_PlayerMoving.transform.position = Transform_CafePoint.position;

            GameManager.Instance.NextDay();
        }
    }
}
