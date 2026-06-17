using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Prefab_NormalCustomer;
    [SerializeField] private List<Transform> Transform_Waypoints = new List<Transform>();
    [SerializeField] private List<Transform> Transform_Spawnpoints = new List<Transform>();
    [SerializeField] private int Int_SpawnCouont = 3;

    private void Start()
    {
        SpawnCustomer();
    }

    private void SpawnCustomer()
    {
        for (int i=0; i< Int_SpawnCouont; i++)
        {
            Transform spawnPoint = Transform_Spawnpoints[Random.Range(0, Transform_Spawnpoints.Count)];
            GameObject obj = Instantiate(Prefab_NormalCustomer, spawnPoint.position, Quaternion.identity);

            NormalCustomer customer = obj.GetComponent<NormalCustomer>();
            if (customer == null) return;

            customer.Initialize(CustomerType.Normal, CustomerRace.Human, 2f, 5f, 90f, Transform_Waypoints);
        }
    }
}
