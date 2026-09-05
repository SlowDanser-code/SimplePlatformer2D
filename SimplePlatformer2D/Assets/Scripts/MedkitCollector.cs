using UnityEngine;

[RequireComponent(typeof(Health))]
public class MedkitCollector : MonoBehaviour
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
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
            medkit.TryUse(health);
        }
    }
}
