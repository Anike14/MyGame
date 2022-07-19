using System.Collections;
using UnityEngine;

public class ScoutedFeedback : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer spriteRendrer;

    private float invisibleTime = 0.3f, visibleTime = 0.3f;

    public void PlayFeedback(bool ignoreThisEvent)
    {
        Debug.Log(ignoreThisEvent);
        if (ignoreThisEvent || spriteRendrer == null)
            return;
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        for (int i = 0; i < 5; i++) {
            Color spriteColor = spriteRendrer.color;
            spriteColor.a = 0;
            spriteRendrer.color = spriteColor;
            yield return new WaitForSeconds(invisibleTime);

            spriteColor.a = 1;
            spriteRendrer.color = spriteColor;
            yield return new WaitForSeconds(visibleTime);
        }
    }
}
