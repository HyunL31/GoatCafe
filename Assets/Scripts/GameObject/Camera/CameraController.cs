using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("GOAT")]
    [SerializeField] private Transform Transform_Goat;

    [Header("카메라 위치 설정")]
    [SerializeField] private Vector3 _offset = new Vector3(-0.2f, 3f, -5f);

    [Header("회전 설정")]
    [SerializeField] private float _rotateSpeed = 3f;

    [Header("벽 충돌 설정")]
    [SerializeField] private LayerMask _collisionLayer;
    [SerializeField] private float _collisionPadding = 0.3f;

    private float _rotationY = 0f;
    private float _rotationX = 20f;

    private void Start()
    {
        if (Transform_Goat == null)
        {
            this.LogError("Transform_Goat가 연결되지 않았습니다! 인스펙터를 확인해주세요.");
        }
    }

    private void Update()
    {
        HandleCursor();

        if (Cursor.lockState != CursorLockMode.Locked) return;

        RotateCamera();
    }

    private void LateUpdate()
    {
        if (Transform_Goat == null) return;

        Quaternion rotation = GetCameraRotation();

        Vector3 targetPosition = GetTargetPosition(rotation);

        targetPosition = AvoidWall(targetPosition);

        transform.position = targetPosition;
        transform.LookAt(Transform_Goat.position);
    }

    private void HandleCursor()
    {
        // UI 테스트용: F1 = 잠금, F2 = 풀기
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (CursorManager.Instance != null)
            {
                CursorManager.Instance.LockCursor();
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (CursorManager.Instance != null)
            {
                CursorManager.Instance.UnlockCursor();
            }
        }
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationY += mouseX * _rotateSpeed;
        _rotationX -= mouseY * _rotateSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -10f, 60f);
    }

    private Quaternion GetCameraRotation()
    {
        return Quaternion.Euler(_rotationX, _rotationY, 0);
    }

    private Vector3 GetTargetPosition(Quaternion rotation)
    {
        return Transform_Goat.position + rotation * _offset;
    }

    private Vector3 AvoidWall(Vector3 targetPosition)
    {
        Vector3 pivot = Transform_Goat.position;
        Vector3 direction = targetPosition - pivot;

        float distance = direction.magnitude;

        if (Physics.Raycast(pivot, direction.normalized, out RaycastHit hit, distance, _collisionLayer))
        {
            return hit.point - direction.normalized * _collisionPadding;
        }
        return targetPosition;
    }
}