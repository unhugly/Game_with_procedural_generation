using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    private PlayerBattleController playerBattleController;
    private Vector3 originalScale;
    private float maxHealth;
    private bool initialized = false;

    void Start()
    {
        playerBattleController = GetComponentInParent<PlayerBattleController>();
        originalScale = transform.localScale;
        StartCoroutine(LateStart(0.2f));
    }

    IEnumerator LateStart(float value)
    {
        yield return new WaitForSeconds(value);
        maxHealth = playerBattleController.GetMaxHealth();
        initialized = true;
    }

    void Update()
    {
        if (initialized)
        {
            float currentHealth = playerBattleController.GetCurrentHealth();
            float healthScale = currentHealth / maxHealth;
            transform.localScale = new Vector3(healthScale * originalScale.x, originalScale.y, originalScale.z);
        }
    }
}

