using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CoinSpawner : MonoBehaviour
{
    [FormerlySerializedAs("coinPool")]
    [SerializeField] private CoinPool _coinPool;
    [FormerlySerializedAs("spawnPoints")]
    [SerializeField] private Transform[] _spawnPoints;
    [FormerlySerializedAs("spawnDelay")]
    [SerializeField] private float _spawnDelay = 2f;

    private readonly List<Transform> _freeSpawnPoints = new List<Transform>();
    private readonly Dictionary<Coin, Transform> _occupiedSpawnPoints =
        new Dictionary<Coin, Transform>();

    private void Start()
    {
        _freeSpawnPoints.AddRange(_spawnPoints);

        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        while (enabled)
        {
            Spawn();

            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    private void Spawn()
    {
        if (_freeSpawnPoints.Count == 0)
            return;

        int randomIndex = Random.Range(0, _freeSpawnPoints.Count);

        Transform spawnPoint = _freeSpawnPoints[randomIndex];

        Coin coin = _coinPool.Get();

        coin.transform.position = spawnPoint.position;

        coin.Collected += OnCoinCollected;

        _freeSpawnPoints.Remove(spawnPoint);
        _occupiedSpawnPoints.Add(coin, spawnPoint);
    }

    private void OnCoinCollected(Coin coin)
    {
        coin.Collected -= OnCoinCollected;

        Transform spawnPoint = _occupiedSpawnPoints[coin];

        _occupiedSpawnPoints.Remove(coin);
        _freeSpawnPoints.Add(spawnPoint);

        _coinPool.Return(coin);
    }
}