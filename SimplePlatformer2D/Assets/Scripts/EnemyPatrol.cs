using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float reachDistance = 0.1f;

    private Rigidbody2D rigidbody2D;
    private Transform target;
    private float previousDirection;

    public event Action<float> DirectionChanged;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        target = rightPoint;
    }

    private void FixedUpdate()
    {
        Move();

        if (IsTargetReached())
        {
            ChangeTarget();
        }
    }

    private void Move()
    {
        float direction =
            Mathf.Sign(target.position.x - transform.position.x);

        rigidbody2D.velocity =
            new Vector2(direction * speed, rigidbody2D.velocity.y);

        if (direction != previousDirection)
        {
            previousDirection = direction;

            DirectionChanged?.Invoke(direction);
        }
    }

    private bool IsTargetReached()
    {
        float distance =
            Mathf.Abs(target.position.x - transform.position.x);

        return distance <= reachDistance;
    }

    private void ChangeTarget()
    {
        if (target == rightPoint)
        {
            target = leftPoint;
        }
        else
        {
            target = rightPoint;
        }
    }
}