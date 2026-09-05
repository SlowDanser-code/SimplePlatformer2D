using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private Attack attack;

    private void OnEnable()
    {
        inputReader.AttackPressed += OnAttackPressed;
    }

    private void OnDisable()
    {
        inputReader.AttackPressed -= OnAttackPressed;
    }

    private void OnAttackPressed()
    {
        attack.TryAttack();
    }
}