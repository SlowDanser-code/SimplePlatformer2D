using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private const string IsRunning = "IsRunning";

    [SerializeField] private PlayerInputReader inputReader;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputReader.MovementInputChanged += OnMovementInputChanged;
    }

    private void OnDisable()
    {
        inputReader.MovementInputChanged -= OnMovementInputChanged;
    }

    private void OnMovementInputChanged(float direction)
    {
        animator.SetBool(IsRunning, direction != 0);
    }
}