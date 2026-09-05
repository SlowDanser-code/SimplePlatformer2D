using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private readonly List<Collider2D> candidates = new List<Collider2D>();

    public bool TryGetPlayer(out Transform player)
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(playerLayer);
        filter.useTriggers = false;
        Physics2D.OverlapCircle(transform.position, detectionRadius, filter, candidates);

        foreach (Collider2D playerCollider in candidates)
        {
            Health health = playerCollider.GetComponentInParent<Health>();

            if (health == null || health.IsAlive == false)
                continue;

            if (Physics2D.Linecast(transform.position, playerCollider.bounds.center, obstacleLayer))
                continue;

            player = health.transform;
            return true;
        }

        player = null;

        return false;
    }
}
