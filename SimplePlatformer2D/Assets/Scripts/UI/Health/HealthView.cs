using UnityEngine;

public abstract class HealthView : MonoBehaviour
{
    [SerializeField] private Health health;

    protected Health Health => health;

    protected virtual void OnEnable()
    {
        health.Changed += OnHealthChanged;

        UpdateView();
    }

    protected virtual void OnDisable()
    {
        health.Changed -= OnHealthChanged;
    }

    private void OnHealthChanged(float currentValue)
    {
        UpdateView();
    }

    protected float GetHealthRatio()
    {
        return health.CurrentValue / health.MaxValue;
    }

    protected abstract void UpdateView();
}