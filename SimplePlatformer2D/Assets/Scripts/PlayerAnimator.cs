using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private const string _isRunning = "IsRunning";

    [FormerlySerializedAs("inputReader")]
    [SerializeField] private PlayerInputReader _inputReader;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _inputReader.MovementInputChanged += OnMovementInputChanged;
    }

    private void OnDisable()
    {
        _inputReader.MovementInputChanged -= OnMovementInputChanged;
    }

    private void OnMovementInputChanged(float direction)
    {
        _animator.SetBool(_isRunning, direction != 0);
    }
}