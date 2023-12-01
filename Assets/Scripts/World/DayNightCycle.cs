using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum DayTime { Day, Night }

[RequireComponent(typeof(Light))]
public class DayNightCycle : MonoBehaviour
{
    [Range(1, 100)] public float speedMultiplier = 1f;
    public static DayTime time = DayTime.Day;
    public float maxAmbientIntensity = 0.55f;
    public float minAmbientIntensity = 0.05f;

    private bool isTransitioning = false;
    private const float BASE_ROTATION_SPEED = 360f / (20f * 60f); // 360 градусов за 20 минут

    private void Update()
    {
        float rotationAmount = BASE_ROTATION_SPEED * speedMultiplier * Time.deltaTime;
        transform.Rotate(Vector3.right * rotationAmount);

        UpdateAmbientIntensity();
    }

    private void UpdateAmbientIntensity()
    {
        float xRotation = transform.eulerAngles.x;

        if (xRotation > 200f && xRotation < 340f && time != DayTime.Night)
        {
            time = DayTime.Night;
            if (!isTransitioning) // Проверка, идет ли уже переход
            {
                StartCoroutine(FadeAmbientIntensity(minAmbientIntensity, 2f));
                isTransitioning = true;
            }
            speedMultiplier = 2f;
        }
        else if ((xRotation <= 200f || xRotation >= 340f) && time != DayTime.Day)
        {
            time = DayTime.Day;
            if (!isTransitioning) // Проверка, идет ли уже переход
            {
                StartCoroutine(FadeAmbientIntensity(maxAmbientIntensity, 2f));
                isTransitioning = true;
            }
            speedMultiplier = 1f;
        }
    }

    IEnumerator FadeAmbientIntensity(float targetIntensity, float duration)
    {
        float startIntensity = RenderSettings.ambientIntensity;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            RenderSettings.ambientIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
            yield return null;
        }

        RenderSettings.ambientIntensity = targetIntensity;
        isTransitioning = false; // Сброс флага перехода
    }
}

