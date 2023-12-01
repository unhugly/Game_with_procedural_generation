using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBar : MonoBehaviour
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
        float currentArmor = livingEntity.GetCurrentArmor();
        float armorScale = currentArmor / maxHealth;
        transform.localScale = new Vector3(armorScale * originalScale.x, originalScale.y, originalScale.z);
        if (livingEntity.GetCurrentArmor() < maxArmor) GetComponent<MeshRenderer>().enabled = true;
        if (livingEntity.GetCurrentArmor() <= 0) GetComponent<MeshRenderer>().enabled = false;
    }
}
