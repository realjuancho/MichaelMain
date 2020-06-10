using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeSpeed = 10f;
    [Range(0.00f, 1.00f)]
    public float maxTimeShake = 0.1f;
    [Range(0.00f, 10.00f)]
    public float lightShake = 0.1f;
    [Range(0.00f, 10.00f)]
    public float mediumShake = 0.2f;
    [Range(0.00f, 10.00f)]
    public float strongShake = 0.3f;
   
    // Update is called once per frame
    void Update()
    {
        HandleShake();
    }

    public void RequestShake(float shakeTime, ShakeType type)
    {
        timeShaking += shakeTime;
        shakeType = type;
        timeShaking = Mathf.Clamp(timeShaking, 0, maxTimeShake);

    }

    public enum ShakeType { Strong, Medium, Light };
    float timeShaking = 0;
    ShakeType shakeType;
    Vector3 originalPosition;

    void HandleShake()
    {
   
        if (timeShaking > 0)
        {
            originalPosition = transform.localPosition;
        
            float x = 0, y = 0, magnitude = 0;

            switch (shakeType)
            {
                case ShakeType.Light:
                    magnitude = lightShake;
                    break;
                case ShakeType.Medium:
                    magnitude = mediumShake;
                    break;
                case ShakeType.Strong:
                    magnitude = strongShake;
                    break;
            }

            float proportion = timeShaking / maxTimeShake;

            x = Random.Range(-proportion, proportion) * magnitude ;
            y = Random.Range(-proportion, proportion) * magnitude ;


            Vector3 targetPosition = new Vector3(originalPosition.x +x, originalPosition.y +y, originalPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, shakeSpeed * Time.deltaTime);

            timeShaking -= Time.deltaTime;
        }
          

    }
}
