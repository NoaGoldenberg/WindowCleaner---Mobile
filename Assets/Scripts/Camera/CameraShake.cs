using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.2f;

    private Vector3 originalPos;
    private bool isShaking = false;
    
    public void TriggerShake()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        isShaking = true;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float shakeAmount = Mathf.Sin(Time.time * Mathf.PI * 2.0f / shakeDuration) * shakeMagnitude * Time.deltaTime;
            transform.localRotation = transform.rotation * Quaternion.Euler(0, 0, shakeAmount);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.rotation = Quaternion.identity;
        isShaking = false;
    }
}