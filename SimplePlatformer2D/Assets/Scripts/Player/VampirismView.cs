using UnityEngine;
using UnityEngine.UI;

public class VampirismView : MonoBehaviour
{
    [SerializeField] private GameObject radiusView;
    [SerializeField] private Slider timerSlider;

    private bool isCooldown;

    public void StartAbility()
    {
        isCooldown = false;

        radiusView.SetActive(true);
        timerSlider.gameObject.SetActive(true);

        timerSlider.value = 1f;
    }

    public void StartCooldown()
    {
        isCooldown = true;

        radiusView.SetActive(false);

        timerSlider.value = 0f;
    }

    public void Tick(float elapsedTime, float duration)
    {
        float progress = elapsedTime / duration;

        if (isCooldown)
        {
            timerSlider.value = progress;
        }
        else
        {
            timerSlider.value = 1f - progress;
        }
    }

    public void Stop()
    {
        radiusView.SetActive(false);

        timerSlider.value = 1f;
        timerSlider.gameObject.SetActive(false);
    }
}