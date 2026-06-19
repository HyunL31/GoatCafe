using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private Animator Animator_Goat;
    [SerializeField] private GameObject Goat_Humanoid;

    private float _inputZ;
    private float _inputX;

    private float _walkSpeed = 3f;
    private float _runSpeed = 5f;

    private bool _isAttack = false;
    private CancellationTokenSource _attackToken;

    private Camera _camera;

    private int _stamina = 100;

    private void Start()
    {
        //_stamina = GameManager.Instance.PlayerModel.Stamina;
        _camera = Camera.main;
    }

    private void Update()
    {
        _inputZ = Input.GetAxisRaw("Vertical");
        _inputX = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && _stamina >= 10)
        {
            _stamina -= 10;
            AttackRoutine().Forget();
        }
    }

    private void FixedUpdate()
    {
        if (_isAttack)
        {
            return;
        }

        MoveToward(_inputZ, _inputX);
    }

    private void MoveToward(float inputZ, float inputX)
    {
        if (Goat_Humanoid.activeSelf)
        {
            return;
        }

        RotateDirection();

        if (_inputZ == 0 && _inputX == 0)
        {
            Animator_Goat.SetBool("Walk", false);
            Animator_Goat.SetBool("Run", false);

            return;
        }

        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;

        Vector3 moveDirection = (forward * inputZ + right * inputX).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float moveSpeed = isRunning ? _runSpeed : _walkSpeed;

        Vector3 moveVelocity = moveDirection * moveSpeed;
        transform.position += new Vector3(moveVelocity.x, 0, moveVelocity.z) * Time.fixedDeltaTime;

        Animator_Goat.SetBool("Walk", !isRunning);
        Animator_Goat.SetBool("Run", isRunning);
    }

    private void RotateDirection()
    {
        float targetRotation = _camera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, targetRotation, 0);
    }

    private async UniTask AttackRoutine()
    {
        CancelAttack();
        _attackToken = new CancellationTokenSource();

        _isAttack = true;

        Animator_Goat.SetTrigger("Attack");

        await UniTask.Delay(TimeSpan.FromSeconds(2.7f), cancellationToken: _attackToken.Token);

        _isAttack = false;
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

    private void AddGoatStamina(int value)
    {
        _stamina += value;
    }

    private void AddGoatSpeed(int value, bool isRun)
    {
        if (isRun)
        {
            _runSpeed += value;
        }
        else
        {
            _walkSpeed += value;
        }
    }
}
