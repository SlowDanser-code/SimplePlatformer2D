using UnityEngine;

public class Medkit : MonoBehaviour
{
    [SerializeField] private float healAmount = 25f;

    public bool TryUse(Health health)
    {
        if (isActiveAndEnabled == false || health == null)
            return false;

        if (health.TryHeal(healAmount) == false)
            return false;

        gameObject.SetActive(false);

        return true;
    }
}
