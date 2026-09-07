using System;
using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    private const string _horizontalAxis = "Horizontal";
    private const string _jumpButton = "Jump";
    private const string _attackButton = "Fire1";

    public event Action<float> MovementInputChanged;
    public event Action JumpPressed;
    public event Action AttackPressed;

    private void OnDisable()
    {
        MovementInputChanged?.Invoke(0);
    }

    private void Update()
    {
        float direction = Input.GetAxisRaw(_horizontalAxis);

        MovementInputChanged?.Invoke(direction);

        if (Input.GetButtonDown(_jumpButton))
        {
            JumpPressed?.Invoke();
        }

        if (Input.GetButtonDown(_attackButton))
        {
            AttackPressed?.Invoke();
        }
    }
}