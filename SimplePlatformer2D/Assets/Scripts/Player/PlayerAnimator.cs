using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private const string _isRunning = "IsRunning";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetRunning(bool isRunning)
    {
        _animator.SetBool(_isRunning, isRunning);
    }
}
