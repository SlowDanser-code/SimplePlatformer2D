using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAI : MonoBehaviour
{
    [FormerlySerializedAs("detector")]
    [SerializeField] private EnemyDetector _detector;
    [FormerlySerializedAs("movement")]
    [SerializeField] private EnemyMovement _movement;
    [FormerlySerializedAs("patrol")]
    [SerializeField] private EnemyPatrol _patrol;
    [FormerlySerializedAs("attack")]
    [SerializeField] private Attack _attack;

    private void FixedUpdate()
    {
        if (_detector.TryGetPlayer(out Transform player))
        {
            if (_attack.IsTargetInRange())
            {
                Attack();
            }
            else
            {
                Chase(player);
            }
        }
        else
        {
            Patrol();
        }
    }

    private void Attack()
    {
        _movement.Stop();

        _attack.TryAttack();
    }

    private void Chase(Transform player)
    {
        _movement.MoveTo(player.position.x);
    }

    private void Patrol()
    {
        float targetX = _patrol.GetTargetPositionX();

        _movement.MoveTo(targetX);
    }
}