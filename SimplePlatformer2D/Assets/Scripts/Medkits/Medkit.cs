using UnityEngine;
using UnityEngine.Serialization;

public class Medkit : MonoBehaviour
{
    [FormerlySerializedAs("healAmount")]
    [SerializeField] private float _healAmount = 25f;

    public bool TryUse(Health health)
    {
        if (isActiveAndEnabled == false || health == null)
            return false;

        if (health.TryHeal(_healAmount) == false)
            return false;

        gameObject.SetActive(false);

        return true;
    }
}
