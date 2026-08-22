using System;
using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    public event Action<float> MovementInputChanged;
    public event Action JumpPressed;

    private void Update()
    {
        float direction = Input.GetAxisRaw("Horizontal");

        MovementInputChanged?.Invoke(direction);

        if (Input.GetButtonDown("Jump"))
        {
            JumpPressed?.Invoke();
        }
    }
}