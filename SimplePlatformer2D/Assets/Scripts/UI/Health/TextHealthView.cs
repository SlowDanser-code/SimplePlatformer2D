using TMPro;
using UnityEngine;

public class TextHealthView : HealthView
{
    [SerializeField] private TMP_Text healthText;

    protected override void UpdateView()
    {
        int currentValue = Mathf.CeilToInt(Health.CurrentValue);
        int maxValue = Mathf.CeilToInt(Health.MaxValue);

        healthText.text = $"{currentValue}/{maxValue}";
    }
}