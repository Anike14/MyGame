using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPenetratedAnimationFeedBack : MonoBehaviour
{
    [SerializeField]
    private UnitBase tankObject;
    
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Color penetratedColor;

    [SerializeField]
    private Color destroyedColor;

    public void PlayFeedback()
    {
        if (tankObject == null) return;
        StartCoroutine(ScratchedCoroutine());
        tankObject.ActivateDestroyed();
    }

    private IEnumerator ScratchedCoroutine()
    {
        Color origin = spriteRenderer.color;
        spriteRenderer.color = penetratedColor;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = destroyedColor;
        StopAllCoroutines();
    }
}
