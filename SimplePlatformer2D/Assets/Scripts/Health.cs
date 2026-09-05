using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField, Min(1)] private float maxValue = 100f;

    private float currentValue;

    public event Action<float> Changed;
    public event Action Died;

    public float CurrentValue => currentValue;
    public float MaxValue => maxValue;
    public bool IsAlive => currentValue > 0;

    private void Awake()
    {
        maxValue = Mathf.Max(1, maxValue);
        currentValue = maxValue;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0 || IsAlive == false)
            return;

        currentValue = Mathf.Max(currentValue - damage, 0);

        Debug.Log($"{name}: здоровье = {currentValue}");

        Changed?.Invoke(currentValue);

        if (IsAlive == false)
        {
            Died?.Invoke();
        }
    }

    public bool TryHeal(float value)
    {
        if (value <= 0 || IsAlive == false)
            return false;

        if (currentValue >= maxValue)
            return false;

        currentValue = Mathf.Min(currentValue + value, maxValue);

        Changed?.Invoke(currentValue);

        return true;
    }
}
