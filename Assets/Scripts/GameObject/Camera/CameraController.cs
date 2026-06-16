using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("GOAT")]
    [SerializeField] private Transform Transform_Goat;

    [Header("카메라 위치 설정")]
    [SerializeField] private Vector3 _offset = new Vector3(0, 5, -7);

    private void LateUpdate()
    {
        if (Transform_Goat == null) return;

        transform.position = Transform_Goat.position + _offset;
    }
}