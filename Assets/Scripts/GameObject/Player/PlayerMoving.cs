using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private Animator Animator_Goat;
    [SerializeField] private GameObject Goat_Humanoid;
    [SerializeField] private PlayerAttack PlayerAttack;
    [SerializeField] private Rigidbody Rigidbody_BasicGoat;
    [SerializeField] private Transform Transform_HomePoint;

    public int Stamina { get; private set; }

    private float _inputZ;
    private float _inputX;

    private float _walkSpeed = 3f;
    private float _runSpeed = 5f;
    private bool _isAttack = false;
    private bool _isAlive = true;

    private CancellationTokenSource _attackToken;
    private CancellationTokenSource _dieToken;
    private Camera _camera;

    public bool IsAttacking => _isAttack;

    public event Action<bool> OnAttackStateChanged;

    private void Start()
    {
        SaveManager.Instance.OnSetStamina += SetStamina;
        _camera = Camera.main;

        GameManager.Instance.OnUseStaminaItem += AddGoatStamina;
        GameManager.Instance.OnUseSpeedItem += AddGoatSpeed;
        GameManager.Instance.OnMoveHome += MoveHome;
    }

    private void Update()
    {
        _inputZ = InputManager.Vertical;
        _inputX = InputManager.Horizontal;

        if (!MiniGameManager.Instance.isGame && !DayMinigame.Instance.IsPlaying && !StoreManager.Instance.IsActiveStore() && Input.GetKeyDown(KeyCode.Space))
        {
            if (Stamina == 0)
            {
                SaveManager.Instance.OnSetStamina?.Invoke();
            }

            if (Stamina >= 10 && !_isAttack)
            {
                AttackRoutine().Forget();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isAttack)
        {
            return;
        }

        RotateDirection();
        MoveToward(_inputZ, _inputX);
    }

    private void InitializeGoat()
    {
        Stamina = 100;
        GameManager.Instance.OnChangedStamina?.Invoke(Stamina);
        this.transform.position = new Vector3(0, 5f, 0);

        _walkSpeed = 3f;
        _runSpeed = 5f;
        _isAttack = false;
        _isAlive = true;
    }

    private void MoveToward(float inputZ, float inputX)
    {
        if (Goat_Humanoid.activeSelf || !_isAlive)
        {
            return;
        }

        if (_inputZ == 0 && _inputX == 0)
        {
            Animator_Goat.SetBool("Walk", false);
            Animator_Goat.SetBool("Run", false);

            return;
        }

        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;
        forward.y = 0f;
        right.y = 0f;

        Vector3 moveDirection = (forward * inputZ + right * inputX).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float moveSpeed = isRunning ? _runSpeed : _walkSpeed;

        Vector3 moveVelocity = moveDirection * moveSpeed;
        Rigidbody_BasicGoat.MovePosition(Rigidbody_BasicGoat.position + moveVelocity * Time.fixedDeltaTime);

        Animator_Goat.SetBool("Walk", !isRunning);
        Animator_Goat.SetBool("Run", isRunning);
    }

    private void RotateDirection()
    {
        if (!_isAlive)
        {
            return;
        }

        float targetRotation = _camera.transform.eulerAngles.y;
        Quaternion nextRotation = Quaternion.Euler(0, targetRotation, 0);

        Rigidbody_BasicGoat.MoveRotation(nextRotation);
    }

    private void MoveHome()
    {
        MoveHomeAsync().Forget();
    }

    private async UniTaskVoid MoveHomeAsync()
    {
        Vector3 targetPosition = Transform_HomePoint.position;

        Rigidbody_BasicGoat.isKinematic = true;

        Rigidbody_BasicGoat.linearVelocity = Vector3.zero;
        Rigidbody_BasicGoat.angularVelocity = Vector3.zero;

        Rigidbody_BasicGoat.transform.position = targetPosition;
        this.transform.position = targetPosition;
        Rigidbody_BasicGoat.position = targetPosition;

        _inputX = 0f;
        _inputZ = 0f;

        if (Goat_Humanoid != null)
        {
            Goat_Humanoid.transform.localPosition = Vector3.zero;
        }

        if (_camera != null)
        {
            var camController = _camera.GetComponent<CameraController>();
            if (camController != null)
            {
                float targetAngleY = Transform_HomePoint.eulerAngles.y;
                camController.SetCameraRotation(targetAngleY);
            }
        }

        await UniTask.WaitForFixedUpdate();

        if (Rigidbody_BasicGoat != null)
        {
            Rigidbody_BasicGoat.isKinematic = false;

            Rigidbody_BasicGoat.linearVelocity = Vector3.zero;
            Rigidbody_BasicGoat.angularVelocity = Vector3.zero;
            Rigidbody_BasicGoat.position = targetPosition;
        }
    }

    private async UniTask AttackRoutine()
    {
        CancelAttack();
        _attackToken = new CancellationTokenSource();

        _isAttack = true;
        OnAttackStateChanged?.Invoke(true);

        Stamina -= 10;

        if (SaveManager.Instance != null && SaveManager.Instance.CurrentPlayerModel != null)
        {
            SaveManager.Instance.CurrentPlayerModel.Stamina = Stamina;
        }

        GameManager.Instance.OnChangedStamina?.Invoke(Stamina);

        Animator_Goat.SetTrigger("Attack");

        await UniTask.Delay(TimeSpan.FromSeconds(2.5f), cancellationToken: _attackToken.Token);

        PlayerAttack.Attack();

        await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: _attackToken.Token);

        _isAttack = false;
        OnAttackStateChanged?.Invoke(false);

        if (Stamina <= 0)
        {
            Die().Forget();
        }
    }

    private async UniTask Die()
    {
        CancelDie();
        _dieToken = new CancellationTokenSource();

        _isAlive = false;

        Animator_Goat.SetTrigger("Die");

        await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: _dieToken.Token);

        GameManager.Instance.DetermineEnding();
        InitializeGoat();
    }

    private void CancelAttack()
    {
        if (_attackToken != null)
        {
            _attackToken.Cancel();
            _attackToken?.Dispose();
            _attackToken = null;
        }
    }

    private void CancelDie()
    {
        if (_dieToken != null)
        {
            _dieToken.Cancel();
            _dieToken?.Dispose();
            _dieToken = null;
        }
    }

    public bool IsAlive()
    {
        return _isAlive;
    }

    private void AddGoatStamina(int value)
    {
        Stamina += value;

        if (Stamina > 100)
        {
            Stamina = 100;
        }

        GameManager.Instance.OnChangedStamina?.Invoke(Stamina);
        SaveManager.Instance.CurrentPlayerModel.Stamina = Stamina;
    }

    private void AddGoatSpeed(float value)
    {
        _runSpeed = _runSpeed * value;
        _walkSpeed = _walkSpeed * value;
    }

    private void SetStamina()
    {
        Stamina = SaveManager.Instance.CurrentPlayerModel.Stamina;
    }
}