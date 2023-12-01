using UnityEngine;

[RequireComponent(typeof(Collider))] // Обеспечивает наличие коллайдера у объекта
public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage = 1.0f; // Количество урона, которое оружие может нанести
    [HideInInspector] public static bool Activated = false;
    private bool canDealDamage = false;
    private bool hasCollidedThisAttack = false;

    private void Start()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true; // Устанавливает коллайдер как триггер
        }
        else
        {
            Debug.LogWarning("Weapon script requires a Collider component."); // Выводит предупреждение, если коллайдер отсутствует
        }
    }

    private void HandleAttackStateChange(bool isAttacking)
    {
        canDealDamage = isAttacking;
        if (!isAttacking)
        {
            // Сбросить флаг, позволяющий снова нанести урон, когда атака закончилась
            hasCollidedThisAttack = false;
        }
    }

    private void OnEnable()
    {
        PlayerBattleController.OnAttackStateChange += HandleAttackStateChange;
    }

    private void OnDisable()
    {
        PlayerBattleController.OnAttackStateChange -= HandleAttackStateChange;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage || hasCollidedThisAttack) return;

        LivingEntity entity = other.GetComponent<LivingEntity>();
        if (entity != null)
        {
            entity.TakeDamage(damage);
            hasCollidedThisAttack = true;
        }
    }
}
