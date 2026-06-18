using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("GOAT")]
    [SerializeField] private Transform Transform_Goat;

    [Header("카메라 위치 설정")]
    [SerializeField] private Vector3 _offset = new Vector3(0, 5, -7);

    [Header("회전 설정")]
    [SerializeField] private float _rotateSpeed = 3f;

    private float _rotationY = 0f;
    private float _rotationX = 20f;

    private void Start()
    {
        if (CursorManager.Instance != null)
        {
            CursorManager.Instance.LockCursor();
        }
    }

    private void Update()
    {
        // ESC 누르면 커서 풀기 (테스트용)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CursorManager.Instance != null)
            {
                CursorManager.Instance.UnlockCursor();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (CursorManager.Instance != null)
            {
                CursorManager.Instance.LockCursor();
            }
        }

        if (Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationY += mouseX * _rotateSpeed;
        _rotationX -= mouseY * _rotateSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -10f, 60f);

    }

    private void LateUpdate()
    {
        if (Transform_Goat == null) return;

        Quaternion rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
        transform.position = Transform_Goat.position + rotation * _offset;
        transform.LookAt(Transform_Goat.position);
    }
}