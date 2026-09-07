using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Health), typeof(TargetScanner))]
public class Attack : MonoBehaviour
{
    [FormerlySerializedAs("damage")]
    [SerializeField] private float _damage = 20f;
    [FormerlySerializedAs("attackRadius")]
    [SerializeField] private float _attackRadius = 1f;
    [FormerlySerializedAs("attackCooldown")]
    [SerializeField] private float _attackCooldown = 1f;
    [FormerlySerializedAs("targetLayer")]
    [SerializeField] private LayerMask _targetLayer;
    [FormerlySerializedAs("obstacleLayer")]
    [SerializeField] private LayerMask _obstacleLayer;

    private TargetScanner _targetScanner;
    private Health _health;
    private float _nextAttackTime;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _targetScanner = GetComponent<TargetScanner>();
    }

    public bool IsTargetInRange()
    {
        return FindTargetHealth() != null;
    }

    public bool TryAttack()
    {
        if (isActiveAndEnabled == false || _health.IsAlive == false || Time.time < _nextAttackTime)
            return false;

        Health targetHealth = FindTargetHealth();

        if (targetHealth == null || targetHealth.IsAlive == false)
            return false;

        _nextAttackTime = Time.time + _attackCooldown;
        targetHealth.TakeDamage(_damage);

        return true;
    }

    private Health FindTargetHealth()
    {
        Health nearest = null;
        float nearestDistance = float.PositiveInfinity;

        foreach (Collider2D targetCollider in _targetScanner.Scan(_attackRadius, _targetLayer))
        {
            Health candidate = targetCollider.GetComponentInParent<Health>();

            if (candidate == null || candidate == _health || candidate.IsAlive == false)
                continue;

            if (Physics2D.Linecast(transform.position, targetCollider.bounds.center, _obstacleLayer))
                continue;

            float distance = ((Vector2)transform.position - targetCollider.ClosestPoint(transform.position)).sqrMagnitude;

            if (distance < nearestDistance)
            {
                nearest = candidate;
                nearestDistance = distance;
            }
        }

        return nearest;
    }

    private void OnValidate()
    {
        _damage = Mathf.Max(0, _damage);
        _attackRadius = Mathf.Max(0.01f, _attackRadius);
        _attackCooldown = Mathf.Max(0.01f, _attackCooldown);
    }
}
