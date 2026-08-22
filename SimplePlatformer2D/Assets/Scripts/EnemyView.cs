using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyView : MonoBehaviour
{
    [SerializeField] private EnemyPatrol patrol;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        patrol.DirectionChanged += OnDirectionChanged;
    }

    private void OnDisable()
    {
        patrol.DirectionChanged -= OnDirectionChanged;
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