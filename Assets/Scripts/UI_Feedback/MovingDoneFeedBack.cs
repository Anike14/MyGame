using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDoneFeedBack : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Color movingDoneColor;
    
    private Color originalColor;

    private void Start()
    {
        originalColor = spriteRenderer.color;
    }

    public void PlayFeedback() {
        spriteRenderer.color = movingDoneColor;
    }

    public void StopFeedback() {
        spriteRenderer.color = originalColor;
    }

    public void WaitingTurn() {
        StopFeedback();
    }
}