using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D), typeof(EntityRotator))]
public class PlayerMovement : MonoBehaviour
{
    [FormerlySerializedAs("inputReader")]
    [SerializeField] private PlayerInputReader _inputReader;
    [FormerlySerializedAs("speed")]
    [SerializeField] private float _speed = 5f;
    [FormerlySerializedAs("jumpForce")]
    [SerializeField] private float _jumpForce = 7f;

    [FormerlySerializedAs("groundCheck")]
    [SerializeField] private Transform _groundCheck;
    [FormerlySerializedAs("groundCheckRadius")]
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [FormerlySerializedAs("groundLayer")]
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D _body;
    private EntityRotator _rotator;

    private float _direction;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _rotator = GetComponent<EntityRotator>();
    }

    private void OnEnable()
    {
        _inputReader.MovementInputChanged += OnMovementInputChanged;
        _inputReader.JumpPressed += Jump;
    }

    private void OnDisable()
    {
        _inputReader.MovementInputChanged -= OnMovementInputChanged;
        _inputReader.JumpPressed -= Jump;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnMovementInputChanged(float newDirection)
    {
        _direction = newDirection;

        _rotator.Rotate(_direction);
    }

    private void Move()
    {
        _body.velocity =
            new Vector2(_direction * _speed, _body.velocity.y);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            _body.velocity =
                new Vector2(_body.velocity.x, _jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            _groundCheck.position,
            _groundCheckRadius,
            _groundLayer);
    }

    public void Stop()
    {
        _direction = 0;

        _body.velocity =
            new Vector2(0, _body.velocity.y);
    }
}
