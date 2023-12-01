using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    private LivingEntity livingEntity;
    private Vector3 originalScale;
    private float maxHealth;
    private float maxArmor;

    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        livingEntity = GetComponentInParent<LivingEntity>();
        originalScale = transform.localScale;
        maxHealth = livingEntity.GetCurrentHealth();
        maxArmor = livingEntity.GetCurrentArmor();
    }

    void Update()
    {
        float currentHealth = livingEntity.GetCurrentHealth();
        float healthScale = currentHealth / maxHealth;
        transform.localScale = new Vector3(healthScale * originalScale.x, originalScale.y, originalScale.z);
        if (livingEntity.GetCurrentArmor() < maxArmor || livingEntity.GetCurrentHealth() < maxHealth) GetComponent<MeshRenderer>().enabled = true;
    }
}
