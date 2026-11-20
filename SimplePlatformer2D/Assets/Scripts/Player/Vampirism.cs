using System.Collections;
using UnityEngine;

public class Vampirism : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private VampirismDamager damager;
    [SerializeField] private VampirismView view;

    [SerializeField] private float abilityDuration = 6f;
    [SerializeField] private float cooldownDuration = 4f;

    private Coroutine abilityCoroutine;

    private void OnEnable()
    {
        inputReader.VampirismPressed += OnVampirismPressed;
    }

    private void OnDisable()
    {
        inputReader.VampirismPressed -= OnVampirismPressed;

        if (abilityCoroutine != null)
        {
            StopCoroutine(abilityCoroutine);
            abilityCoroutine = null;
        }

        view.Stop();
    }

    private void OnVampirismPressed()
    {
        if (abilityCoroutine != null)
            return;

        abilityCoroutine = StartCoroutine(StartAbility());
    }

    private IEnumerator StartAbility()
    {
        float elapsedTime = 0f;

        view.StartAbility();

        while (elapsedTime < abilityDuration)
        {
            damager.Tick(Time.deltaTime);
            view.Tick(elapsedTime, abilityDuration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        view.StartCooldown();

        float elapsedCooldown = 0f;

        while (elapsedCooldown < cooldownDuration)
        {
            view.Tick(elapsedCooldown, cooldownDuration);

            elapsedCooldown += Time.deltaTime;

            yield return null;
        }

        view.Stop();

        abilityCoroutine = null;
    }
}