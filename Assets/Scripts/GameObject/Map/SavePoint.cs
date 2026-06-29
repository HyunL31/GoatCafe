using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private Transform Transform_CafePoint;
    [SerializeField] private Transform Transfomr_PlayerMoving;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveManager.Instance.CurrentPlayerModel.Coin = StoreManager.Instance.Coin;
            SaveManager.Instance.SaveData();
            Transfomr_PlayerMoving.transform.position = Transform_CafePoint.position;

            GameManager.Instance.NextDay();
        }
    }
}
