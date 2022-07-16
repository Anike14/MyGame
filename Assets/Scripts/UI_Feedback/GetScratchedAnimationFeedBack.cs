using System.Collections;
using UnityEngine;

public class GetScratchedAnimationFeedBack : MonoBehaviour
{
    [SerializeField]
    private GameObject tankObject;
    
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Color scratchedColor;

    private Vector3 originalPosition;

    public void PlayFeedback()
    {
        if (tankObject == null) return;
        originalPosition = tankObject.transform.position;
        StartCoroutine(ScratchedCoroutine());
        tankObject.transform.position = originalPosition;
    }

    private IEnumerator ScratchedCoroutine()
    {
        Color origin = spriteRenderer.color;
        spriteRenderer.color = scratchedColor;
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
        for (int shaking = 5; shaking >= 0; shaking--) {
            for (int i = 0; i < 2; i++) {
                if (tankObject.transform.localScale.x > 0) 
                    xxx += 0.03f;
                else xxx -= 0.03f;
                tankObject.transform.position = new Vector3(xxx, originalPosition.y, originalPosition.z);
                yield return new WaitForSeconds(0.01f);
            }
            for (int i = 0; i < 4; i++) {
                if (tankObject.transform.localScale.x > 0) 
                    xxx -= 0.03f;
                else xxx += 0.03f;
                tankObject.transform.position = new Vector3(xxx, originalPosition.y, originalPosition.z);
                yield return new WaitForSeconds(0.01f);
            }
            for (int i = 0; i < 2; i++) {
                if (tankObject.transform.localScale.x > 0) 
                    xxx += 0.03f;
                else xxx -= 0.03f;
                tankObject.transform.position = new Vector3(xxx, originalPosition.y, originalPosition.z);
                yield return new WaitForSeconds(0.01f);
            }
        }
        StopAllCoroutines();
    }
}
