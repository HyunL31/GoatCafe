using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    private float _input;
    private Rigidbody _rb;
    private Animator _anim;

    private float _walkSpeed = 3f;
    private float _runSpeed = 5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _input = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        MoveToward(_input);
    }

    private void MoveToward(float input)
    {
        if (input == 0)
        {
            _rb.linearVelocity = Vector3.zero;
            _anim.SetBool("Walk", false);
            _anim.SetBool("Run", false);

            return;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float moveSpeed = isRunning ? _runSpeed : _walkSpeed;

        _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, input * moveSpeed);

        _anim.SetBool("Walk", !isRunning);
        _anim.SetBool("Run", isRunning);
    }
}
