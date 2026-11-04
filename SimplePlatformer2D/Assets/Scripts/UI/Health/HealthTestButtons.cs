using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HealthTestButtons : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Button damageButton;
    [SerializeField] private Button healButton;

    [SerializeField] private float damage = 10f;
    [SerializeField] private float healAmount = 10f;

    private void OnEnable()
    {
        damageButton.onClick.AddListener(OnDamageButtonClicked);
        healButton.onClick.AddListener(OnHealButtonClicked);
    }

    private void OnDisable()
    {
        damageButton.onClick.RemoveListener(OnDamageButtonClicked);
        healButton.onClick.RemoveListener(OnHealButtonClicked);
    }

    private void OnDamageButtonClicked()
    {
        health.TakeDamage(damage);

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnHealButtonClicked()
    {
        health.TryHeal(healAmount);

        EventSystem.current.SetSelectedGameObject(null);
    }
}