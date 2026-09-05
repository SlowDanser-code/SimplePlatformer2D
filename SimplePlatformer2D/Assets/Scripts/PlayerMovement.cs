using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;

    private float direction;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        inputReader.MovementInputChanged += OnMovementInputChanged;
        inputReader.JumpPressed += Jump;
    }

    private void OnDisable()
    {
        inputReader.MovementInputChanged -= OnMovementInputChanged;
        inputReader.JumpPressed -= Jump;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnMovementInputChanged(float newDirection)
    {
        direction = newDirection;

        Flip();
    }

    private void Move()
    {
        body.velocity =
            new Vector2(direction * speed, body.velocity.y);
    }

    private void Flip()
    {
        if (direction > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            body.velocity =
                new Vector2(body.velocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer);
    }

    public void Stop()
    {
        direction = 0;

        body.velocity =
            new Vector2(0, body.velocity.y);
    }
}