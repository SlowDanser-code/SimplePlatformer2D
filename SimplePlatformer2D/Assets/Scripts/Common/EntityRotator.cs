using UnityEngine;

[DisallowMultipleComponent]
public class EntityRotator : MonoBehaviour
{
    public void Rotate(float direction)
    {
        if (direction == 0)
            return;

        float angle = direction > 0 ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
