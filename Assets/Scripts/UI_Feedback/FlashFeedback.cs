using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFeedback : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer _spriteRenderer;
    [SerializeField]
    private float _invisibleTime, _visibleTime;

    public void PlayerFeedback() {
        if (this._spriteRenderer == null) return;
        StopFeedBack();
        StartCoroutine(FlashCoroutime());
    }

    private IEnumerator FlashCoroutime() {
        Color spriteColor = this._spriteRenderer.color;
        spriteColor.a = 0;
        this._spriteRenderer.color = spriteColor;
        yield return new WaitForSeconds(_invisibleTime);

        spriteColor.a = 1;
        this._spriteRenderer.color = spriteColor;
        yield return new WaitForSeconds(_visibleTime);

        StartCoroutine(FlashCoroutime());
    }

    public void StopFeedBack() {
        StopAllCoroutines();
        Color spriteColor = this._spriteRenderer.color;
        spriteColor.a = 1;
        this._spriteRenderer.color = spriteColor;
    }
}
