using UnityEngine;

[DisallowMultipleComponent]
public class EntityRotator : MonoBehaviour
{
    public void Rotate(float direction)
    {
        if (direction == 0)
            return;

        // Reflect the whole hierarchy, including child transforms and colliders.
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction);
        transform.localScale = scale;
    }
}
