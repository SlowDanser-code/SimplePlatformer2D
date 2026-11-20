using UnityEngine;

public class VampirismDamager : MonoBehaviour
{
    private const int MaxTargets = 10;

    [SerializeField] private Health playerHealth;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float radius = 3f;
    [SerializeField] private float damagePerSecond = 10f;

    private readonly Collider2D[] results = new Collider2D[MaxTargets];

    private ContactFilter2D contactFilter;

    public float Radius => radius;

    private void Awake()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(enemyLayer);
        contactFilter.useLayerMask = true;
    }

    public void Tick(float deltaTime)
    {
        if (playerHealth.IsAlive == false)
            return;

        Health target = FindNearestTarget();

        if (target == null)
            return;

        float damage = damagePerSecond * deltaTime;

        float healthBeforeDamage = target.CurrentValue;

        target.TakeDamage(damage);

        float actualDamage =
            healthBeforeDamage - target.CurrentValue;

        if (actualDamage > 0)
        {
            playerHealth.TryHeal(actualDamage);
        }
    }

    private Health FindNearestTarget()
    {
        int targetsCount = Physics2D.OverlapCircle(
            transform.position,
            radius,
            contactFilter,
            results);

        Health nearestTarget = null;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < targetsCount; i++)
        {
            Collider2D targetCollider = results[i];

            Health health =
                targetCollider.GetComponentInParent<Health>();

            if (health == null || health.IsAlive == false)
                continue;

            float distance = Vector2.SqrMagnitude(
                health.transform.position - transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = health;
            }
        }

        return nearestTarget;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}