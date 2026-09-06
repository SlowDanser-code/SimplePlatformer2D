using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CoinPool : MonoBehaviour
{
    [FormerlySerializedAs("coinPrefab")]
    [SerializeField] private Coin _coinPrefab;
    [FormerlySerializedAs("initialSize")]
    [SerializeField] private int _initialSize = 5;

    private readonly List<Coin> _coins = new List<Coin>();

    private void Awake()
    {
        for (int i = 0; i < _initialSize; i++)
        {
            CreateCoin();
        }
    }

    public Coin Get()
    {
        foreach (Coin coin in _coins)
        {
            if (coin.gameObject.activeSelf == false)
            {
                coin.gameObject.SetActive(true);

                return coin;
            }
        }

        Coin newCoin = CreateCoin();
        newCoin.gameObject.SetActive(true);

        return newCoin;
    }

    public void Return(Coin coin)
    {
        coin.gameObject.SetActive(false);
    }

    private Coin CreateCoin()
    {
        Coin coin = Instantiate(_coinPrefab, transform);

        coin.gameObject.SetActive(false);

        _coins.Add(coin);

        return coin;
    }
}