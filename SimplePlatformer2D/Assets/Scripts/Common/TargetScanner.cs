using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TargetScanner : MonoBehaviour
{
    private readonly List<Collider2D> _targets = new List<Collider2D>();

    public IReadOnlyList<Collider2D> Scan(float radius, LayerMask targetLayer)
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(targetLayer);
        filter.useTriggers = false;
        Physics2D.OverlapCircle(transform.position, radius, filter, _targets);

        return _targets;
    }
}
