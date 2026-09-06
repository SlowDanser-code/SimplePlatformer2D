using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAttackController : MonoBehaviour
{
    [FormerlySerializedAs("inputReader")]
    [SerializeField] private PlayerInputReader _inputReader;
    [FormerlySerializedAs("attack")]
    [SerializeField] private Attack _attack;

    private void OnEnable()
    {
        _inputReader.AttackPressed += OnAttackPressed;
    }

    private void OnDisable()
    {
        _inputReader.AttackPressed -= OnAttackPressed;
    }

    private void OnAttackPressed()
    {
        _attack.TryAttack();
    }
}