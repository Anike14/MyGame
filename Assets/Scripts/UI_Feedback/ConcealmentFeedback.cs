using UnityEngine;

public class ConcealmentFeedback : MonoBehaviour
{
    [SerializeField]
    private UnitBase me;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    private string originalLayerName;

    private void Start()
    {
        originalLayerName = spriteRenderer.sortingLayerName;
    }

    public void PlayFeedback() {
        if (me.IsDestroyed()) return;
        spriteRenderer.sortingLayerName = "Default";
    }

    public void StopFeedback() {
        if (me.IsDestroyed()) return;
        spriteRenderer.sortingLayerName = originalLayerName;
    }
}