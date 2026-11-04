using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : HealthView
{
    [SerializeField] private Slider slider;

    protected override void UpdateView()
    {
        slider.value = GetHealthRatio();
    }
}