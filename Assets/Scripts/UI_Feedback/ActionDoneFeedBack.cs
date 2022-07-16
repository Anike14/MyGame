using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDoneFeedBack : MonoBehaviour
{
    [SerializeField]
    private UnitBase me;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Color actionDoneColor;
    
    private Color originalColor;

    private void Start()
    {
        originalColor = spriteRenderer.color;
    }

    public void PlayFeedback() {
        if (me.IsDestroyed()) return;
        spriteRenderer.color = actionDoneColor;
    }

    public void StopFeedback() {
        if (me.IsDestroyed()) return;
        spriteRenderer.color = originalColor;
    }

    public void WaitingTurn() {
        StopFeedback();
    }
}