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
    [SerializeField, Min(0.1f)] private float _spawnInterval = 5f;

    private readonly List<Transform> _points = new List<Transform>();
    private readonly List<Medkit> _medkits = new List<Medkit>();
    private readonly List<int> _availableIndices = new List<int>();
    private float _timeUntilSpawn;

    private void Start()
    {
        if (_medkitPrefab == null || _spawnPoints == null)
        {
            Debug.LogError("Assign a medkit prefab and spawn points.", this);
            return;
        }

        foreach (Transform point in _spawnPoints)
        {
            if (point != null && _points.Contains(point) == false)
            {
                _points.Add(point);
                _medkits.Add(null);
            }
        }

        int count = Mathf.Min(_spawnCount, _points.Count);

        for (int i = 0; i < count; i++)
            SpawnMedkit();

        _timeUntilSpawn = Mathf.Max(0.1f, _spawnInterval);
    }

    private void Update()
    {
        if (_medkitPrefab == null || _points.Count == 0)
            return;

        _timeUntilSpawn -= Time.deltaTime;

        if (_timeUntilSpawn > 0f)
            return;

        SpawnMedkit();
        _timeUntilSpawn = Mathf.Max(0.1f, _spawnInterval);
    }

    private void SpawnMedkit()
    {
        _availableIndices.Clear();

        for (int i = 0; i < _points.Count; i++)
        {
            if (_points[i] != null && (_medkits[i] == null || !_medkits[i].gameObject.activeSelf))
                _availableIndices.Add(i);
        }

        if (_availableIndices.Count == 0)
            return;

        int index = _availableIndices[Random.Range(0, _availableIndices.Count)];

        if (_medkits[index] == null)
            _medkits[index] = Instantiate(_medkitPrefab, _points[index].position, Quaternion.identity, transform);
        else
        {
            _medkits[index].transform.position = _points[index].position;
            _medkits[index].gameObject.SetActive(true);
        }
    }
}
