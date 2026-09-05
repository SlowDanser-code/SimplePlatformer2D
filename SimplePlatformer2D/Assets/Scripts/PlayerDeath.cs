using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerAttackController attackController;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.Died += OnDied;
    }

    private void OnDisable()
    {
        health.Died -= OnDied;
    }

    private void OnDied()
    {
        movement.Stop();

        inputReader.enabled = false;
        movement.enabled = false;
        attackController.enabled = false;

        Debug.Log("Player погиб");
    }
}