using System;
using System.Collections;

using UnityEngine;

public class FiringAnimationFeedBack : MonoBehaviour
{
    [SerializeField]
    private GameObject tankObject;
    private Vector3 originalPosition;
    public void PlayFeedback()
    {
        if (tankObject == null) return;
        originalPosition = tankObject.transform.position;
        StartCoroutine(FirePowerCoroutine(AfterFeedback));
    }

    private void AfterFeedback() {
        StopAllCoroutines();
        tankObject.transform.position = originalPosition;
    }

    private IEnumerator FirePowerCoroutine(Action callback)
    {
        float xxx = tankObject.transform.position.x;
        for (int i = 0; i < 4; i++) {
            if (tankObject.transform.localScale.x > 0) 
                xxx += 0.02f;
            else xxx -= 0.02f;
            tankObject.transform.position = new Vector3(xxx, originalPosition.y, originalPosition.z);
            yield return new WaitForSeconds(0.04f);
        }
        for (int i = 0; i < 4; i++) {
            if (tankObject.transform.localScale.x > 0) 
                xxx += 0.01f;
            else xxx -= 0.01f;
            tankObject.transform.position = new Vector3(xxx, originalPosition.y, originalPosition.z);
            yield return new WaitForSeconds(0.08f);
        }
        for (int i = 0; i < 6; i++) {
            if (tankObject.transform.localScale.x > 0) 
                xxx -= 0.02f;
            else xxx += 0.02f;
            tankObject.transform.position = new Vector3(xxx, originalPosition.y, originalPosition.z);
            yield return new WaitForSeconds(0.08f);
        }
        callback();
    }
}