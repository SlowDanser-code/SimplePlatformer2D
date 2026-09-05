using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyView : MonoBehaviour
{
    [SerializeField] private EnemyMovement movement;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        movement.DirectionChanged += OnDirectionChanged;
    }

    private void OnDisable()
    {
        movement.DirectionChanged -= OnDirectionChanged;
    }

    private void OnDirectionChanged(float direction)
    {
        if (direction > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}