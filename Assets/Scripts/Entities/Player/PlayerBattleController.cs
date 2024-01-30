using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class PlayerBattleController : MonoBehaviour
{
    [SerializeField] public float attackSpeed = 0.2f;
    [SerializeField] public float healthPoints = 1;
    [SerializeField] public Transform rightHand;
    [SerializeField] public Transform leftHand;
    [HideInInspector] public static event Action<bool> OnAttackStateChange;

    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private Transform childObject;
    private Vector3 originalScale;
    private Quaternion originalRotation;
    private float maxHealth;

    private void Awake()
    {
        maxHealth = healthPoints;
    }

    private void Update()
    {
        Attack();
        CheckDeath();
    }

    public static void Die()
    {
        SceneManager.LoadScene(0);
        Inventory.ClearInventory();
        PlayerWallet.ResetMoney();
        HungerBar.Feed(100);
    }

    private void CheckDeath()
    {
        if (healthPoints <= 0) Die();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking && rightHand.childCount > 0 && Time.time >= lastAttackTime + attackSpeed)
        {
            lastAttackTime = Time.time; // Обновляем время последней атаки

            childObject = rightHand.GetChild(0);
            originalScale = childObject.localScale;
            originalRotation = childObject.localRotation;
            StartCoroutine(AttackSequence());
        }
    }

    private IEnumerator AttackSequence()
    {
        isAttacking = true;
        OnAttackStateChange?.Invoke(true);

        if (childObject) childObject.localScale = originalScale * 1.5f;
        yield return new WaitForSeconds(0.01f);

        Quaternion targetRotation = Quaternion.Euler(135, 0, 0) * originalRotation;
        float elapsedTime = 0f;

        while (elapsedTime < attackSpeed / 2)
        {
            if (childObject) childObject.localRotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / attackSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (childObject) childObject.localRotation = targetRotation;

        // Сбрасываем таймер
        elapsedTime = 0f;

        // Возвращаем в исходное положение для имитации замаха
        while (elapsedTime < attackSpeed)
        {
            if (childObject) childObject.localRotation = Quaternion.Slerp(targetRotation, originalRotation, elapsedTime / attackSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (childObject)
        {
            childObject.localRotation = originalRotation;
            childObject.localScale = originalScale;
        }

        // Пауза перед следующей возможной атакой
        yield return new WaitForSeconds(0.1f);

        isAttacking = false;
        OnAttackStateChange?.Invoke(false);
    }

    public float GetCurrentHealth()
    {
        return healthPoints;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void Heal(float value)
    {
        healthPoints = Math.Min(GetCurrentHealth() + value, GetMaxHealth());
    }
}
