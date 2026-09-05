using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float reachDistance = 0.1f;
    [SerializeField, Min(0)] private float defaultPatrolDistance = 2f;

    private float leftX;
    private float rightX;
    private bool movingRight = true;

    private void Awake()
    {
        // Store world positions so child waypoints do not move the patrol area.
        float firstX = leftPoint != null ? leftPoint.position.x : transform.position.x - defaultPatrolDistance;
        float secondX = rightPoint != null ? rightPoint.position.x : transform.position.x + defaultPatrolDistance;
        leftX = Mathf.Min(firstX, secondX);
        rightX = Mathf.Max(firstX, secondX);
    }

    public float GetTargetPositionX()
    {
        if (movingRight && transform.position.x >= rightX - reachDistance)
            movingRight = false;
        else if (movingRight == false && transform.position.x <= leftX + reachDistance)
            movingRight = true;

        return movingRight ? rightX : leftX;
    }
}
