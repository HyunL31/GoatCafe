using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [SerializeField] private Transform Transform_HomePosition;
    [SerializeField] private Transform Transform_CafePosition;
    [SerializeField] private GameObject GameObject_Goat;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GoHome()
    {
        if (GameObject_Goat == null) return;
        GameObject_Goat.transform.position = Transform_HomePosition.position;
    }

    public void GoCafe()
    {
        if (GameObject_Goat == null) return;
        GameObject_Goat.transform.position = Transform_CafePosition.position;
    }
}
