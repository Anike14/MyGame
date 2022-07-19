using System.Collections;
using UnityEngine;

public class ConcealmentFeedback : MonoBehaviour
{
    [SerializeField]
    private UnitBase me;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Color originalColor;

    void Awake()
    {
        originalColor = spriteRenderer.color;
    }

    public void PlayFeedback() {
        if (me.IsDestroyed()) return;
        StartCoroutine(hide());
    }

    public void StopFeedbackByMyTurn() {
        if (me.IsDestroyed()) return;
        StartCoroutine(show());
    }

    public void StopFeedbackByScouted() {
        if (me.IsDestroyed()) return;
        StartCoroutine(FlashCoroutine());
        StartCoroutine(show());
    }

    private IEnumerator hide() {
        yield return new WaitForSeconds(0.01f);
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = 0f;
        spriteRenderer.color = spriteColor;
    }

    private IEnumerator show() {
        yield return new WaitForSeconds(0.01f);
        spriteRenderer.color = originalColor;
        
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = 1f;
        spriteRenderer.color = spriteColor;
    }

    private IEnumerator FlashCoroutine()
    {
        Color spriteColor = spriteRenderer.color;
        for (int i = 0; i < 5; i++) {
            spriteColor.a = 0;
            spriteRenderer.color = spriteColor;
            yield return new WaitForSeconds(0.3f);

            spriteColor.a = 1;
            spriteRenderer.color = spriteColor;
            yield return new WaitForSeconds(0.3f);
        }
    }
}