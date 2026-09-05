using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyDetector detector;
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private EnemyPatrol patrol;
    [SerializeField] private Attack attack;

    private void FixedUpdate()
    {
        if (detector.TryGetPlayer(out Transform player))
        {
            if (attack.IsTargetInRange())
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
        movement.Stop();

        attack.TryAttack();
    }

    private void Chase(Transform player)
    {
        movement.MoveTo(player.position.x);
    }

    private void Patrol()
    {
        float targetX = patrol.GetTargetPositionX();

        movement.MoveTo(targetX);
    }
}