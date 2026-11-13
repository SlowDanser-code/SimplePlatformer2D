using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBarView : HealthView
{
    [SerializeField] private Slider slider;
    [SerializeField] private float changeSpeed = 1f;

    private float targetValue;

    protected override void Start()
    {
        base.Start();

        slider.value = targetValue;
    }

    private void Update()
    {
        slider.value = Mathf.MoveTowards(
            slider.value,
            targetValue,
            changeSpeed * Time.deltaTime);
    }

    protected override void UpdateView()
    {
        targetValue = GetHealthRatio();
    }
}