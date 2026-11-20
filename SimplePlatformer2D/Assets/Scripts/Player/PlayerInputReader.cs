using System;
using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    public event Action<float> MovementInputChanged;
    public event Action JumpPressed;
    public event Action AttackPressed;
    public event Action VampirismPressed;

    private void Update()
    {
        float direction = Input.GetAxisRaw("Horizontal");

        MovementInputChanged?.Invoke(direction);

        if (Input.GetButtonDown("Jump"))
        {
            JumpPressed?.Invoke();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            AttackPressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            VampirismPressed?.Invoke();
        }
    }
}