using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Health))]
public class PlayerDeath : MonoBehaviour
{
    [FormerlySerializedAs("inputReader")]
    [SerializeField] private PlayerInputReader _inputReader;
    [FormerlySerializedAs("movement")]
    [SerializeField] private PlayerMovement _movement;
    [FormerlySerializedAs("attackController")]
    [SerializeField] private PlayerAttackController _attackController;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.Died += OnDied;
    }

    private void OnDisable()
    {
        _health.Died -= OnDied;
    }

    private void OnDied()
    {
        _movement.Stop();

        _inputReader.enabled = false;
        _movement.enabled = false;
        _attackController.enabled = false;

        Debug.Log("Player погиб");
    }
}