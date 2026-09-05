using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField, Min(0.01f)] private float groundProbeDistance = 0.2f;

    private Rigidbody2D body;
    private Collider2D bodyCollider;
    private float previousDirection;

    public event Action<float> DirectionChanged;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
    }

    public void MoveTo(float targetX)
    {
        float distance = targetX - transform.position.x;

        if (Mathf.Abs(distance) < 0.01f)
        {
            Stop();
            return;
        }

        float direction = Mathf.Sign(distance);
        float velocity = direction * Mathf.Min(speed, Mathf.Abs(distance) / Time.fixedDeltaTime);

        if (groundLayer.value != 0)
        {
            Bounds bounds = bodyCollider.bounds;
            Vector2 probe = new Vector2(bounds.center.x + direction * (bounds.extents.x + groundProbeDistance), bounds.min.y + 0.05f);

            if (Physics2D.Raycast(probe, Vector2.down, groundProbeDistance + 0.05f, groundLayer).collider == null)
            {
                Stop();
                return;
            }
        }

        body.velocity =
            new Vector2(velocity, body.velocity.y);

        if (direction != previousDirection)
        {
            previousDirection = direction;

            DirectionChanged?.Invoke(direction);
        }
    }

    public void Stop()
    {
        body.velocity =
            new Vector2(0, body.velocity.y);
    }
}
