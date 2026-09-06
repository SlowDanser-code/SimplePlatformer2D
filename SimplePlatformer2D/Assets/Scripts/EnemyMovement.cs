using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    [FormerlySerializedAs("speed")]
    [SerializeField] private float _speed = 2f;
    [FormerlySerializedAs("groundLayer")]
    [SerializeField] private LayerMask _groundLayer;
    [FormerlySerializedAs("groundProbeDistance")]
    [SerializeField, Min(0.01f)] private float _groundProbeDistance = 0.2f;

    private Rigidbody2D _body;
    private Collider2D _bodyCollider;
    private float _previousDirection;

    public event Action<float> DirectionChanged;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _bodyCollider = GetComponent<Collider2D>();
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
        float velocity = direction * Mathf.Min(_speed, Mathf.Abs(distance) / Time.fixedDeltaTime);

        if (_groundLayer.value != 0)
        {
            Bounds bounds = _bodyCollider.bounds;
            Vector2 probe = new Vector2(bounds.center.x + direction * (bounds.extents.x + _groundProbeDistance), bounds.min.y + 0.05f);

            if (Physics2D.Raycast(probe, Vector2.down, _groundProbeDistance + 0.05f, _groundLayer).collider == null)
            {
                Stop();
                return;
            }
        }

        _body.velocity =
            new Vector2(velocity, _body.velocity.y);

        if (direction != _previousDirection)
        {
            _previousDirection = direction;

            DirectionChanged?.Invoke(direction);
        }
    }

    public void Stop()
    {
        _body.velocity =
            new Vector2(0, _body.velocity.y);
    }
}
