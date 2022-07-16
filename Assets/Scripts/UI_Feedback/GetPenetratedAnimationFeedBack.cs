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

    private Vector3 originalPosition;

    public void PlayFeedback()
    {
        if (tankObject == null) return;
        originalPosition = tankObject.transform.position;
        StartCoroutine(ScratchedCoroutine());
        tankObject.transform.position = originalPosition;
        tankObject.ActivateDestroyed();
    }

    private IEnumerator ScratchedCoroutine()
    {
        Color origin = spriteRenderer.color;
        spriteRenderer.color = penetratedColor;
        float xxx = tankObject.transform.position.x;
        for (int i = 0; i < 4; i++) {
            if (tankObject.transform.localScale.x > 0) 
                xxx += 0.03f;
            else xxx -= 0.03f;
            tankObject.transform.position = new Vector3(xxx, originalPosition.y, originalPosition.z);
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 8; i++) {
            if (tankObject.transform.localScale.x > 0) 
                xxx -= 0.03f;
            else xxx += 0.03f;
            tankObject.transform.position = new Vector3(xxx, originalPosition.y, originalPosition.z);
            yield return new WaitForSeconds(0.01f);
        }
        spriteRenderer.color = origin;
        for (int i = 0; i < 4; i++) {
            if (tankObject.transform.localScale.x > 0) 
                xxx += 0.03f;
            else xxx -= 0.03f;
            tankObject.transform.position = new Vector3(xxx, originalPosition.y, originalPosition.z);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = destroyedColor;
        StopAllCoroutines();
    }
}
