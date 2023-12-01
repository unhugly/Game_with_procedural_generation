using UnityEngine;

[RequireComponent(typeof(Collider))] // ������������ ������� ���������� � �������
public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage = 1.0f; // ���������� �����, ������� ������ ����� �������
    [HideInInspector] public static bool Activated = false;
    private bool canDealDamage = false;
    private bool hasCollidedThisAttack = false;

    private void Start()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true; // ������������� ��������� ��� �������
        }
        else
        {
            Debug.LogWarning("Weapon script requires a Collider component."); // ������� ��������������, ���� ��������� �����������
        }
    }

    private void HandleAttackStateChange(bool isAttacking)
    {
        canDealDamage = isAttacking;
        if (!isAttacking)
        {
            // �������� ����, ����������� ����� ������� ����, ����� ����� �����������
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
