using System;
using UnityEngine;

public interface IInteractable // 상호작용 가능한 오브젝트에 붙여서 구분하기 위한 인터페이스
{
    void Interact();
}

public class InputManager : BaseMonoManager<InputManager>
{
    [Header("플레이어 설정")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] public float _mouseSensitivity;
    [SerializeField] public Transform _cameraTransform;

    [Header("점프 및 물리")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private bool _isGrounded;

    [Header("컴포넌트")]
    [SerializeField] private Rigidbody Rigidbody_Goat;
    //[SerializeField] private AnimationController AnimController;
    //[SerializeField] private Goat_GroundDetector GroundDetector;

    [Header("일시정지")]
    //[SerializeField] private GameObject pauseMenuUI;

    public static float Horizontal { get; private set; }
    public static float Vertical { get; private set; }
    public static bool IsPaused { get; private set; } = false;

    

    public event Action OnInteractPressed;
    public event Action OnEscKeyDown;
    public event Action<KeyCode> OnItemUseBtnPressed;

    private float _rotationX = 0f;
    private bool _isMouseLock;

    //void Start()
    //{
    //    ToggleMouseLock(true);
    //}

    private void OnEnable()
    {
        //GroundDetector.GroundTriggeredEvent += OnGroundTriggered;
    }

    private void OnDisable()
    {
        //GroundDetector.GroundTriggeredEvent -= OnGroundTriggered;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    TogglePause();
        //}

        //if (IsPaused)
        //{
        //    ClearInputs();
        //    return;
        //}



        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        RotateCheckOnUpdate();
        MoveOnUpdate();

        //if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        //{
        //    StartJump();
        //}

        //if (Input.GetKeyDown(KeyCode.LeftAlt) && _isGrounded)
        //{
        //    ToggleMouseLock(false);
        //}

        if (Input.GetKeyDown(KeyCode.LeftAlt) && _isGrounded)
        {
            ToggleMouseLock(true);
        }

        if (Input.GetKeyDown(KeyCode.E)) // 상호작용 키세팅
        {
            OnInteractPressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.CurrentState == GameState.Playing)
            {
                UIManager.Instance.OpenInGamePopup(OnEscKeyDown);
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnItemUseBtnPressed?.Invoke(KeyCode.Alpha4);
        }

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            OnItemUseBtnPressed?.Invoke(KeyCode.Alpha5);
        }

    }

    void RotateCheckOnUpdate()
    {
        if (_isMouseLock == false)
        {
            return;
        }
        
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);
        _cameraTransform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);

        this.transform.Rotate(Vector3.up * mouseX);
    }

    public void ToggleMouseLock(bool isLock)
    {
        Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
        _isMouseLock = isLock;
    }

    void MoveOnUpdate()
    {
        //AnimController.SetState((Horizontal <= 0 && Vertical <= 0) ? EntityState.Idle : EntityState.Run);

        Vector3 move = (this.transform.right * Horizontal) + (this.transform.forward * Vertical);

        this.transform.position += ((move * _moveSpeed) * Time.deltaTime);
    }

    void StartJump()
    {
        Rigidbody_Goat.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _isGrounded = false;
    }

    private void OnGroundTriggered(bool isGrounded)
    {
        _isGrounded = isGrounded;
    }

    void TogglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            //pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            ToggleMouseLock(false);
            Cursor.visible = true;
        }
        else
        {
            //pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            ToggleMouseLock(true); 
            Cursor.visible = false;
        }
    }

    void ClearInputs()
    {
        Horizontal = 0f;
        Vertical = 0f;
    }
}