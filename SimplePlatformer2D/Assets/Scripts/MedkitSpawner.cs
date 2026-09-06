using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MedkitSpawner : MonoBehaviour
{
    [FormerlySerializedAs("medkitPrefab")]
    [SerializeField] private Medkit _medkitPrefab;
    [FormerlySerializedAs("spawnPoints")]
    [SerializeField] private Transform[] _spawnPoints;
    [FormerlySerializedAs("spawnCount")]
    [SerializeField, Min(0)] private int _spawnCount = 3;

    private void Start()
    {
        if (_medkitPrefab == null || _spawnPoints == null)
        {
            Debug.LogError("Assign a medkit prefab and spawn points.", this);
            return;
        }

        List<Transform> availablePoints = new List<Transform>();

        foreach (Transform point in _spawnPoints)
        {
            if (point != null && availablePoints.Contains(point) == false)
                availablePoints.Add(point);
        }

        int count = Mathf.Min(_spawnCount, availablePoints.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, availablePoints.Count);
            Instantiate(_medkitPrefab, availablePoints[index].position, Quaternion.identity, transform);
            availablePoints.RemoveAt(index);
        }
    }
}
