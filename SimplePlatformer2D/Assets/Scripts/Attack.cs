using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Attack : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private readonly List<Collider2D> targets = new List<Collider2D>();
    private Health health;
    private float nextAttackTime;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public bool IsTargetInRange()
    {
        return FindTargetHealth() != null;
    }

    public bool TryAttack()
    {
        if (isActiveAndEnabled == false || health.IsAlive == false || Time.time < nextAttackTime)
            return false;

        Health targetHealth = FindTargetHealth();

        if (targetHealth == null || targetHealth.IsAlive == false)
            return false;

        nextAttackTime = Time.time + attackCooldown;
        targetHealth.TakeDamage(damage);

        return true;
    }

    private Health FindTargetHealth()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(targetLayer);
        filter.useTriggers = false;
        Physics2D.OverlapCircle(transform.position, attackRadius, filter, targets);

        Health nearest = null;
        float nearestDistance = float.PositiveInfinity;

        foreach (Collider2D targetCollider in targets)
        {
            Health candidate = targetCollider.GetComponentInParent<Health>();

            if (candidate == null || candidate == health || candidate.IsAlive == false)
                continue;

            if (Physics2D.Linecast(transform.position, targetCollider.bounds.center, obstacleLayer))
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
        damage = Mathf.Max(0, damage);
        attackRadius = Mathf.Max(0.01f, attackRadius);
        attackCooldown = Mathf.Max(0.01f, attackCooldown);
    }
}
