using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(TargetScanner))]
public class EnemyDetector : MonoBehaviour
{
    [FormerlySerializedAs("detectionRadius")]
    [SerializeField] private float _detectionRadius = 5f;
    [FormerlySerializedAs("playerLayer")]
    [SerializeField] private LayerMask _playerLayer;
    [FormerlySerializedAs("obstacleLayer")]
    [SerializeField] private LayerMask _obstacleLayer;

    private TargetScanner _targetScanner;

    private void Awake()
    {
        _targetScanner = GetComponent<TargetScanner>();
    }

    public bool TryGetPlayer(out Transform player)
    {
        foreach (Collider2D playerCollider in _targetScanner.Scan(_detectionRadius, _playerLayer))
        {
            Health health = playerCollider.GetComponentInParent<Health>();

            if (health == null || health.IsAlive == false)
                continue;

            if (Physics2D.Linecast(transform.position, playerCollider.bounds.center, _obstacleLayer))
                continue;

            player = health.transform;
            return true;
        }

        player = null;

        return false;
    }
}
