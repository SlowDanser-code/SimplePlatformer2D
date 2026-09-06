using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    [FormerlySerializedAs("maxValue")]
    [SerializeField, Min(1)] private float _maxValue = 100f;

    public event Action<float> Changed;
    public event Action Died;

    public float CurrentValue { get; private set; }
    public float MaxValue => _maxValue;
    public bool IsAlive => CurrentValue > 0;

    private void Awake()
    {
        _maxValue = Mathf.Max(1, _maxValue);
        CurrentValue = _maxValue;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0 || IsAlive == false)
            return;

        CurrentValue = Mathf.Max(CurrentValue - damage, 0);

        Debug.Log($"{name}: здоровье = {CurrentValue}");

        Changed?.Invoke(CurrentValue);

        if (IsAlive == false)
        {
            Died?.Invoke();
        }
    }

    public bool TryHeal(float value)
    {
        if (value <= 0 || IsAlive == false)
            return false;

        if (CurrentValue >= _maxValue)
            return false;

        CurrentValue = Mathf.Min(CurrentValue + value, _maxValue);

        Changed?.Invoke(CurrentValue);

        return true;
    }
}
