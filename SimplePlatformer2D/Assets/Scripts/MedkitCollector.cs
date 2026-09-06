using UnityEngine;

[RequireComponent(typeof(Health))]
public class MedkitCollector : MonoBehaviour
{
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryCollect(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryCollect(collision);
    }

    private void TryCollect(Collider2D collision)
    {
        Medkit medkit = collision.GetComponentInParent<Medkit>();

        if (medkit != null)
        {
            medkit.TryUse(_health);
        }
    }
}
