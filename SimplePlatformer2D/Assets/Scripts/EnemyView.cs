using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(EntityRotator))]
public class EnemyView : MonoBehaviour
{
    [FormerlySerializedAs("movement")]
    [SerializeField] private EnemyMovement _movement;

    private EntityRotator _rotator;

    private void Awake()
    {
        _rotator = GetComponent<EntityRotator>();
    }

    private void OnEnable()
    {
        _movement.DirectionChanged += OnDirectionChanged;
    }

    private void OnDisable()
    {
        _movement.DirectionChanged -= OnDirectionChanged;
    }

    private void OnDirectionChanged(float direction)
    {
        _rotator.Rotate(direction);
    }
}
