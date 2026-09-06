using UnityEngine;
using UnityEngine.Serialization;

public class EnemyPatrol : MonoBehaviour
{
    [FormerlySerializedAs("leftPoint")]
    [SerializeField] private Transform _leftPoint;
    [FormerlySerializedAs("rightPoint")]
    [SerializeField] private Transform _rightPoint;
    [FormerlySerializedAs("reachDistance")]
    [SerializeField] private float _reachDistance = 0.1f;
    [FormerlySerializedAs("defaultPatrolDistance")]
    [SerializeField, Min(0)] private float _defaultPatrolDistance = 2f;

    private float _leftX;
    private float _rightX;
    private bool _movingRight = true;

    private void Awake()
    {
        // Store world positions so child waypoints do not move the patrol area.
        float firstX = _leftPoint != null ? _leftPoint.position.x : transform.position.x - _defaultPatrolDistance;
        float secondX = _rightPoint != null ? _rightPoint.position.x : transform.position.x + _defaultPatrolDistance;
        _leftX = Mathf.Min(firstX, secondX);
        _rightX = Mathf.Max(firstX, secondX);
    }

    public float GetTargetPositionX()
    {
        if (_movingRight && transform.position.x >= _rightX - _reachDistance)
            _movingRight = false;
        else if (_movingRight == false && transform.position.x <= _leftX + _reachDistance)
            _movingRight = true;

        return _movingRight ? _rightX : _leftX;
    }
}
