using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public event Action<Coin> Collected;

    public void Collect()
    {
        Debug.Log("Монета собрана");

        Collected?.Invoke(this);
    }
}