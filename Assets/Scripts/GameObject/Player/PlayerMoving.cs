using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    private float _inputZ;
    private float _inputX;

    private Rigidbody _rb;
    private Animator _anim;

    private float _walkSpeed = 3f;
    private float _runSpeed = 5f;

    private bool _isAttack = false;
    private CancellationTokenSource _attackToken;

    private int _stamina;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _stamina = GameManager.Instance.PlayerModel.Stamina;
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
        RotateDirection();

        if (_inputZ == 0 && _inputX == 0)
        {
            _rb.linearVelocity = Vector3.zero;
            _anim.SetBool("Walk", false);
            _anim.SetBool("Run", false);

            return;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float moveSpeed = isRunning ? _runSpeed : _walkSpeed;

        Vector3 moveDirection = new Vector3(_inputX, 0, inputZ).normalized;
        _rb.linearVelocity = moveDirection * moveSpeed;

        _anim.SetBool("Walk", !isRunning);
        _anim.SetBool("Run", isRunning);
    }

    private void RotateDirection()
    {
        
    }

    private async UniTask AttackRoutine()
    {
        CancelAttack();
        _attackToken = new CancellationTokenSource();

        _isAttack = true;

        _anim.SetTrigger("Attack");

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
