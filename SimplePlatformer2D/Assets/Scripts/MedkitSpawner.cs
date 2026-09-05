using System.Collections.Generic;
using UnityEngine;

public class MedkitSpawner : MonoBehaviour
{
    [SerializeField] private Medkit medkitPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField, Min(0)] private int spawnCount = 3;

    private void Start()
    {
        if (medkitPrefab == null || spawnPoints == null)
        {
            Debug.LogError("Assign a medkit prefab and spawn points.", this);
            return;
        }

        List<Transform> availablePoints = new List<Transform>();

        foreach (Transform point in spawnPoints)
        {
            if (point != null && availablePoints.Contains(point) == false)
                availablePoints.Add(point);
        }

        int count = Mathf.Min(spawnCount, availablePoints.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, availablePoints.Count);
            Instantiate(medkitPrefab, availablePoints[index].position, Quaternion.identity, transform);
            availablePoints.RemoveAt(index);
        }
    }
}
