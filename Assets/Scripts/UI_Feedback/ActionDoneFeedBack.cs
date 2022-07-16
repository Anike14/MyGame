using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDoneFeedBack : MonoBehaviour
{
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
        spriteRenderer.color = actionDoneColor;
    }

    public void StopFeedback() {
        spriteRenderer.color = originalColor;
    }

    public void WaitingTurn() {
        StopFeedback();
    }
}