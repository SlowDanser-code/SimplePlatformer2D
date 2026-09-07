using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints = new List<Transform>();
    [FormerlySerializedAs("reachDistance")]
    [SerializeField, Min(0)] private float _reachDistance = 0.1f;

    private readonly List<float> _waypointPositionsX = new List<float>();
    private int _targetIndex;

    private void Awake()
    {
        foreach (Transform waypoint in _waypoints)
        {
            if (waypoint != null)
                _waypointPositionsX.Add(waypoint.position.x);
        }
    }

    public float GetTargetPositionX()
    {
        if (_waypointPositionsX.Count == 0)
            return transform.position.x;

        if (Mathf.Abs(transform.position.x - _waypointPositionsX[_targetIndex]) <= _reachDistance)
            _targetIndex = (_targetIndex + 1) % _waypointPositionsX.Count;

        return _waypointPositionsX[_targetIndex];
    }
}
