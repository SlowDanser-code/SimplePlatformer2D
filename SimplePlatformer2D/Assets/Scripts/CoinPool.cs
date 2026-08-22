using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private int initialSize = 5;

    private readonly List<Coin> coins = new List<Coin>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateCoin();
        }
    }

    public Coin Get()
    {
        foreach (Coin coin in coins)
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
        Coin coin = Instantiate(coinPrefab, transform);

        coin.gameObject.SetActive(false);

        coins.Add(coin);

        return coin;
    }
}