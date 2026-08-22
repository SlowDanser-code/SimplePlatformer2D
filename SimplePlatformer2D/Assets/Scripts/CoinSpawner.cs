using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private CoinPool coinPool;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnDelay = 2f;

    private readonly List<Transform> freeSpawnPoints = new List<Transform>();
    private readonly Dictionary<Coin, Transform> occupiedSpawnPoints =
        new Dictionary<Coin, Transform>();

    private void Start()
    {
        freeSpawnPoints.AddRange(spawnPoints);

        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        while (enabled)
        {
            Spawn();

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Spawn()
    {
        if (freeSpawnPoints.Count == 0)
            return;

        int randomIndex = Random.Range(0, freeSpawnPoints.Count);

        Transform spawnPoint = freeSpawnPoints[randomIndex];

        Coin coin = coinPool.Get();

        coin.transform.position = spawnPoint.position;

        coin.Collected += OnCoinCollected;

        freeSpawnPoints.Remove(spawnPoint);
        occupiedSpawnPoints.Add(coin, spawnPoint);
    }

    private void OnCoinCollected(Coin coin)
    {
        coin.Collected -= OnCoinCollected;

        Transform spawnPoint = occupiedSpawnPoints[coin];

        occupiedSpawnPoints.Remove(coin);
        freeSpawnPoints.Add(spawnPoint);

        coinPool.Return(coin);
    }
}