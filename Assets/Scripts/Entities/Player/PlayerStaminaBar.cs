using System.Collections;
using UnityEngine;

public class PlayerStaminaBar : MonoBehaviour
{
    private PlayerController playerController;
    private Vector3 originalScale;
    private float maxStamina;
    private bool initialized = false;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        originalScale = transform.localScale;
        StartCoroutine(LateStart(0.2f));
    }

    IEnumerator LateStart(float value)
    {
        yield return new WaitForSeconds(value);
        maxStamina = playerController.GetMaxStamina();
        initialized = true;
    }

    void Update()
    {
        if (initialized)
        {
            float currentStamina = playerController.GetCurrentStamina();
            float staminaScale = currentStamina / maxStamina;
            transform.localScale = new Vector3(staminaScale * originalScale.x, originalScale.y, originalScale.z);
        }
    }
}
